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

public partial class EMR_Immunization_PrintPdf : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();

    StringBuilder sbNextHeader = new StringBuilder();
    StringBuilder sbtopheader = new StringBuilder();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            clsIVF objIVF = new clsIVF(sConString);
            DataSet ds = new DataSet();
            if (Request.QueryString["RegistrationID"] != null && !common.myStr(Request.QueryString["RegistrationID"]).Equals(string.Empty))
            {
                ViewState["RegistrationID"] = common.myStr(Request.QueryString["RegistrationID"]);
            }
            else
            {
                ViewState["RegistrationID"] = common.myStr(Session["RegistrationID"]);
            }


            if (Request.QueryString["RegistrationNo"] != null && !common.myStr(Request.QueryString["RegistrationNo"]).Equals(string.Empty))
            {
                ViewState["RegistrationNo"] = common.myStr(Request.QueryString["RegistrationNo"]);
            }
            else
            {
                ViewState["RegistrationNo"] = common.myStr(Session["RegistrationNo"]);

            }
            ViewState["EncounterNo"] = Session["EncounterNo"];

            int MarginTop = 0;
            int MarginBottom = 0;
            int MarginLeft = 0;
            int MarginRight = 0;


            clsParsePDFDischarge cPrint = new clsParsePDFDischarge();


            try
            {
                string strHTML = BindgvDueDateGrid().ToString();



                ds = objIVF.getPrintTemplateReportSetup("IM");
                if (ds.Tables[0].Rows.Count == 0)
                {
                    ds = objIVF.getPrintTemplateReportSetup("PT");
                }

                if (ds.Tables[0].Rows.Count > 0)
                {
                    MarginTop = common.myInt(ds.Tables[0].Rows[0]["MarginTop"]);
                    MarginBottom = common.myInt(ds.Tables[0].Rows[0]["MarginBottom"]);
                    MarginLeft = common.myInt(ds.Tables[0].Rows[0]["MarginLeft"]);
                    MarginRight = common.myInt(ds.Tables[0].Rows[0]["MarginRight"]);

                    //-------------------Added by Vinod----------------------------------------------
                    ViewState["ReportId"] = common.myInt(ds.Tables[0].Rows[0]["ReportId"]);
                    //-------------------Added by Vinod----------------------------------------------

                    if (common.myBool(ds.Tables[0].Rows[0]["ShowPrintByInPageFooter"]))
                    {
                        cPrint.FooterLeftText = "Printed By: " + common.myStr(Session["UserName"]);
                    }
                    if (common.myBool(ds.Tables[0].Rows[0]["ShowPrintDateInPageFooter"]))
                    {
                        cPrint.FooterCenterText = "Print Date & Time: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                    }

                    strHTML += getSignatureDetails(common.myBool(ds.Tables[0].Rows[0]["IsPrintDoctorSignature"]));


                    if (common.myLen(ds.Tables[0].Rows[0]["ReportFooterText"]) > 0)
                    {
                        cPrint.LowestFooter = common.myStr(ds.Tables[0].Rows[0]["ReportFooterText"]) + Environment.NewLine + " ";
                    }
                    else
                    {
                        cPrint.LowestFooter = " " + Environment.NewLine + " ";
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
                cPrint.HeaderHtml = string.Empty;

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
                if (!Ex.Message.Contains("Thread was being aborted"))
                {
                    objException.HandleException(Ex);
                }
            }
            finally
            {
                objIVF = null;
            }
        }

    }

    protected StringBuilder BindgvDueDateGrid()
    {
        DataSet ds = new DataSet();
        DataTable dtUniqueVaccAge = new DataTable();
        DataView dvUniqueVaccAge = new DataView();
        DataTable dtUniqueAgeRow = new DataTable();

        string currentAge = string.Empty;
        string previousAge = string.Empty;
        int rowindex = 0;
        StringBuilder sb = new StringBuilder();
        ds = new DataSet();
        Hashtable hashIn = new Hashtable();
        BaseC.EMRImmunization Immu = new BaseC.EMRImmunization(sConString);


        string EMRImmPrintBlankBatchNoAndExpiry = string.Empty;
        string dob = string.Empty;
        try
        {

            if (ViewState["RegistrationID"] != null && common.myStr(ViewState["RegistrationID"].ToString()) != "" && Convert.ToInt32(ViewState["RegistrationID"]) > 0)
            {
                string Reg = ViewState["RegistrationID"].ToString();
                dob = Immu.GetPatientDOB(Convert.ToInt32(ViewState["RegistrationID"]), Convert.ToInt16(Session["HospitalLocationID"]));

            }


            ds = Immu.GetPatientImmunizationDueDates(Convert.ToInt16(common.myInt(Session["HospitalLocationID"])), common.myInt(ViewState["RegistrationID"]), common.myStr(dob));
            EMRImmPrintBlankBatchNoAndExpiry = common.GetFlagValueHospitalSetup(Convert.ToInt16(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), "EMRImmPrintBlankBatchNoAndExpiry", sConString);

            dtUniqueVaccAge = ds.Tables[0].DefaultView.ToTable(true, "Age");
            sb.Append("<Div valign='top'  colspan='2'  style='text-align:center;font-family: " + common.myStr(hdnFontName.Value) + ";font-size:12pt;'><b>IMMUNIZATION RECORD</b></Div>");
            sb.Append("<br />");
            sb.Append("<table cellpadding = '5' cellspacing = '0'  border='1' style='font-size:9pt;' >");

            if (common.myStr(Session["FacilityName"]).ToUpper().Contains("CHAITANYA"))
            {
                sb.Append("<tr><td valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";'>Age</td><td valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";'>Name of Vaccination</td><td valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + "; '>Given Date</td><td valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + "; '>Brand</td></tr>");

                for (int idag = 0; idag < dtUniqueVaccAge.Rows.Count; idag++)
                {

                    previousAge = common.myStr(dtUniqueVaccAge.Rows[idag]["Age"]);
                    dvUniqueVaccAge = ds.Tables[0].DefaultView;
                    if (!string.IsNullOrEmpty(previousAge))
                    {
                        dvUniqueVaccAge.RowFilter = "Age = '" + previousAge + "'";
                    }
                    dtUniqueAgeRow = dvUniqueVaccAge.ToTable();

                    rowindex = 0;


                    for (int imd = 0; imd < dtUniqueAgeRow.Rows.Count; imd++)
                    {

                        currentAge = common.myStr(dtUniqueAgeRow.Rows[imd]["Age"]);
                        sb.Append("<tr>");

                        rowindex += 1;
                        if (rowindex == 1)
                        {
                            if (common.myStr(EMRImmPrintBlankBatchNoAndExpiry) == "Y")
                            {
                                sb.Append("<td width='25%' valign='top' RowSpan='" + dtUniqueAgeRow.Rows.Count + "' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + "; '>" + dtUniqueAgeRow.Rows[imd]["Age"] + "</td><td width='25%' valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + "; '>" + dtUniqueAgeRow.Rows[imd]["ImmunizationName"] + "</td><td width='25%' valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + "; '>" + dtUniqueAgeRow.Rows[imd]["GivenDate"] + "</td><td width='25%' valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + "; '>" + dtUniqueAgeRow.Rows[imd]["ItemBrandName"] + "</td></tr>");
                            }
                            else
                            {
                                sb.Append("<td width='25%' valign='top' RowSpan='" + dtUniqueAgeRow.Rows.Count + "' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + "; '>" + dtUniqueAgeRow.Rows[imd]["Age"] + "</td><td width='25%' valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + "; '>" + dtUniqueAgeRow.Rows[imd]["ImmunizationName"] + "</td><td width='25%' valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + "; '>" + dtUniqueAgeRow.Rows[imd]["GivenDate"] + "</td><td width='25%' valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + "; '>" + dtUniqueAgeRow.Rows[imd]["ItemBrandName"] + "</td></tr>");
                            }
                        }
                        else
                        {
                            if (common.myStr(EMRImmPrintBlankBatchNoAndExpiry) == "Y")
                            {
                                sb.Append("<td width='20%' valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + "; '>" + dtUniqueAgeRow.Rows[imd]["ImmunizationName"] + "</td><td width='15%' valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + "; '>" + dtUniqueAgeRow.Rows[imd]["GivenDate"] + "</td><td width='15%' valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + "; '>" + dtUniqueAgeRow.Rows[imd]["ItemBrandName"] + "</td</tr>");
                            }
                            else
                            {
                                sb.Append("<td width='20%' valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + "; '>" + dtUniqueAgeRow.Rows[imd]["ImmunizationName"] + "</td><td width='15%' valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + "; '>" + dtUniqueAgeRow.Rows[imd]["GivenDate"] + "</td><td width='15%' valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + "; '>" + dtUniqueAgeRow.Rows[imd]["ItemBrandName"] + "</td></tr>");
                            }

                        }
                    }
                }
            }
            else
            {

                sb.Append("<tr><td valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";'>Age</td><td valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";'>Vaccins</td><td valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + "; '> DueDate</td><td valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + "; '>GivenDate</td><td valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + "; '>Batch No</td><td valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + "; '>Brand</td><td valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + "; '>ExpiryDate</td></tr>");

                for (int idag = 0; idag < dtUniqueVaccAge.Rows.Count; idag++)
                {

                    previousAge = common.myStr(dtUniqueVaccAge.Rows[idag]["Age"]);
                    dvUniqueVaccAge = ds.Tables[0].DefaultView;
                    if (!string.IsNullOrEmpty(previousAge))
                    {
                        dvUniqueVaccAge.RowFilter = "Age = '" + previousAge + "'";
                    }
                    dtUniqueAgeRow = dvUniqueVaccAge.ToTable();

                    rowindex = 0;


                    for (int imd = 0; imd < dtUniqueAgeRow.Rows.Count; imd++)
                    {

                        currentAge = common.myStr(dtUniqueAgeRow.Rows[imd]["Age"]);
                        sb.Append("<tr>");

                        rowindex += 1;
                        if (rowindex == 1)
                        {
                            if (common.myStr(EMRImmPrintBlankBatchNoAndExpiry) == "Y")
                            {
                                sb.Append("<td width='15%' valign='top' RowSpan='" + dtUniqueAgeRow.Rows.Count + "' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + "; '>" + dtUniqueAgeRow.Rows[imd]["Age"] + "</td><td width='15%' valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + "; '>" + dtUniqueAgeRow.Rows[imd]["ImmunizationName"] + "</td><td width='15%' valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + "; '>" + dtUniqueAgeRow.Rows[imd]["ImmunizationdueDate"] + "</td><td width='15%' valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + "; '>" + dtUniqueAgeRow.Rows[imd]["GivenDate"] + "</td><td width='15%' valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + "; '>" + string.Empty + "</td><td width='15%' valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + "; '>" + dtUniqueAgeRow.Rows[imd]["ItemBrandName"] + "</td><td width='10%' valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";'>" + string.Empty + "</td></tr>");
                            }
                            else
                            {
                                sb.Append("<td width='15%' valign='top' RowSpan='" + dtUniqueAgeRow.Rows.Count + "' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + "; '>" + dtUniqueAgeRow.Rows[imd]["Age"] + "</td><td width='15%' valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + "; '>" + dtUniqueAgeRow.Rows[imd]["ImmunizationName"] + "</td><td width='15%' valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + "; '>" + dtUniqueAgeRow.Rows[imd]["ImmunizationdueDate"] + "</td><td width='15%' valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + "; '>" + dtUniqueAgeRow.Rows[imd]["GivenDate"] + "</td><td width='15%' valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + "; '>" + dtUniqueAgeRow.Rows[imd]["BatchNo"] + "</td><td width='15%' valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + "; '>" + dtUniqueAgeRow.Rows[imd]["ItemBrandName"] + "</td><td width='10%' valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";'>" + dtUniqueAgeRow.Rows[imd]["ExpiryDate"] + "</td></tr>");
                            }
                        }
                        else
                        {
                            if (common.myStr(EMRImmPrintBlankBatchNoAndExpiry) == "Y")
                            {
                                sb.Append("<td width='20%' valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + "; '>" + dtUniqueAgeRow.Rows[imd]["ImmunizationName"] + "</td><td width='15%' valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + "; '>" + dtUniqueAgeRow.Rows[imd]["ImmunizationdueDate"] + "</td><td width='15%' valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + "; '>" + dtUniqueAgeRow.Rows[imd]["GivenDate"] + "</td><td width='15%' valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + "; '>" + string.Empty + "</td><td width='15%' valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + "; '>" + dtUniqueAgeRow.Rows[imd]["ItemBrandName"] + "</td><td width='10%' valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";'>" + string.Empty + "</td></tr>");
                            }
                            else
                            {
                                sb.Append("<td width='20%' valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + "; '>" + dtUniqueAgeRow.Rows[imd]["ImmunizationName"] + "</td><td width='15%' valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + "; '>" + dtUniqueAgeRow.Rows[imd]["ImmunizationdueDate"] + "</td><td width='15%' valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + "; '>" + dtUniqueAgeRow.Rows[imd]["GivenDate"] + "</td><td width='15%' valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + "; '>" + dtUniqueAgeRow.Rows[imd]["BatchNo"] + "</td><td width='15%' valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + "; '>" + dtUniqueAgeRow.Rows[imd]["ItemBrandName"] + "</td><td width='10%' valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";'>" + dtUniqueAgeRow.Rows[imd]["ExpiryDate"] + "</td></tr>");
                            }

                        }
                    }
                }

            }

            sb.Append("</table>");

        }
        catch (Exception Ex)
        {

            objException.HandleException(Ex);
        }
        finally
        {
            hashIn = null;
            Immu = null;
            ds.Dispose();
            EMRImmPrintBlankBatchNoAndExpiry = string.Empty;
        }
        return sb;
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
                    IsPrintHospitalHeader = common.myBool(ds.Tables[0].Rows[0]["IsPrintHospitalHeader"]);

                    HeaderId = common.myInt(ds.Tables[0].Rows[0]["HeaderId"]);
                }
            }

            sb.Append("<div>");

            if (IsPrintHospitalHeader)
            {
                ds = new DataSet();
                //yogesh 16/08/2022
                if (Session["FacilityName"].Equals("PRACHI HOSPITAL "))
                {
                    ds = objivf.getFacility(common.myInt(Session["FacilityId"]), common.myInt(Session["HospitalLocationID"]), common.myStr(Session["FacilityName"]));
                }
                else
                {
                    ds = objivf.getFacility(common.myInt(Session["FacilityId"]), common.myInt(Session["HospitalLocationID"]));
                }
                if (ds.Tables[0].Rows.Count > 0)
                {
                    sb.Append("<table border='0' width='98%' cellpadding='1' cellspacing='1' style='font-size:9pt;margin-bottom:5px;'>");
                    for (int idx = 0; idx < ds.Tables[0].Rows.Count; idx++)
                    {
                        DataRow DR = ds.Tables[0].Rows[idx];

                        FileName = common.myStr(DR["LogoImagePath"]);
                        if (FileName != "")
                        {
                            SignImage = "<img width='60px' height='62px' src='" + Server.MapPath("~") + FileName + "' />";
                            strSingImagePath = Server.MapPath("~") + FileName;
                        }

                        NABHFileName = common.myStr(DR["NABHLogoImagePath"]);
                        if (NABHFileName != "")
                        {
                            NABHSignImage = "<img width='80px;' height='62px' src='" + Server.MapPath("~") + NABHFileName + "' />";
                            NABHstrSingImagePath = Server.MapPath("~") + NABHFileName;
                        }
                        sb.Append("<BR/>");
                        //sb.Append("<tr>");
                        //sb.Append("<td>&nbsp;</td>");
                        ////sb.Append("<td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td>");
                        //sb.Append("</tr>");

                        sb.Append("<tr>");

                        sb.Append("<td colspan='1' valign='top' align='left'>");
                        sb.Append("<table border='0' cellpadding='0' cellspacing='0'>");
                        sb.Append("<tr>");
                        sb.Append("<td valign='top' >" + (File.Exists(strSingImagePath) ? SignImage : string.Empty) + "</td>");
                        sb.Append("</tr>");
                        sb.Append("</table>");
                        sb.Append("</td>");
                        //yogesh 12/08/2022
                        sb.Append("<td colspan='6' valign='top' align='center'>");
                        sb.Append("<table border='0' cellpadding='0' cellspacing='0'>");

                        sb.Append("<tr>");
                        sb.Append("<td  align ='center' style='font-size:16pt' ><b>" + common.myStr(DR["FacilityName"]).Trim() + "</b></td>");
                        sb.Append("</tr>");


                        sb.Append("<tr>");
                        sb.Append("<td align ='center' style='font-size:12pt'>" + common.myStr(DR["Name2"]).Trim() + "</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td align ='center' style='font-size:11pt'>" + common.myStr(DR["HeaderLine3"]).Trim() + "</td>");
                        sb.Append("</tr>");


                        sb.Append("<tr>");
                        sb.Append("<td align ='center' style='font-size:9pt'>Toll Free:" + common.myStr(DR["TollFree"]) + " Mobile No: " + common.myStr(DR["ImmunizationHeaderMobNo"]) + "</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td align ='center'  style='font-size:9px'><b> Email : " + common.myStr(DR["EmailId"]) + " Website : " + common.myStr(DR["WebSite"]) + "</b></td>");
                        sb.Append("</tr>");
                        sb.Append("</table>");
                        sb.Append("</td>");

                        sb.Append("<td align='center' valign='top'>");
                        sb.Append("<table border='0' cellpadding='0' cellspacing='0'>");
                        sb.Append("<tr>");
                        sb.Append("<td valign='top' align='center'>" + (File.Exists(NABHstrSingImagePath) ? NABHSignImage : string.Empty) + "</td>");
                        sb.Append("</tr>");
                        sb.Append("</table>");
                        sb.Append("</td>");

                        sb.Append("</tr>");

                    }
                    sb.Append("</table>");
                }
            }
            else
            {
                sb.Append("<br />");
                sb.Append("<br />");
                sb.Append("<br />");
            }

            sb.Append("</div>");
            sb.Append("<hr />");
            sb.Append(getIVFPatient().ToString());
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
        BaseC.Patient patient = new BaseC.Patient(sConString);
        StringBuilder sb = new StringBuilder();
        //DataSet dspatientdetails = new DataSet();
        DataSet ds = new DataSet();
        clsIVF objivf = new clsIVF(sConString);
        try
        {
            ds = patient.getEMRPatientDetailsImmunizationPrint(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
                                          common.myStr(ViewState["RegistrationNo"]), common.myStr(ViewState["EncounterNo"]), common.myInt(Session["UserId"]), common.myInt(ViewState["RegistrationID"]));
            //---------------------New Code Added Starts--------------------------
            BaseC.Hospital objHospital = new BaseC.Hospital(sConString);
            DataSet dsHeader = new DataSet();


            if (common.myInt(ViewState["ReportId"]) > 0)
            {
                dsHeader = objHospital.GetReportHeader("", common.myInt(ViewState["ReportId"]));// header
            }
            else
            {
                dsHeader = objHospital.GetReportHeader("");// header
            }

            DataTable dtHeaderMerge = dsHeader.Tables[0];

            string headerRow = "<hr width='100%' size='1px' />";
            #region Header Section of Report

            if (dsHeader.Tables.Count > 0)
            {
                int RowNo = 0;
                string colspan = "";

                if (dsHeader.Tables[0].Rows.Count > 0)
                {

                    sb.Append("<table border='0' cellpadding='0' cellspacing='0'>");

                    int ColumnCount = common.myInt(dsHeader.Tables[0].Compute("MAX(ColNo)", string.Empty));

                    sb.Append("<tr><td colspan='" + (ColumnCount * 2) + "' >" + headerRow + "</td></tr>");

                    for (int i = 0; i < dsHeader.Tables[0].Rows.Count; i++)
                    {
                        if (RowNo == 0)
                        {
                            RowNo = common.myInt(dsHeader.Tables[0].Rows[i]["Rowno"]);
                            sb.Append("<tr align='left'>");

                            if (ColumnCount == 1)
                            {
                                //sb.Append("<td align='left'>" + common.myStr(dsHeader.Tables[0].Rows[i]["FieldCaption"]) + "</td>");
                                //sb.Append("<td align='left'></td></tr>");
                                dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=1 AND Rowno=" + RowNo;
                                if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
                                {
                                    sb.Append("<td width=10% align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;font-weight:bold;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</td>");
                                    sb.Append("<td width=20% align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td></tr>");
                                }
                                else
                                {
                                    sb.Append("<td width=10% align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;font-weight:bold;'>&nbsp;</td>");
                                    sb.Append("<td width=20% align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;'>&nbsp;</td>");
                                }
                            }

                            if (ColumnCount == 2)
                            {

                                dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=2 AND Rowno=" + RowNo;
                                if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
                                {
                                    i += 1;
                                    colspan = "";
                                }
                                else
                                {
                                    colspan = " colspan='3'";
                                }

                                dsHeader.Tables[0].DefaultView.RowFilter = "";

                                dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=1 AND Rowno=" + RowNo;
                                if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
                                {
                                    sb.Append("<td                 width=10% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;font-weight:bold;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</td>");
                                    sb.Append("<td " + colspan + " width=20% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td>");
                                }
                                else
                                {
                                    sb.Append("<td width=10% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;font-weight:bold;'>&nbsp;</td>");
                                    sb.Append("<td width=20% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;'>&nbsp;</td>");
                                }
                                dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=2 AND Rowno=" + RowNo;
                                if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
                                {
                                    sb.Append("<td align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;font-weight:bold;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</td>");
                                    sb.Append("<td align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td></tr>");
                                }
                                else
                                {
                                    sb.Append("<td align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;font-weight:bold;'>&nbsp;</td>");
                                    sb.Append("<td align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;'>&nbsp;</td>");
                                }
                            }

                            if (ColumnCount == 3)
                            {
                                i += 2;
                                dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=1 AND Rowno=" + RowNo;
                                if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
                                {
                                    sb.Append("<td width=10% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;font-weight:bold;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</td>");
                                    dtHeaderMerge.DefaultView.RowFilter = "ColNo=2 AND Rowno=" + RowNo;
                                    if (dtHeaderMerge.DefaultView.ToTable().Rows.Count > 0)// this for merge td
                                    {
                                        dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=1 AND Rowno=" + RowNo;
                                        sb.Append("<td width=20% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td>");
                                    }
                                    else
                                    {
                                        dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=1 AND Rowno=" + RowNo;
                                        sb.Append("<td colspan='3' valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td>");
                                    }
                                }
                                else
                                {
                                    sb.Append("<td width=10% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;font-weight:bold;'>&nbsp;</td>");
                                    sb.Append("<td width=20% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;'>&nbsp;</td>");
                                }
                                dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=2 AND Rowno=" + RowNo;
                                if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
                                {
                                    sb.Append("<td align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;font-weight:bold;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</td>");
                                    sb.Append("<td align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td>");
                                }
                                //else
                                //{
                                //    sb.Append("<td align='left' valign='top' style='font-family: "+ common.myStr(hdnFontName.Value) +";font-size:9pt;'></td>");
                                //    sb.Append("<td align='left' valign='top' style='font-family: "+ common.myStr(hdnFontName.Value) +";font-size:9pt;'></td>");
                                //}
                                dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=3 AND Rowno=" + RowNo;
                                if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
                                {
                                    sb.Append("<td align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;font-weight:bold;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</td>");
                                    sb.Append("<td align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td></tr>");
                                }
                                else
                                {
                                    sb.Append("<td align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;font-weight:bold;'>&nbsp;</td>");
                                    sb.Append("<td align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;'>&nbsp;</td>");
                                }
                            }
                        }
                        else if (RowNo == common.myInt(dsHeader.Tables[0].Rows[i]["Rowno"]))
                        {
                            sb.Append("<tr align='left'>");
                            if (ColumnCount == 1)
                            {
                                dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=1 AND Rowno=" + RowNo;
                                if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
                                {
                                    sb.Append("<td width=10% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;font-weight:bold;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</td>");
                                    sb.Append("<td width=20% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td></tr>");
                                }
                                else
                                {
                                    sb.Append("<td width=10% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;font-weight:bold;'>&nbsp;</td>");
                                    sb.Append("<td width=20% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;'>&nbsp;</td>");
                                }


                            }
                            if (ColumnCount == 2)
                            {
                                dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=2 AND Rowno=" + RowNo;
                                if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
                                {
                                    i += 1;
                                    colspan = "";
                                }
                                else
                                {
                                    colspan = " colspan='3'";
                                }
                                dsHeader.Tables[0].DefaultView.RowFilter = "";

                                dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=1 AND Rowno=" + RowNo;
                                if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
                                {
                                    sb.Append("<td                 width=10% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;font-weight:bold;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</td>");
                                    sb.Append("<td " + colspan + " width=20% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td>");
                                }
                                else
                                {
                                    sb.Append("<td width=10% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;font-weight:bold;'>&nbsp;</td>");
                                    sb.Append("<td width=20% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;'>&nbsp;</td>");
                                }
                                dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=2 AND Rowno=" + RowNo;
                                if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
                                {
                                    sb.Append("<td align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;font-weight:bold;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</td>");
                                    sb.Append("<td align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td></tr>");
                                }
                                //else
                                //{
                                //    sb.Append("<td align='left' valign='top' style='font-family: "+ common.myStr(hdnFontName.Value) +";font-size:9pt;'>&nbsp;</td>");
                                //    sb.Append("<td align='left' valign='top' style='font-family: "+ common.myStr(hdnFontName.Value) +";font-size:9pt;'>&nbsp;</td>");
                                //}

                            }
                            if (ColumnCount == 3)
                            {
                                i += 2;

                                dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=1 AND Rowno=" + RowNo;
                                if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
                                {
                                    sb.Append("<td width=10% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;font-weight:bold;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</td>");

                                    dtHeaderMerge.DefaultView.RowFilter = "ColNo=2 AND Rowno=" + RowNo;
                                    if (dtHeaderMerge.DefaultView.ToTable().Rows.Count > 0)// this for merge td
                                    {
                                        dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=1 AND Rowno=" + RowNo;
                                        sb.Append("<td width=20% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td>");
                                    }
                                    else
                                    {
                                        dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=1 AND Rowno=" + RowNo;
                                        sb.Append("<td colspan='3' valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td>");
                                        // sb.Append("<td  colspan='3' align='left' style='font-family: "+ common.myStr(hdnFontName.Value) +";font-size:9pt;'></td>");
                                    }
                                }
                                else
                                {
                                    sb.Append("<td width=10% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;font-weight:bold;'>&nbsp;</td>");
                                    sb.Append("<td width=20% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;'>&nbsp;</td>");
                                }
                                dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=2 AND Rowno=" + RowNo;
                                if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
                                {
                                    sb.Append("<td align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;font-weight:bold;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</td>");
                                    sb.Append("<td align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td>");

                                }
                                //else
                                //{
                                //    sb.Append("<td align='left' valign='top' style='font-family: "+ common.myStr(hdnFontName.Value) +";font-size:9pt;'></td>");
                                //    sb.Append("<td align='left' valign='top' style='font-family: "+ common.myStr(hdnFontName.Value) +";font-size:9pt;'></td>");
                                //}
                                dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=3 AND Rowno=" + RowNo;
                                if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
                                {
                                    sb.Append("<td align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;font-weight:bold;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</td>");
                                    sb.Append("<td align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td></tr>");
                                }
                                else
                                {
                                    sb.Append("<td align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;font-weight:bold;'>&nbsp;</td>");
                                    sb.Append("<td align='left' valign='top' style='font-family:" + common.myStr(hdnFontName.Value) + ";font-size:11pt;'>&nbsp;</td>");
                                }
                            }
                        }
                        else
                        {
                            sb.Append("<tr align='left'>");
                            if (ColumnCount == 1)
                            {
                                //sb.Append("<td align='left'>" + common.myStr(dr["FieldCaption"]) + "</td>");
                                //sb.Append("<td align='left'></td></tr>");
                                dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=1 AND Rowno=" + RowNo;
                                if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
                                {
                                    sb.Append("<td width=10% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;font-weight:bold;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</td>");
                                    sb.Append("<td width=20% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td></tr>");
                                }
                                else
                                {
                                    sb.Append("<td width=10% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;font-weight:bold;'>&nbsp;</td>");
                                    sb.Append("<td width=20% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;'>&nbsp;</td>");
                                }
                            }
                            if (ColumnCount == 2)
                            {
                                //i = +1;
                                //sb.Append("<td align='left'>" + common.myStr(dr["FieldCaption"]) + "</td>");
                                //sb.Append("<td align='left'>dsadas</td>");
                                //sb.Append("<td align='left'>" + common.myStr(dr["FieldCaption"]) + "</td>");
                                //sb.Append("<td align='left'>Male</td></tr>");

                                dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=2 AND Rowno=" + RowNo;
                                if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
                                {
                                    i += 1;
                                    colspan = "";
                                }
                                else
                                {
                                    colspan = " colspan='3'";
                                }
                                dsHeader.Tables[0].DefaultView.RowFilter = "";

                                dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=1 AND Rowno=" + RowNo;
                                if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
                                {
                                    sb.Append("<td                 width=10% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;font-weight:bold;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</td>");
                                    sb.Append("<td " + colspan + " width=20% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td>");
                                }
                                else
                                {
                                    sb.Append("<td width=10% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;font-weight:bold;'>&nbsp;</td>");
                                    sb.Append("<td width=20% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;'>&nbsp;</td>");
                                }
                                dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=2 AND Rowno=" + RowNo;
                                if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
                                {
                                    sb.Append("<td align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;font-weight:bold;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</td>");
                                    sb.Append("<td align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td></tr>");
                                }
                                //else
                                //{
                                //    sb.Append("<td align='left' valign='top' style='font-family: "+ common.myStr(hdnFontName.Value) +";font-size:9pt;'>&nbsp;</td>");
                                //    sb.Append("<td align='left' valign='top' style='font-family: "+ common.myStr(hdnFontName.Value) +";font-size:9pt;'>&nbsp;</td>");
                                //}
                            }
                            if (ColumnCount == 3)
                            {
                                i += 2;
                                dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=1 AND Rowno=" + RowNo;
                                if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
                                {
                                    sb.Append("<td width=10% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;font-weight:bold;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</td>");
                                    dtHeaderMerge.DefaultView.RowFilter = "ColNo=2 AND Rowno=" + RowNo;
                                    if (dtHeaderMerge.DefaultView.ToTable().Rows.Count > 0)// this for merge td
                                    {
                                        dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=1 AND Rowno=" + RowNo;
                                        sb.Append("<td width=20% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td>");
                                    }
                                    else
                                    {
                                        dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=1 AND Rowno=" + RowNo;
                                        sb.Append("<td colspan='3' valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td>");
                                    }
                                }
                                else
                                {
                                    sb.Append("<td width=10% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;font-weight:bold;'>&nbsp;</td>");
                                    sb.Append("<td width=20% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;'>&nbsp;</td>");
                                }
                                dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=2 AND Rowno=" + RowNo;

                                if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
                                {
                                    sb.Append("<td align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;font-weight:bold;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</td>");
                                    sb.Append("<td align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td>");
                                }
                                //else
                                //{
                                //    sb.Append("<td align='left' style='font-family: "+ common.myStr(hdnFontName.Value) +";font-size:9pt;'></td>");
                                //    sb.Append("<td align='left' style='font-family: "+ common.myStr(hdnFontName.Value) +";font-size:9pt;'></td>");
                                //}
                                dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=3 AND Rowno=" + RowNo;
                                if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
                                {
                                    sb.Append("<td align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;font-weight:bold;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</td>");
                                    sb.Append("<td align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td></tr>");
                                }
                                else
                                {
                                    sb.Append("<td align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;font-weight:bold;'>&nbsp;</td>");
                                    sb.Append("<td align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;'>&nbsp;</td>");
                                }
                            }

                        }
                        //if (i + 1 != common.myInt(dsHeader.Tables[0].Rows.Count))
                        if ((i + 1) < common.myInt(dsHeader.Tables[0].Rows.Count))
                        {
                            RowNo = common.myInt(dsHeader.Tables[0].Rows[i + 1]["Rowno"]);

                            sb.Append("<tr><td colspan='6'>" + headerRow + "</td></tr>");

                        }

                        if ((RowNo - 1) < 3)
                        {
                            sbNextHeader = new StringBuilder();
                            sbNextHeader.Append(sb.ToString());

                            sbNextHeader.Append("</table>");
                        }
                    }
                    sb.Append("</table>");

                    sb.Append("<table cellpadding='0' cellspacing='0'><tr><td>" + headerRow + "</td></tr></table>");


                }
            }

            #endregion


        }
        catch (Exception Ex)
        {
            objException.HandleException(Ex);
        }
        finally
        {
            ds.Dispose();
            //dspatientdetails.Dispose();
            objivf = null;
        }
        return sb;

    }
    public string Addvalue(string Caption, DataSet ds)
    {
        string value = string.Empty;
        try
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                switch (common.myStr(Caption).ToUpper().Trim())
                {
                    case "PN"://Patient Name
                        value = common.myStr(ds.Tables[0].Rows[0]["PatientName"]);
                        break;
                    case "PMN"://Patient Mobile No  // yogesh 09/08/2022
                        value = common.myStr(ds.Tables[0].Rows[0]["MobileNo"]);
                        break;
                    case "RN"://Registration Number
                        value = common.myStr(ds.Tables[0].Rows[0]["RegistrationNo"]);
                        break;
                    case "SD"://Sample Collected Date
                        value = common.myStr(ds.Tables[0].Rows[0]["SampleCollectedDate"]);
                        break;
                    case "PGA"://Patient Age
                        value = common.myStr(ds.Tables[0].Rows[0]["Age"]);
                        break;
                    case "EN"://Encounter Number
                        value = common.myStr(ds.Tables[0].Rows[0]["EncounterNo"]);
                        break;
                    case "RD"://Result Date
                        value = common.myStr(ds.Tables[0].Rows[0]["ResultDate"]);
                        break;
                    case "BNW"://Ward
                        value = common.myStr(ds.Tables[0].Rows[0]["Ward"]);
                        break;
                    case "LN"://Lab No
                        value = common.myStr(ds.Tables[0].Rows[0]["LabNo"]);
                        break;
                    case "RS"://Report Status
                        value = common.myStr(ds.Tables[0].Rows[0]["ReportStatus"]);
                        break;
                    case "RB"://Referred By
                        value = common.myStr(ds.Tables[0].Rows[0]["Referred By"]);
                        break;
                    case "PM"://Nationality Name
                        value = common.myStr(ds.Tables[0].Rows[0]["Nationality_Name"]);
                        break;
                    case "CN"://Company Name                    
                    case "CMN"://Company Name
                        value = common.myStr(ds.Tables[0].Rows[0]["CompanyName"]);
                        break;
                    case "DOA"://Date Of Admission
                        if (common.myStr(Request.QueryString["For"]) == "DISSUM")
                        {
                            if (common.myStr(ds.Tables[0].Rows[0]["DOA"]).Length > 10)
                            {
                                value = common.myDate(ds.Tables[0].Rows[0]["DOA"]).ToString("dd/MM/yyyy hh:mm tt");
                            }
                            else
                            {
                                value = common.myStr(ds.Tables[0].Rows[0]["DOA"]);
                            }
                        }
                        else
                        {
                            value = common.myStr(ds.Tables[0].Rows[0]["DOA"]);
                        }
                        break;
                    case "DOD"://Date Of Discharge
                        if (common.myStr(Request.QueryString["For"]) == "DISSUM")
                        {
                            value = common.myStr(ds.Tables[0].Rows[0]["DOD"]);
                        }
                        else
                        {
                            value = common.myStr(ds.Tables[0].Rows[0]["DOD"]);
                        }
                        break;
                    case "CDN"://Consulting Doctor Name                    
                    case "EDC":
                        value = common.myStr(ds.Tables[0].Rows[0]["ConsultingDoctor"]);
                        break;
                    case "CDD"://Consulting Doctor Designation                  
                    case "PDE":
                        value = common.myStr(ds.Tables[0].Rows[0]["ConsultingDoctorDesignation"]);
                        break;
                    case "CND"://Consulting Doctor Department              
                    case "PDN":
                        value = common.myStr(ds.Tables[0].Rows[0]["ConsultingDoctorDepartment"]);
                        break;
                    case "PAG"://Patient Of Gender
                        value = common.myStr(ds.Tables[0].Rows[0]["Age/Gender"]);
                        break;
                    case "BNO"://Bed Number
                        value = common.myStr(ds.Tables[0].Rows[0]["BedNo"]);
                        break;
                    case "BCM"://Bed Category
                        value = common.myStr(ds.Tables[0].Rows[0]["BedCategoryName"]);
                        break;
                    case "PADD"://Patient Address
                        value = common.myStr(ds.Tables[0].Rows[0]["PatientAddress"]);
                        break;
                    case "PSP"://Provider Speciality
                        value = common.myStr(ds.Tables[0].Rows[0]["SpecializationName"]);
                        break;
                    case "MN"://Mother Name
                        value = common.myStr(ds.Tables[0].Rows[0]["MotherName"]);
                        break;
                    case "FN"://Father Name
                        value = common.myStr(ds.Tables[0].Rows[0]["FatherName"]);
                        break;
                    case "SDN"://Secondary Doctor Name
                        value = common.myStr(ds.Tables[0].Rows[0]["SecondaryDoctorName"]);
                        break;
                    case "DST"://Discharge Status
                        value = common.myStr(ds.Tables[0].Rows[0]["DischargeStatus"]);
                        break;
                    case "DDT"://Delivery Date Time
                        value = common.myStr(ds.Tables[0].Rows[0]["DeliveryDateTime"]);
                        break;
                    case "GN"://Guardian Name
                        value = common.myStr(ds.Tables[0].Rows[0]["GuardianName"]);
                        break;
                    case "PVT": // PatientVisit
                        value = common.myStr(ds.Tables[0].Rows[0]["PatientVisit"]);
                        break;
                    case "MLC": // PatientVisit
                        value = common.myStr(ds.Tables[0].Rows[0]["MLC"]);
                        break;
                    case "SN": // PatientVisit
                        value = common.myStr(ds.Tables[0].Rows[0]["SponserName"]);
                        break;
                    case "PC":
                        value = common.myStr(ds.Tables[0].Rows[0]["Patientcategory"]);
                        break;
                    case "REL":
                        value = common.myStr(ds.Tables[0].Rows[0]["ReligionName"]);
                        break;
                    case "OCC":
                        value = common.myStr(ds.Tables[0].Rows[0]["OccupationName"]);
                        break;
                    case "ROR":
                        value = common.myStr(ds.Tables[0].Rows[0]["RegistrationOtherRemarks"]);
                        break;
                    case "DOB":
                        value = common.myStr(ds.Tables[0].Rows[0]["DateOfBirth"]);
                        break;
                    case "EDT":
                        value = common.myStr(ds.Tables[0].Rows[0]["EncounterDate"]);
                        break;
                    default:
                        value = string.Empty;
                        break;
                }
            }

        }
        catch
        {
        }
        return value;
    }
    private string getSignatureDetails(bool IsPrintDoctorSignature)
    {
        string sImage = "";
        StringBuilder sbSign = new StringBuilder();
        string FileName = string.Empty;

        StringBuilder strSQL = new StringBuilder();
        String strSingImagePath = "";
        Hashtable hstInput = new Hashtable();
        string SignImage = "";
        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        DataSet ds = new DataSet();
        try
        {
            if (IsPrintDoctorSignature)
            {
                if (common.myInt(Session["DoctorID"]) > 0)
                {
                    hstInput.Add("@intId", common.myInt(Session["DoctorID"]));

                    strSQL.Append("SELECT TOP 1  ISNULL(tm.Name + ' ','') + ISNULL(e.FirstName,'') + ISNULL(' ' + e.MiddleName,'') + ISNULL(' ' + e.LastName,'') AS DoctorName, ");
                    strSQL.Append(" e.Education AS DoctorEducation, e.Designation AS DoctorDesignation, ");
                    strSQL.Append(" e.ID AS DoctorId, dd.UPIN, spm.Name AS DoctorSpecialization, GETUTCDATE() AS SignatureWithDateTime, ");
                    strSQL.Append(" esi.ImageId, esi.SignatureImage, esi.ImageType ,dd.SignatureLine1 ,dd.SignatureLine2,dd.SignatureLine3,dd.SignatureLine4");
                    strSQL.Append(" FROM Employee e WITH(NOLOCK) ");
                    strSQL.Append(" LEFT OUTER JOIN DoctorDetails dd WITH(NOLOCK) ON e.ID = dd.DoctorId ");
                    strSQL.Append(" LEFT OUTER JOIN SpecialisationMaster spm WITH(NOLOCK) ON dd.SpecialisationId = spm.Id AND spm.Active = 1 ");
                    strSQL.Append(" LEFT OUTER JOIN EmployeeSignatureImage esi WITH(NOLOCK) ON esi.EmployeeId = e.ID AND esi.Active = 1 ");
                    strSQL.Append(" LEFT JOIN TitleMaster tm WITH (NOLOCK) ON tm.TitleID = e.TitleId ");
                    strSQL.Append(" WHERE e.ID = @intId ");
                    strSQL.Append(" AND e.Active = 1 ");
                    strSQL.Append(" ORDER BY esi.ImageId DESC ");

                    ds = dl.FillDataSet(CommandType.Text, strSQL.ToString(), hstInput);
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
                        sbSign.Append("<table cellspacing='0' cellpadding='0' border='0' style='font-family: Candara;font-size:10pt;'>");
                        sbSign.Append("<tr><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>");

                        sbSign.Append("<tr>");
                        sbSign.Append("<td></td><td></td><td></td><td></td>");
                        sbSign.Append("<td colspan='3' align='left'>");
                        sbSign.Append(hdnDoctorImage.Value.ToString());
                        sbSign.Append("</td>");
                        sbSign.Append("</tr>");

                        sbSign.Append("<tr>");
                        sbSign.Append("<td></td><td></td><td></td><td></td>");
                        sbSign.Append("<td colspan='3' valign='top' align='left'>");
                        sbSign.Append(common.myStr(ds.Tables[0].Rows[0]["DoctorName"]).Trim() + " " + ", <SPAN style='font-family: Candara;font-size:9pt;'>" + common.myStr(ds.Tables[0].Rows[0]["DoctorEducation"]).Trim() + "</SPAN>");
                        sbSign.Append("</td>");
                        sbSign.Append("</tr>");



                        if (common.myStr(ds.Tables[0].Rows[0]["SignatureLine1"]).Trim().Length > 0)
                        {
                            sbSign.Append("<tr>");
                            sbSign.Append("<td></td><td></td><td></td><td></td>");
                            sbSign.Append("<td colspan='3' align ='left'>" + common.myStr(ds.Tables[0].Rows[0]["SignatureLine1"]).Trim() + "</td>");
                            sbSign.Append("</tr>");
                        }
                        if (common.myStr(ds.Tables[0].Rows[0]["SignatureLine2"]).Trim().Length > 0)
                        {
                            sbSign.Append("<tr>");
                            sbSign.Append("<td></td><td></td><td></td><td></td>");
                            sbSign.Append("<td colspan='3' align ='left'>" + common.myStr(ds.Tables[0].Rows[0]["SignatureLine2"]).Trim() + "</td>");
                            sbSign.Append("</tr>");
                        }
                        if (common.myStr(ds.Tables[0].Rows[0]["SignatureLine3"]).Trim().Length > 0)
                        {
                            sbSign.Append("<tr>");
                            sbSign.Append("<td></td><td></td><td></td><td></td>");
                            sbSign.Append("<td colspan='3' align ='left'>" + common.myStr(ds.Tables[0].Rows[0]["SignatureLine3"]).Trim() + "</td>");
                            sbSign.Append("</tr>");
                        }
                        if (common.myStr(ds.Tables[0].Rows[0]["SignatureLine4"]).Trim().Length > 0)
                        {
                            sbSign.Append("<tr>");
                            sbSign.Append("<td></td><td></td><td></td><td></td>");
                            sbSign.Append("<td colspan='3' align ='left'>" + common.myStr(ds.Tables[0].Rows[0]["SignatureLine4"]).Trim() + "</td>");
                            sbSign.Append("</tr>");
                        }

                        sbSign.Append("</table>");
                    }
                    else
                    {
                        sbSign.Append("<table cellspacing='0' cellpadding='0' border='0' style='font-family: Candara;font-size:10pt;'>");
                        sbSign.Append("<tr><td></td></tr>");
                        sbSign.Append("<tr><td></td></tr>");
                        sbSign.Append("<tr><td align='right'><b>Consultant&nbsp;Signature</b></td></tr>");
                        sbSign.Append("</table>");
                    }
                }
            }
            else
            {
                sbSign.Append("<table cellspacing='0' cellpadding='0' border='0' style='font-family: Candara;font-size:10pt;'>");
                sbSign.Append("<tr><td></td></tr>");
                sbSign.Append("<tr><td></td></tr>");
                sbSign.Append("<tr><td align='right'><b>Consultant&nbsp;Signature</b></td></tr>");
                sbSign.Append("</table>");
            }
        }
        catch (Exception Ex)
        {
            objException.HandleException(Ex);
        }
        finally
        {
            strSQL = null;
            hstInput = null;
            dl = null;
            ds.Dispose();
        }

        return sbSign.ToString();
    }
}