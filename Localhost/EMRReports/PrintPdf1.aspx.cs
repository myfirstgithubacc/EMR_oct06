using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.UI;
using System.IO;
using System.Text;
using System.Net;

public partial class EMRReports_PrintPdf1 : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    private static String FileName = "";
    private static string strimgData = "";
    private static String Education = "";
    string ImageTextPrintAfterDoctorSignatureonDischargeSummary = "";

    //yogesh11/05/2022
    private string ftppath = ConfigurationManager.AppSettings["FTP"];
    private string FileFolder = ConfigurationManager.AppSettings["FileFolder"];

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            StringBuilder sb = new StringBuilder();
            StringBuilder sbNextHeader = new StringBuilder();
            string SOURCE = common.myStr(Request.QueryString["SOURCE"]);
            string LABNO = common.myStr(Request.QueryString["LABNO"]);

            ViewState["SignaturePrintOnlyInFinalization"] = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]),
                                                            common.myInt(Session["FacilityId"]), "SignaturePrintOnlyInFinalization", sConString);
            ImageTextPrintAfterDoctorSignatureonDischargeSummary = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), "ImageTextPrintAfterSignatureonDischargeSummary", sConString);



            ViewState["FlagShowDepartmentInDischargeSummary"] = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), "ShowDepartmentInDischargeSummary", sConString);



            ViewState["IsShowAllDepartmentDoctorsInDischargeSummary"] = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]),
                                                                        common.myInt(Session["FacilityId"]), "IsShowAllDepartmentDoctorsInDischargeSummary", sConString);

            ViewState["ShowHeaderOnAllPagesOfDischargeSummary"] = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]),
                                                common.myInt(Session["FacilityId"]), "ShowHeaderOnAllPagesOfDischargeSummary", sConString);


            ViewState["ShowHeaderAllDetailOnAllPagesOfDischargeSummary"] = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]),
                                                                        common.myInt(Session["FacilityId"]), "ShowHeaderAllDetailOnAllPagesOfDischargeSummary", sConString);

            ViewState["IsShowOtherDoctorInDischargeSummary"] = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]),
                                                            common.myInt(Session["FacilityId"]), "IsShowOtherDoctorInDischargeSummary", sConString);

            ViewState["IsShowAllDepartmentDoctorsInDeathSummary"] = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]),
                                                                        common.myInt(Session["FacilityId"]), "IsShowAllDepartmentDoctorsInDeathSummary", sConString);

            hdnFontName.Value = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), "DischargeSummaryFont", sConString);
            if (common.myStr(hdnFontName.Value).Equals(string.Empty))
            {
                hdnFontName.Value = "Candara";
            }

            hdnFontSize.Value = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), "DischargeSummaryFontSize", sConString);
            if (common.myStr(hdnFontSize.Value).Equals(string.Empty))
            {
                hdnFontSize.Value = "9";
            }

            BindSummary bnotes = new BindSummary(sConString);
            DataSet ds = bnotes.GetPrintDischargeSummary(common.myInt(Request.QueryString["EncId"]), common.myInt(HttpContext.Current.Session["HospitalLocationId"]),
                                           common.myInt(Request.QueryString["RegId"]), common.myInt(Request.QueryString["ReportId"]),
                                           common.myInt(HttpContext.Current.Session["FacilityId"]), common.myInt(Request.QueryString["SummaryId"]), common.myStr(Request.QueryString["Type"]));

            // Details of top header and doctors name 
            StringBuilder sbtopheader = new StringBuilder();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ViewState["SignDoctorID"] = common.myStr(ds.Tables[0].Rows[0]["SignDoctorID"]);
                ViewState["SignJuniorDoctorID"] = common.myStr(ds.Tables[0].Rows[0]["SignJuniorDoctorID"]);
                ViewState["SignSecondaryDoctorID"] = common.myStr(ds.Tables[0].Rows[0]["SecondaryDoctorId"]);
                ViewState["SignThirdDoctorID"] = common.myStr(ds.Tables[0].Rows[0]["SignThirdDoctorID"]);
                ViewState["SignFourthDoctorID"] = common.myStr(ds.Tables[0].Rows[0]["SignFourthDoctorID"]);

                ViewState["PreparedById"] = common.myStr(ds.Tables[0].Rows[0]["PreparedById"]);
                ViewState["PreparedByName"] = common.myStr(ds.Tables[0].Rows[0]["PreparedByName"]);

                ViewState["FinalizeBy"] = common.myStr(ds.Tables[0].Rows[0]["FinalizeBy"]);
                ViewState["FinalizeByName"] = common.myStr(ds.Tables[0].Rows[0]["FinalizeByName"]);


                ViewState["SignDoctorName"] = common.myStr(ds.Tables[0].Rows[0]["SignDoctorName"]);
                ViewState["SignJuniorDoctorName"] = common.myStr(ds.Tables[0].Rows[0]["SignJuniorDoctorName"]);
                ViewState["SignSecondaryDoctorName"] = common.myStr(ds.Tables[0].Rows[0]["SecondaryDoctorName"]);
                ViewState["SignThirdDoctorName"] = common.myStr(ds.Tables[0].Rows[0]["SignThirdDoctorName"]);
                ViewState["SignFourthDoctorName"] = common.myStr(ds.Tables[0].Rows[0]["SignFourthDoctorName"]);

                ViewState["SignatureLine1"] = common.myStr(ds.Tables[0].Rows[0]["SignatureLine1"]);
                ViewState["SignatureLine2"] = common.myStr(ds.Tables[0].Rows[0]["SignatureLine2"]);
                ViewState["SignatureLine3"] = common.myStr(ds.Tables[0].Rows[0]["SignatureLine3"]);
                ViewState["SignatureLine4"] = common.myStr(ds.Tables[0].Rows[0]["SignatureLine4"]);

                ViewState["JuniorSignatureLine1"] = common.myStr(ds.Tables[0].Rows[0]["JuniorDoctorSignatureLine1"]);
                ViewState["JuniorSignatureLine2"] = common.myStr(ds.Tables[0].Rows[0]["JuniorDoctorSignatureLine2"]);
                ViewState["JuniorSignatureLine3"] = common.myStr(ds.Tables[0].Rows[0]["JuniorDoctorSignatureLine3"]);
                ViewState["JuniorSignatureLine4"] = common.myStr(ds.Tables[0].Rows[0]["JuniorDoctorSignatureLine4"]);

                ViewState["SecondarySignatureLine1"] = common.myStr(ds.Tables[0].Rows[0]["SecondaryDoctorSignatureLine1"]);
                ViewState["SecondarySignatureLine2"] = common.myStr(ds.Tables[0].Rows[0]["SecondaryDoctorSignatureLine2"]);
                ViewState["SecondarySignatureLine3"] = common.myStr(ds.Tables[0].Rows[0]["SecondaryDoctorSignatureLine3"]);
                ViewState["SecondarySignatureLine4"] = common.myStr(ds.Tables[0].Rows[0]["SecondaryDoctorSignatureLine4"]);

                ViewState["ThirdDoctorSignatureLine1"] = common.myStr(ds.Tables[0].Rows[0]["ThirdDoctorSignatureLine1"]);
                ViewState["ThirdDoctorSignatureLine2"] = common.myStr(ds.Tables[0].Rows[0]["ThirdDoctorSignatureLine2"]);
                ViewState["ThirdDoctorSignatureLine3"] = common.myStr(ds.Tables[0].Rows[0]["ThirdDoctorSignatureLine3"]);
                ViewState["ThirdDoctorSignatureLine4"] = common.myStr(ds.Tables[0].Rows[0]["ThirdDoctorSignatureLine4"]);

                ViewState["FourthDoctorSignatureLine1"] = common.myStr(ds.Tables[0].Rows[0]["FourthDoctorSignatureLine1"]);
                ViewState["FourthDoctorSignatureLine2"] = common.myStr(ds.Tables[0].Rows[0]["FourthDoctorSignatureLine2"]);
                ViewState["FourthDoctorSignatureLine3"] = common.myStr(ds.Tables[0].Rows[0]["FourthDoctorSignatureLine3"]);
                ViewState["FourthDoctorSignatureLine4"] = common.myStr(ds.Tables[0].Rows[0]["FourthDoctorSignatureLine4"]);

                ViewState["signdesg"] = common.myStr(ds.Tables[0].Rows[0]["signdesg"]);
                ViewState["DepartmentName"] = common.myStr(ds.Tables[0].Rows[0]["DepartmentName"]);
                ViewState["Education"] = common.myStr(ds.Tables[0].Rows[0]["Education"]);
                ViewState["SpecializationName"] = common.myStr(ds.Tables[0].Rows[0]["SpecializationName"]);
                //added by bhakti
                ViewState["WardNameBedNo"] = common.myStr(ds.Tables[0].Rows[0]["WardNameBedNo"]);
                ViewState["UPIN"] = common.myStr(ds.Tables[0].Rows[0]["UPIN"]).Trim();
                ViewState["Addendum"] = common.myStr(ds.Tables[0].Rows[0]["Addendum"]).Trim();

                ViewState["ShowPrintByInPageFooter"] = common.myBool(ds.Tables[0].Rows[0]["ShowPrintByInPageFooter"]);
                ViewState["ShowPrintDateInPageFooter"] = common.myBool(ds.Tables[0].Rows[0]["ShowPrintDateInPageFooter"]);

                ViewState["ShowLastUpdatedByNameInPageFooter"] = common.myBool(ds.Tables[0].Rows[0]["ShowLastUpdatedByNameInPageFooter"]);
                ViewState["ShowLastUpdatedDateInPageFooter"] = common.myBool(ds.Tables[0].Rows[0]["ShowLastUpdatedDateInPageFooter"]);
                ViewState["LastUpdatedByName"] = common.myStr(ds.Tables[0].Rows[0]["LastUpdatedByName"]);
                ViewState["LastUpdatedDate"] = common.myStr(ds.Tables[0].Rows[0]["LastUpdatedDate"]);

                //change Palendra

                ViewState["ShowPrintHeaderImage"] = common.myBool(ds.Tables[0].Rows[0]["PrintHeaderImage"]);
                ViewState["ShowPrintFooterImage"] = common.myBool(ds.Tables[0].Rows[0]["PrintFooterImage"]);
                ViewState["PrintHeaderImagePath"] = common.myStr(ds.Tables[0].Rows[0]["PrintHeaderImagePath"]);
                ViewState["PrintFooterImagePath"] = common.myStr(ds.Tables[0].Rows[0]["PrintFooterImagePath"]);
                ViewState["PrintVersionCode"] = common.myStr(ds.Tables[0].Rows[0]["PrintVersionCode"]);

                //change Palendra



                if (common.myStr(Session["FacilityName"]).ToUpper().Contains("VENKATESHWAR"))
                {
                    sbtopheader.Append(AddTopHeader(common.myInt(ds.Tables[0].Rows[0]["ConsultingDoctorDepartmentId"]),
                                                common.myStr(ds.Tables[0].Rows[0]["ConsultingDoctorDepartment"]),
                                                common.myStr(ds.Tables[0].Rows[0]["EntrySite"])));
                }
                else
                {
                    if (common.myStr(ViewState["IsShowAllDepartmentDoctorsInDeathSummary"]) == "Y")
                    {
                        sbtopheader.Append(AddTopHeaderNew(common.myInt(ds.Tables[0].Rows[0]["ConsultingDoctorDepartmentId"]),
                                            common.myStr(ds.Tables[0].Rows[0]["ConsultingDoctorDepartment"]),
                                            common.myStr(ds.Tables[0].Rows[0]["EntrySite"])));
                    }
                }

                try
                {
                    ViewState["ReportFooterText"] = common.myStr(ds.Tables[0].Rows[0]["ReportFooterText"]).Trim();
                }
                catch
                {
                }
            }
            BaseC.Hospital objHospital = new BaseC.Hospital(sConString);
            DataSet dsHeader = new DataSet();
            if (common.myStr(Request.QueryString["For"]) == "DthSum")
            {
                if (common.myInt(Request.QueryString["ReportId"]) > 0)
                {
                    dsHeader = objHospital.GetReportHeader("DE", common.myInt(Request.QueryString["ReportId"]));//Death summary header
                }
                else
                {
                    dsHeader = objHospital.GetReportHeader("DE");
                }
            }
            else
            {
                if (common.myInt(Request.QueryString["ReportId"]) > 0)
                {
                    dsHeader = objHospital.GetReportHeader("DS", common.myInt(Request.QueryString["ReportId"]));//Discharge summary header
                }
                else
                {
                    dsHeader = objHospital.GetReportHeader("DS");//Discharge summary header
                }
            }
            DataTable dtHeaderMerge = dsHeader.Tables[0];

            string headerRow = "<hr width='100%' size='1px' />";

            #region Title header
            StringBuilder sbTitle = new StringBuilder();

            if (ds.Tables[0].Rows.Count > 0)
            {
                if (common.myStr(Session["FacilityName"]).ToUpper().Contains("VENKATESHWAR"))
                {
                    if (Request.QueryString["Finalize"] != null)
                    {
                        if (!common.myStr(Request.QueryString["Finalize"]).Equals(string.Empty))
                        {
                            if (common.myStr(Request.QueryString["Finalize"]).Equals("F"))
                            {
                                sbTitle.Append("<table cellpadding='0' cellspacing='0'><tr><td valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + "; font-size:20pt;'>" + common.myStr(ds.Tables[0].Rows[0]["HeadingName"]) + " ( Draft ) " + "</td></tr></table>");
                            }
                            else
                            {
                                sbTitle.Append("<table cellpadding='0' cellspacing='0'><tr><td valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + "; font-size:20pt;'>" + common.myStr(ds.Tables[0].Rows[0]["HeadingName"]) + "</td></tr></table>");
                            }
                        }
                        else
                        {
                            sbTitle.Append("<table cellpadding='0' cellspacing='0'><tr><td valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + "; font-size:20pt;'>" + common.myStr(ds.Tables[0].Rows[0]["HeadingName"]) + "</td></tr></table>");
                        }
                    }
                    else
                    {
                        sbTitle.Append("<table cellpadding='0' cellspacing='0'><tr><td valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + "; font-size:20pt;'>" + common.myStr(ds.Tables[0].Rows[0]["HeadingName"]) + "</td></tr></table>");
                    }
                }
                else if (common.myStr(Session["FacilityName"]).ToUpper().Contains("HEALTHWORLD"))
                {
                    if (Request.QueryString["Finalize"] != null)
                    {
                        if (!common.myStr(Request.QueryString["Finalize"]).Equals(string.Empty))
                        {
                            if (common.myStr(Request.QueryString["Finalize"]).Equals("F"))
                            {
                                sbTitle.Append("<table cellpadding='0' cellspacing='0'><tr><td valign='top' align='center' style='font-family:Calibri; font-size:14pt;font-weight:bold;'>" + common.myStr(ds.Tables[0].Rows[0]["HeadingName"]).ToUpper() + " ( DRAFT ) " + "</td></tr></table>");
                            }
                            else
                            {
                                sbTitle.Append("<table cellpadding='0' cellspacing='0'><tr><td valign='top' align='center' style='font-family:Calibri; font-size:14pt;font-weight:bold;'>" + common.myStr(ds.Tables[0].Rows[0]["HeadingName"]).ToUpper() + "</td></tr></table>");
                            }
                        }
                        else
                        {
                            sbTitle.Append("<table cellpadding='0' cellspacing='0'><tr><td valign='top' align='center' style='font-family:Calibri; font-size:14pt;font-weight:bold;'>" + common.myStr(ds.Tables[0].Rows[0]["HeadingName"]).ToUpper() + "</td></tr></table>");
                        }
                    }
                    else
                    {
                        sbTitle.Append("<table cellpadding='0' cellspacing='0'><tr><td valign='top' align='center' style='font-family:Calibri; font-size:14pt;font-weight:bold;'>" + common.myStr(ds.Tables[0].Rows[0]["HeadingName"]).ToUpper() + "</td></tr></table>");
                    }
                }
                else if (common.myStr(Session["FacilityName"]).ToUpper().Contains("PUSHPAWATI"))   //Added on 15042022 Manoj Puri
                {
                    if (Request.QueryString["Finalize"] != null)
                    {
                        if (!common.myStr(Request.QueryString["Finalize"]).Equals(string.Empty))
                        {
                            if (common.myStr(Request.QueryString["Finalize"]).Equals("F"))
                            {
                                sbTitle.Append("<table cellpadding='0' cellspacing='0'><tr><td valign='top' align='center' style='font-family: " + common.myStr(hdnFontName.Value) + "; font-size:12pt;font-weight:bold;'>" + common.myStr(ds.Tables[0].Rows[0]["HeadingName"]).ToUpper() + "</td></tr></table>");
                            }
                            else
                            {
                                sbTitle.Append("<table cellpadding='0' cellspacing='0'><tr><td valign='top' align='center' style='font-family: " + common.myStr(hdnFontName.Value) + "; font-size:12pt;font-weight:bold;'>" + common.myStr(ds.Tables[0].Rows[0]["HeadingName"]).ToUpper() + "</td></tr></table>");
                            }
                        }
                        else
                        {
                            sbTitle.Append("<table cellpadding='0' cellspacing='0'><tr><td valign='top' align='center' style='font-family: " + common.myStr(hdnFontName.Value) + "; font-size:12pt;font-weight:bold;'>" + common.myStr(ds.Tables[0].Rows[0]["HeadingName"]).ToUpper() + "</td></tr></table>");
                        }
                    }
                    else
                    {
                        sbTitle.Append("<table cellpadding='0' cellspacing='0'><tr><td valign='top' align='center' style='font-family: " + common.myStr(hdnFontName.Value) + "; font-size:12pt;font-weight:bold;'>" + common.myStr(ds.Tables[0].Rows[0]["HeadingName"]).ToUpper() + "</td></tr></table>");
                    }
                }
                else
                {
                    if (Request.QueryString["Finalize"] != null)
                    {
                        if (!common.myStr(Request.QueryString["Finalize"]).Equals(string.Empty))
                        {
                            if (common.myStr(Request.QueryString["Finalize"]).Equals("F"))
                            {
                                sbTitle.Append("<table cellpadding='0' cellspacing='0'><tr><td valign='top' align='center' style='font-family: " + common.myStr(hdnFontName.Value) + "; font-size:12pt;font-weight:bold;'>" + common.myStr(ds.Tables[0].Rows[0]["HeadingName"]).ToUpper() + " ( DRAFT ) " + "</td></tr></table>");
                            }
                            else
                            {
                                sbTitle.Append("<table cellpadding='0' cellspacing='0'><tr><td valign='top' align='center' style='font-family: " + common.myStr(hdnFontName.Value) + "; font-size:12pt;font-weight:bold;'>" + common.myStr(ds.Tables[0].Rows[0]["HeadingName"]).ToUpper() + "</td></tr></table>");
                            }
                        }
                        else
                        {
                            sbTitle.Append("<table cellpadding='0' cellspacing='0'><tr><td valign='top' align='center' style='font-family: " + common.myStr(hdnFontName.Value) + "; font-size:12pt;font-weight:bold;'>" + common.myStr(ds.Tables[0].Rows[0]["HeadingName"]).ToUpper() + "</td></tr></table>");
                        }
                    }
                    else
                    {
                        sbTitle.Append("<table cellpadding='0' cellspacing='0'><tr><td valign='top' align='center' style='font-family: " + common.myStr(hdnFontName.Value) + "; font-size:12pt;font-weight:bold;'>" + common.myStr(ds.Tables[0].Rows[0]["HeadingName"]).ToUpper() + "</td></tr></table>");
                    }
                }
            }
            else
            {
                if (common.myStr(Request.QueryString["For"]) == "DthSum")
                {
                    sbTitle.Append("<table cellpadding='0' cellspacing='0'><tr><td valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:20pt;'>Death Summary</td></tr></table>");
                }
                else
                {
                    if (Request.QueryString["Finalize"] != null)
                    {
                        if (!common.myStr(Request.QueryString["Finalize"]).Equals(string.Empty))
                        {
                            if (common.myStr(Request.QueryString["Finalize"]).Equals("F"))
                            {
                                sbTitle.Append("<table cellpadding='0' cellspacing='0'><tr><td valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:20pt;'>Discharge Summary ( Draft )</td></tr></table>");
                            }
                            else
                            {
                                sbTitle.Append("<table cellpadding='0' cellspacing='0'><tr><td valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:20pt;'>Discharge Summary</td></tr></table>");
                            }
                        }
                        else
                        {
                            sbTitle.Append("<table cellpadding='0' cellspacing='0'><tr><td valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:20pt;'>Discharge Summary</td></tr></table>");
                        }
                    }
                    else
                    {
                        sbTitle.Append("<table cellpadding='0' cellspacing='0'><tr><td valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:20pt;'>Discharge Summary</td></tr></table>");
                    }

                    //sbTitle.Append("<table cellpadding='0' cellspacing='0'><tr><td valign='top' align='left' style='font-family: "+ common.myStr(hdnFontName.Value) +";font-size:20pt;'>Discharge Summary</td></tr></table>");
                }
            }

            #endregion

            #region Header Section of Report

            if (dsHeader.Tables.Count > 0)
            {
                int RowNo = 0;
                string colspan = "";

                if (dsHeader.Tables[0].Rows.Count > 0)
                {
                    if (common.myStr(Request.QueryString["For"]) == "DthSum" || common.myStr(Request.QueryString["For"]) == "DISSUM")
                    {
                        #region VENKATESHWAR
                        if (common.myStr(Session["FacilityName"]).ToUpper().Contains("VENKATESHWAR"))
                        {
                            //select d.RowNo, d.ColNo, d.FieldCaption, oo.ObjectValue
                            //from EMRTemplateDataObjects oo
                            //inner join EMRFormHeaderDetails d on oo.Id=d.ObjectId
                            //where oo.Active=1 and d.HeaderId=19 and d.Active=1
                            //Order by d.RowNo, d.ColNo

                            //1	1	Patient     	PN
                            //1	2	Age / Gender	PAG
                            //1	3	UHID	        RN
                            //1	4	IP No.	        EN                        
                            //2	1	Address	        PADD
                            //3	1	Consultant  	CDN
                            //3	2	DOA	            DOA
                            //3	3	DOD	            DOD

                            sb.Append("<table border='0' width='100%' cellpadding='0' cellspacing='0'>");

                            sb.Append("<tr>");
                            sb.Append("<td>&nbsp;</td>");
                            sb.Append("<td>&nbsp;</td>");
                            sb.Append("<td>&nbsp;</td>");
                            sb.Append("<td>&nbsp;</td>");
                            sb.Append("<td>&nbsp;</td>");
                            sb.Append("<td>&nbsp;</td>");
                            sb.Append("<td>&nbsp;</td>");
                            sb.Append("<td>&nbsp;</td>");
                            sb.Append("<td>&nbsp;</td>");
                            sb.Append("<td>&nbsp;</td>");
                            sb.Append("<td>&nbsp;</td>");
                            sb.Append("<td>&nbsp;</td>");
                            sb.Append("<td>&nbsp;</td>");
                            sb.Append("<td>&nbsp;</td>");
                            sb.Append("<td>&nbsp;</td>");
                            sb.Append("<td>&nbsp;</td>");
                            sb.Append("<td>&nbsp;</td>");
                            sb.Append("<td>&nbsp;</td>");
                            sb.Append("<td>&nbsp;</td>");
                            sb.Append("<td>&nbsp;</td>");
                            sb.Append("</tr>");

                            sb.Append("<tr><td colspan='20' >" + headerRow + "</td></tr>");

                            //row1
                            sb.Append("<tr align='left'>");

                            dsHeader.Tables[0].DefaultView.RowFilter = "";
                            dsHeader.Tables[0].DefaultView.RowFilter = "ObjectValue='PN'";

                            if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
                            {
                                sb.Append("<td colspan='2' valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;font-weight:bold;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</td>");
                                sb.Append("<td colspan='5' valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:12pt;font-weight:bold;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td>");
                            }
                            else
                            {
                                sb.Append("<td colspan='2' valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;'>&nbsp;</td>");
                                sb.Append("<td colspan='5' valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;'>&nbsp;</td>");
                            }

                            dsHeader.Tables[0].DefaultView.RowFilter = "";
                            dsHeader.Tables[0].DefaultView.RowFilter = "ObjectValue='PAG'";

                            if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
                            {
                                sb.Append("<td colspan='6' valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;'><SPAN style='font-weight:bold;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</SPAN> : " +
                                                            Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td>");
                                //sb.Append("<td colspan='2' valign='top' align='left' style='font-family: "+ common.myStr(hdnFontName.Value) +";font-size:9pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td>");
                            }
                            else
                            {
                                sb.Append("<td colspan='2' valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;'>&nbsp;</td>");
                                sb.Append("<td colspan='2' valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;'>&nbsp;</td>");
                            }

                            dsHeader.Tables[0].DefaultView.RowFilter = "";
                            dsHeader.Tables[0].DefaultView.RowFilter = "ObjectValue='RN'";

                            if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
                            {
                                sb.Append("<td valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;font-weight:bold;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</td>");
                                sb.Append("<td colspan='3' valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td>");
                            }
                            else
                            {
                                sb.Append("<td valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;'>&nbsp;</td>");
                                sb.Append("<td colspan='3' valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;'>&nbsp;</td>");
                            }

                            dsHeader.Tables[0].DefaultView.RowFilter = "";
                            dsHeader.Tables[0].DefaultView.RowFilter = "ObjectValue='EN'";

                            if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
                            {
                                sb.Append("<td valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;font-weight:bold;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</td>");
                                sb.Append("<td colspan='2' valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td>");
                            }
                            else
                            {
                                sb.Append("<td valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;'>&nbsp;</td>");
                                sb.Append("<td colspan='2' valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;'>&nbsp;</td>");
                            }

                            sb.Append("</tr>");

                            sb.Append("<tr><td colspan='20'>" + headerRow + "</td></tr>");

                            //Next Header
                            sbNextHeader = new StringBuilder();
                            sbNextHeader.Append(sb.ToString());
                            sbNextHeader.Append("</table>");

                            //row2
                            sb.Append("<tr align='left'>");

                            dsHeader.Tables[0].DefaultView.RowFilter = "";
                            dsHeader.Tables[0].DefaultView.RowFilter = "ObjectValue='PADD'";

                            if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
                            {
                                sb.Append("<td colspan='2'  valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;font-weight:bold;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</td>");
                                sb.Append("<td colspan='18' valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td>");
                            }
                            else
                            {
                                sb.Append("<td colspan='2'  valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;'>&nbsp;</td>");
                                sb.Append("<td colspan='18' valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;'>&nbsp;</td>");
                            }

                            sb.Append("</tr>");

                            sb.Append("<tr><td colspan='20'>" + headerRow + "</td></tr>");


                            //row3
                            sb.Append("<tr align='left'>");

                            dsHeader.Tables[0].DefaultView.RowFilter = "";
                            dsHeader.Tables[0].DefaultView.RowFilter = "ObjectValue='CDN'";

                            if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
                            {
                                sb.Append("<td colspan='2' valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;font-weight:bold;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</td>"); //"Consultant" 
                                sb.Append("<td colspan='6' valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;font-weight:bold;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td>");
                            }
                            else
                            {
                                sb.Append("<td colspan='2' valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;'>&nbsp;</td>");
                                sb.Append("<td colspan='6' valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:1pt;'>&nbsp;</td>");
                            }

                            dsHeader.Tables[0].DefaultView.RowFilter = "";
                            dsHeader.Tables[0].DefaultView.RowFilter = "ObjectValue='DOA'";

                            if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
                            {
                                sb.Append("<td valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;font-weight:bold;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</td>");
                                sb.Append("<td colspan='5' valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td>");
                            }
                            else
                            {
                                sb.Append("<td valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;'>&nbsp;</td>");
                                sb.Append("<td colspan='5' valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;'>&nbsp;</td>");
                            }

                            dsHeader.Tables[0].DefaultView.RowFilter = "";
                            dsHeader.Tables[0].DefaultView.RowFilter = "ObjectValue='DOD'";

                            if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
                            {
                                sb.Append("<td valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;font-weight:bold;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</td>");
                                sb.Append("<td colspan='5' valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td>");
                            }
                            else
                            {
                                sb.Append("<td valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;'>&nbsp;</td>");
                                sb.Append("<td colspan='5' valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;'>&nbsp;</td>");
                            }

                            sb.Append("</tr>");

                            sb.Append("<tr><td colspan='20'>" + headerRow + "</td></tr>");

                            sb.Append("</table>");
                        }
                        #endregion
                        #region BLK
                        else if (common.myStr(Session["FacilityName"]).ToUpper().Contains("BLK"))
                        {
                            sb.Append("<table border='0' cellpadding='0' cellspacing='0'>");

                            int ColumnCount = common.myInt(dsHeader.Tables[0].Compute("MAX(ColNo)", string.Empty));

                            sb.Append("<tr><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr>");

                            //sb.Append("<tr><td colspan='" + (ColumnCount * 2) + "' >" + headerRow + "</td></tr>");
                            sb.Append("<tr><td colspan='7' >" + headerRow + "</td></tr>");

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
                                            sb.Append("<td width=10% align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</td>");
                                            sb.Append("<td width=20% align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td></tr>");
                                        }
                                        else
                                        {
                                            sb.Append("<td width=10% align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>&nbsp;</td>");
                                            sb.Append("<td width=20% align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>&nbsp;</td>");
                                        }
                                    }

                                    if (ColumnCount == 2)
                                    {
                                        //i = +1;
                                        //sb.Append("<td align='left'>" + common.myStr(dsHeader.Tables[0].Rows[i]["FieldCaption"]) + "</td>");
                                        //sb.Append("<td align='left'>dsadas</td>");
                                        //sb.Append("<td align='left'>" + common.myStr(dsHeader.Tables[0].Rows[i]["FieldCaption"]) + "</td>");
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
                                            sb.Append("<td                 width=10% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</td>");
                                            sb.Append("<td colspan='3' width=20% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td>");
                                        }
                                        else
                                        {
                                            sb.Append("<td width=10% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>&nbsp;</td>");
                                            sb.Append("<td colspan='3' width=20% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>&nbsp;</td>");
                                        }
                                        dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=2 AND Rowno=" + RowNo;
                                        if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
                                        {
                                            sb.Append("<td align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</td>");
                                            sb.Append("<td colspan='2' align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td></tr>");
                                        }
                                        else
                                        {
                                            sb.Append("<td align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>&nbsp;</td>");
                                            sb.Append("<td colspan='2' align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>&nbsp;</td>");
                                        }
                                    }

                                    if (ColumnCount == 3)
                                    {
                                        i += 2;
                                        dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=1 AND Rowno=" + RowNo;
                                        if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
                                        {
                                            sb.Append("<td width=10% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</td>");
                                            dtHeaderMerge.DefaultView.RowFilter = "ColNo=2 AND Rowno=" + RowNo;
                                            if (dtHeaderMerge.DefaultView.ToTable().Rows.Count > 0)// this for merge td
                                            {
                                                dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=1 AND Rowno=" + RowNo;
                                                sb.Append("<td width=20% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td>");
                                            }
                                            else
                                            {
                                                dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=1 AND Rowno=" + RowNo;
                                                sb.Append("<td colspan='3' valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td>");
                                            }
                                        }
                                        else
                                        {
                                            sb.Append("<td width=10% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>&nbsp;</td>");
                                            sb.Append("<td width=20% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>&nbsp;</td>");
                                        }
                                        dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=2 AND Rowno=" + RowNo;
                                        if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
                                        {
                                            sb.Append("<td align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</td>");
                                            sb.Append("<td align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td>");
                                        }
                                        //else
                                        //{
                                        //    sb.Append("<td align='left' valign='top' style='font-family: "+ common.myStr(hdnFontName.Value) +";font-size:10pt;'></td>");
                                        //    sb.Append("<td align='left' valign='top' style='font-family: "+ common.myStr(hdnFontName.Value) +";font-size:10pt;'></td>");
                                        //}
                                        dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=3 AND Rowno=" + RowNo;
                                        if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
                                        {
                                            sb.Append("<td align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</td>");
                                            sb.Append("<td align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td></tr>");
                                        }
                                        else
                                        {
                                            sb.Append("<td align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>&nbsp;</td>");
                                            sb.Append("<td align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>&nbsp;</td>");
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
                                            sb.Append("<td width=10% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</td>");
                                            sb.Append("<td width=20% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td></tr>");
                                        }
                                        else
                                        {
                                            sb.Append("<td width=10% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>&nbsp;</td>");
                                            sb.Append("<td width=20% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>&nbsp;</td>");
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

                                        if (common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]).Equals("PADD"))
                                        {
                                            if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
                                            {
                                                sb.Append("<td                 width=10% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</td>");
                                                sb.Append("<td colspan='6' width=20% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td></tr>");
                                            }
                                            else
                                            {
                                                sb.Append("<td width=10% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>&nbsp;</td>");
                                                sb.Append("<td colspan='6' width=20% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>&nbsp;</td>");
                                            }
                                        }
                                        else
                                        {
                                            if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
                                            {
                                                sb.Append("<td                 width=10% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</td>");
                                                sb.Append("<td colspan='3' width=20% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td>");
                                            }
                                            else
                                            {
                                                sb.Append("<td width=10% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>&nbsp;</td>");
                                                sb.Append("<td colspan='3' width=20% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>&nbsp;</td>");
                                            }
                                            dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=2 AND Rowno=" + RowNo;
                                            if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
                                            {
                                                sb.Append("<td align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</td>");
                                                sb.Append("<td colspan='2' align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td></tr>");
                                            }
                                            else
                                            {
                                                sb.Append("<td align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>&nbsp;</td>");
                                                sb.Append("<td colspan='2' align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>&nbsp;</td>");
                                            }
                                        }
                                    }
                                    if (ColumnCount == 3)
                                    {
                                        i += 2;

                                        dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=1 AND Rowno=" + RowNo;
                                        if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
                                        {
                                            sb.Append("<td width=10% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</td>");

                                            dtHeaderMerge.DefaultView.RowFilter = "ColNo=2 AND Rowno=" + RowNo;
                                            if (dtHeaderMerge.DefaultView.ToTable().Rows.Count > 0)// this for merge td
                                            {
                                                dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=1 AND Rowno=" + RowNo;
                                                sb.Append("<td width=20% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td>");
                                            }
                                            else
                                            {
                                                dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=1 AND Rowno=" + RowNo;
                                                sb.Append("<td colspan='3' valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td>");
                                                // sb.Append("<td  colspan='3' align='left' style='font-family: "+ common.myStr(hdnFontName.Value) +";font-size:10pt;'></td>");
                                            }
                                        }
                                        else
                                        {
                                            sb.Append("<td width=10% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>&nbsp;</td>");
                                            sb.Append("<td width=20% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>&nbsp;</td>");
                                        }
                                        dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=2 AND Rowno=" + RowNo;
                                        if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
                                        {
                                            sb.Append("<td align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</td>");
                                            sb.Append("<td align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td>");

                                        }
                                        //else
                                        //{
                                        //    sb.Append("<td align='left' valign='top' style='font-family: "+ common.myStr(hdnFontName.Value) +";font-size:10pt;'></td>");
                                        //    sb.Append("<td align='left' valign='top' style='font-family: "+ common.myStr(hdnFontName.Value) +";font-size:10pt;'></td>");
                                        //}
                                        dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=3 AND Rowno=" + RowNo;
                                        if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
                                        {
                                            sb.Append("<td align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</td>");
                                            sb.Append("<td align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td></tr>");
                                        }
                                        else
                                        {
                                            sb.Append("<td align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>&nbsp;</td>");
                                            sb.Append("<td align='left' valign='top' style='font-family:" + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>&nbsp;</td>");
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
                                            sb.Append("<td width=10% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</td>");
                                            sb.Append("<td width=20% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td></tr>");
                                        }
                                        else
                                        {
                                            sb.Append("<td width=10% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>&nbsp;</td>");
                                            sb.Append("<td width=20% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>&nbsp;</td>");
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
                                            sb.Append("<td                 width=10% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</td>");
                                            sb.Append("<td " + colspan + " width=20% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td>");
                                        }
                                        else
                                        {
                                            sb.Append("<td width=10% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>&nbsp;</td>");
                                            sb.Append("<td width=20% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>&nbsp;</td>");
                                        }
                                        dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=2 AND Rowno=" + RowNo;
                                        if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
                                        {
                                            sb.Append("<td align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</td>");
                                            sb.Append("<td align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td></tr>");
                                        }
                                        //else
                                        //{
                                        //    sb.Append("<td align='left' valign='top' style='font-family: "+ common.myStr(hdnFontName.Value) +";font-size:10pt;'>&nbsp;</td>");
                                        //    sb.Append("<td align='left' valign='top' style='font-family: "+ common.myStr(hdnFontName.Value) +";font-size:10pt;'>&nbsp;</td>");
                                        //}
                                    }
                                    if (ColumnCount == 3)
                                    {
                                        i += 2;
                                        dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=1 AND Rowno=" + RowNo;
                                        if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
                                        {
                                            sb.Append("<td width=10% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</td>");
                                            dtHeaderMerge.DefaultView.RowFilter = "ColNo=2 AND Rowno=" + RowNo;
                                            if (dtHeaderMerge.DefaultView.ToTable().Rows.Count > 0)// this for merge td
                                            {
                                                dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=1 AND Rowno=" + RowNo;
                                                sb.Append("<td width=20% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td>");
                                            }
                                            else
                                            {
                                                dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=1 AND Rowno=" + RowNo;
                                                sb.Append("<td colspan='3' valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td>");
                                            }
                                        }
                                        else
                                        {
                                            sb.Append("<td width=10% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>&nbsp;</td>");
                                            sb.Append("<td width=20% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>&nbsp;</td>");
                                        }
                                        dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=2 AND Rowno=" + RowNo;

                                        if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
                                        {
                                            sb.Append("<td align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</td>");
                                            sb.Append("<td align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td>");
                                        }
                                        //else
                                        //{
                                        //    sb.Append("<td align='left' style='font-family: "+ common.myStr(hdnFontName.Value) +";font-size:10pt;'></td>");
                                        //    sb.Append("<td align='left' style='font-family: "+ common.myStr(hdnFontName.Value) +";font-size:10pt;'></td>");
                                        //}
                                        dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=3 AND Rowno=" + RowNo;
                                        if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
                                        {
                                            sb.Append("<td align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</td>");
                                            sb.Append("<td align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td></tr>");
                                        }
                                        else
                                        {
                                            sb.Append("<td align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>&nbsp;</td>");
                                            sb.Append("<td align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>&nbsp;</td>");
                                        }
                                    }

                                }
                                //if (i + 1 != common.myInt(dsHeader.Tables[0].Rows.Count))
                                if ((i + 1) < common.myInt(dsHeader.Tables[0].Rows.Count))
                                {
                                    RowNo = common.myInt(dsHeader.Tables[0].Rows[i + 1]["Rowno"]);

                                    //sb.Append("<tr><td colspan='6'>" + headerRow + "</td></tr>");

                                }

                                if ((RowNo - 1) < 3)
                                {
                                    sbNextHeader = new StringBuilder();
                                    sbNextHeader.Append(sb.ToString());
                                    sbNextHeader.Append("<tr><td colspan='7'>" + headerRow + "</td></tr>");

                                    sbNextHeader.Append("</table>");
                                }
                            }
                            sb.Append("</table>");

                            sb.Append("<table cellpadding='0' cellspacing='0'><tr><td>" + headerRow + "</td></tr></table>");
                        }
                        #endregion
                        #region OtherClients
                        else
                        {
                            sb.Append("<table border='0' cellpadding='0' cellspacing='0'>");

                            int ColumnCount = common.myInt(dsHeader.Tables[0].Compute("MAX(ColNo)", string.Empty));

                            sb.Append("<tr><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr>");

                            //sb.Append("<tr><td colspan='" + (ColumnCount * 2) + "' >" + headerRow + "</td></tr>");
                            sb.Append("<tr><td colspan='7'>" + headerRow + "</td></tr>");

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
                                            sb.Append("<td width=10% align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;font-weight:bold;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</td>");
                                            sb.Append("<td width=20% align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td></tr>");
                                        }
                                        else
                                        {
                                            sb.Append("<td width=10% align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>&nbsp;</td>");
                                            sb.Append("<td width=20% align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>&nbsp;</td>");
                                        }
                                    }

                                    if (ColumnCount == 2)
                                    {
                                        //i = +1;
                                        //sb.Append("<td align='left'>" + common.myStr(dsHeader.Tables[0].Rows[i]["FieldCaption"]) + "</td>");
                                        //sb.Append("<td align='left'>dsadas</td>");
                                        //sb.Append("<td align='left'>" + common.myStr(dsHeader.Tables[0].Rows[i]["FieldCaption"]) + "</td>");
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
                                            sb.Append("<td                 width=10% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;font-weight:bold;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</td>");
                                            sb.Append("<td colspan='3' width=20% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td>");
                                        }
                                        else
                                        {
                                            sb.Append("<td width=10% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>&nbsp;</td>");
                                            sb.Append("<td colspan='3' width=20% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>&nbsp;</td>");
                                        }
                                        dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=2 AND Rowno=" + RowNo;
                                        if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
                                        {
                                            sb.Append("<td align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;font-weight:bold;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</td>");
                                            sb.Append("<td colspan='2' align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td></tr>");
                                        }
                                        else
                                        {
                                            sb.Append("<td align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>&nbsp;</td>");
                                            sb.Append("<td colspan='2' align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>&nbsp;</td>");
                                        }
                                    }

                                    if (ColumnCount == 3)
                                    {
                                        i += 2;
                                        dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=1 AND Rowno=" + RowNo;
                                        if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
                                        {
                                            sb.Append("<td width=10% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;font-weight:bold;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</td>");
                                            dtHeaderMerge.DefaultView.RowFilter = "ColNo=2 AND Rowno=" + RowNo;
                                            if (dtHeaderMerge.DefaultView.ToTable().Rows.Count > 0)// this for merge td
                                            {
                                                dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=1 AND Rowno=" + RowNo;
                                                sb.Append("<td width=20% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td>");
                                            }
                                            else
                                            {
                                                dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=1 AND Rowno=" + RowNo;
                                                sb.Append("<td colspan='3' valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td>");
                                            }
                                        }
                                        else
                                        {
                                            sb.Append("<td width=10% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>&nbsp;</td>");
                                            sb.Append("<td width=20% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>&nbsp;</td>");
                                        }
                                        dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=2 AND Rowno=" + RowNo;
                                        if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
                                        {
                                            sb.Append("<td align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;font-weight:bold;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</td>");
                                            sb.Append("<td align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td>");
                                        }
                                        //else
                                        //{
                                        //    sb.Append("<td align='left' valign='top' style='font-family: "+ common.myStr(hdnFontName.Value) +";font-size:10pt;'></td>");
                                        //    sb.Append("<td align='left' valign='top' style='font-family: "+ common.myStr(hdnFontName.Value) +";font-size:10pt;'></td>");
                                        //}
                                        dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=3 AND Rowno=" + RowNo;
                                        if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
                                        {
                                            sb.Append("<td align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;font-weight:bold;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</td>");
                                            sb.Append("<td align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td></tr>");
                                        }
                                        else
                                        {
                                            sb.Append("<td align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>&nbsp;</td>");
                                            sb.Append("<td align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>&nbsp;</td>");
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
                                            sb.Append("<td width=10% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;font-weight:bold;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</td>");
                                            sb.Append("<td width=20% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td></tr>");
                                        }
                                        else
                                        {
                                            sb.Append("<td width=10% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>&nbsp;</td>");
                                            sb.Append("<td width=20% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>&nbsp;</td>");
                                        }
                                    }
                                    if (ColumnCount == 2)
                                    {
                                        dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=2 AND Rowno=" + RowNo;
                                        if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
                                        {
                                            colspan = "";
                                            dsHeader.Tables[0].DefaultView.RowFilter = "";

                                            dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=1 AND Rowno=" + RowNo;
                                            if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
                                            {
                                                i += 1;
                                            }
                                        }
                                        else
                                        {
                                            colspan = " colspan='3'";
                                        }

                                        bool IsAddressPrint = false;
                                        dsHeader.Tables[0].DefaultView.RowFilter = "";

                                        dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=1 AND Rowno=" + RowNo;

                                        if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
                                        {
                                            if (common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]).Equals("PADD"))
                                            {
                                                if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
                                                {
                                                    IsAddressPrint = true;

                                                    sb.Append("<td                 width=10% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;font-weight:bold;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</td>");
                                                    sb.Append("<td colspan='6' width=20% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td></tr>");
                                                }
                                                else
                                                {
                                                    sb.Append("<td width=10% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>&nbsp;</td>");
                                                    sb.Append("<td colspan='6' width=20% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>&nbsp;</td>");
                                                }
                                            }
                                            else
                                            {
                                                if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
                                                {
                                                    sb.Append("<td                 width=10% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;font-weight:bold;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</td>");
                                                    sb.Append("<td colspan='3' width=20% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td>");
                                                }
                                                else
                                                {
                                                    sb.Append("<td width=10% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>&nbsp;</td>");
                                                    sb.Append("<td colspan='3' width=20% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>&nbsp;</td>");
                                                }
                                            }
                                        }
                                        else
                                        {
                                            sb.Append("<td align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>&nbsp;</td>");
                                            sb.Append("<td colspan='3' align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>&nbsp;</td>");
                                        }

                                        dsHeader.Tables[0].DefaultView.RowFilter = "";

                                        dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=2 AND Rowno=" + RowNo;

                                        if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
                                        {
                                            if (common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]).Equals("PADD"))
                                            {
                                                sb.Append("<td                 width=10% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;font-weight:bold;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</td>");
                                                sb.Append("<td colspan='6' width=20% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td></tr>");
                                            }
                                            else
                                            {
                                                sb.Append("<td align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;font-weight:bold;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</td>");
                                                sb.Append("<td colspan='2' align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td></tr>");
                                            }
                                        }
                                        else
                                        {
                                            if (!IsAddressPrint)
                                            {
                                                sb.Append("<td align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>&nbsp;</td>");
                                                sb.Append("<td colspan='2' align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>&nbsp;</td>");
                                            }
                                        }
                                    }
                                    if (ColumnCount == 3)
                                    {
                                        i += 2;

                                        dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=1 AND Rowno=" + RowNo;
                                        if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
                                        {
                                            sb.Append("<td width=10% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;font-weight:bold;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</td>");

                                            dtHeaderMerge.DefaultView.RowFilter = "ColNo=2 AND Rowno=" + RowNo;
                                            if (dtHeaderMerge.DefaultView.ToTable().Rows.Count > 0)// this for merge td
                                            {
                                                dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=1 AND Rowno=" + RowNo;
                                                sb.Append("<td width=20% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td>");
                                            }
                                            else
                                            {
                                                dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=1 AND Rowno=" + RowNo;
                                                sb.Append("<td colspan='3' valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td>");
                                                // sb.Append("<td  colspan='3' align='left' style='font-family: "+ common.myStr(hdnFontName.Value) +";font-size:10pt;'></td>");
                                            }
                                        }
                                        else
                                        {
                                            sb.Append("<td width=10% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>&nbsp;</td>");
                                            sb.Append("<td width=20% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>&nbsp;</td>");
                                        }
                                        dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=2 AND Rowno=" + RowNo;
                                        if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
                                        {
                                            sb.Append("<td align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;font-weight:bold;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</td>");
                                            sb.Append("<td align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td>");

                                        }
                                        //else
                                        //{
                                        //    sb.Append("<td align='left' valign='top' style='font-family: "+ common.myStr(hdnFontName.Value) +";font-size:10pt;'></td>");
                                        //    sb.Append("<td align='left' valign='top' style='font-family: "+ common.myStr(hdnFontName.Value) +";font-size:10pt;'></td>");
                                        //}
                                        dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=3 AND Rowno=" + RowNo;
                                        if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
                                        {
                                            sb.Append("<td align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;font-weight:bold;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</td>");
                                            sb.Append("<td align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td></tr>");
                                        }
                                        else
                                        {
                                            sb.Append("<td align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>&nbsp;</td>");
                                            sb.Append("<td align='left' valign='top' style='font-family:" + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>&nbsp;</td>");
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
                                            sb.Append("<td width=10% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;font-weight:bold;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</td>");
                                            sb.Append("<td width=20% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td></tr>");
                                        }
                                        else
                                        {
                                            sb.Append("<td width=10% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>&nbsp;</td>");
                                            sb.Append("<td width=20% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>&nbsp;</td>");
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
                                            sb.Append("<td width=10% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;font-weight:bold;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</td>");
                                            sb.Append("<td " + colspan + " width=20% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td>");
                                        }
                                        else
                                        {
                                            sb.Append("<td width=10% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>&nbsp;</td>");
                                            sb.Append("<td width=20% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>&nbsp;</td>");
                                        }
                                        dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=2 AND Rowno=" + RowNo;
                                        if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
                                        {
                                            sb.Append("<td align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;font-weight:bold;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</td>");
                                            sb.Append("<td align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td></tr>");
                                        }
                                        //else
                                        //{
                                        //    sb.Append("<td align='left' valign='top' style='font-family: "+ common.myStr(hdnFontName.Value) +";font-size:10pt;'>&nbsp;</td>");
                                        //    sb.Append("<td align='left' valign='top' style='font-family: "+ common.myStr(hdnFontName.Value) +";font-size:10pt;'>&nbsp;</td>");
                                        //}
                                    }
                                    if (ColumnCount == 3)
                                    {
                                        i += 2;
                                        dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=1 AND Rowno=" + RowNo;
                                        if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
                                        {
                                            sb.Append("<td width=10% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;font-weight:bold;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</td>");
                                            dtHeaderMerge.DefaultView.RowFilter = "ColNo=2 AND Rowno=" + RowNo;
                                            if (dtHeaderMerge.DefaultView.ToTable().Rows.Count > 0)// this for merge td
                                            {
                                                dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=1 AND Rowno=" + RowNo;
                                                sb.Append("<td width=20% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td>");
                                            }
                                            else
                                            {
                                                dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=1 AND Rowno=" + RowNo;
                                                sb.Append("<td colspan='3' valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td>");
                                            }
                                        }
                                        else
                                        {
                                            sb.Append("<td width=10% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>&nbsp;</td>");
                                            sb.Append("<td width=20% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>&nbsp;</td>");
                                        }
                                        dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=2 AND Rowno=" + RowNo;

                                        if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
                                        {
                                            sb.Append("<td align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;font-weight:bold;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</td>");
                                            sb.Append("<td align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td>");
                                        }
                                        //else
                                        //{
                                        //    sb.Append("<td align='left' style='font-family: "+ common.myStr(hdnFontName.Value) +";font-size:10pt;'></td>");
                                        //    sb.Append("<td align='left' style='font-family: "+ common.myStr(hdnFontName.Value) +";font-size:10pt;'></td>");
                                        //}
                                        dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=3 AND Rowno=" + RowNo;
                                        if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
                                        {
                                            sb.Append("<td align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;font-weight:bold;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</td>");
                                            sb.Append("<td align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td></tr>");
                                        }
                                        else
                                        {
                                            sb.Append("<td align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>&nbsp;</td>");
                                            sb.Append("<td align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>&nbsp;</td>");
                                        }
                                    }

                                }
                                //if (i + 1 != common.myInt(dsHeader.Tables[0].Rows.Count))
                                if ((i + 1) < common.myInt(dsHeader.Tables[0].Rows.Count))
                                {
                                    RowNo = common.myInt(dsHeader.Tables[0].Rows[i + 1]["Rowno"]);

                                    //sb.Append("<tr><td colspan='6'>" + headerRow + "</td></tr>");

                                }

                                //if ((RowNo - 1) < 5)
                                //{
                                if ((RowNo - 1) < 3)
                                {
                                    sbNextHeader = new StringBuilder();
                                    sbNextHeader.Append(sb.ToString());
                                    //sbNextHeader.Append("<tr><td colspan='7'>" + headerRow + "</td></tr>");

                                    sbNextHeader.Append("</table>");
                                    sbNextHeader.Append("<table cellpadding='0' cellspacing='0'><tr><td>" + headerRow + "</td></tr></table>");
                                }
                            }
                            //Change Palendra
                            sb.Append("<tr><td><br/></td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr>");

                            sb.Append("</table>");
                            sb.Append("<table cellpadding='0' cellspacing='0'><tr><td>" + headerRow + "</td></tr></table>");







                            //select d.RowNo, d.ColNo, d.FieldCaption, oo.ObjectValue
                            //from EMRTemplateDataObjects oo
                            //inner join EMRFormHeaderDetails d on oo.Id=d.ObjectId
                            //where oo.Active=1 and d.HeaderId=19 and d.Active=1
                            //Order by d.RowNo, d.ColNo

                            //1   1   Name PN
                            //1   2   UHID RN
                            //2   1   Consultant CDN
                            //2   2   IP No.	EN
                            //3   1   Company CMN
                            //3   2   Age / Gender  PAG
                            //4   1   Bed No  BNO
                            //4   2   DOA DOA
                            //5   1   Contact No  PMN
                            //5   2   DOD DOD
                            //6   1   Address PADD

                            //sb.Append("<table border='0' width='100%' cellpadding='0' cellspacing='0'>");

                            //sb.Append("<tr>");
                            //sb.Append("<td>&nbsp;</td>");
                            //sb.Append("<td>&nbsp;</td>");
                            //sb.Append("<td>&nbsp;</td>");
                            //sb.Append("<td>&nbsp;</td>");
                            //sb.Append("<td>&nbsp;</td>");
                            //sb.Append("<td>&nbsp;</td>");
                            //sb.Append("<td>&nbsp;</td>");
                            //sb.Append("<td>&nbsp;</td>");
                            //sb.Append("<td>&nbsp;</td>");
                            //sb.Append("<td>&nbsp;</td>");
                            //sb.Append("<td>&nbsp;</td>");
                            //sb.Append("<td>&nbsp;</td>");
                            //sb.Append("<td>&nbsp;</td>");
                            //sb.Append("<td>&nbsp;</td>");
                            //sb.Append("<td>&nbsp;</td>");
                            //sb.Append("<td>&nbsp;</td>");
                            //sb.Append("<td>&nbsp;</td>");
                            //sb.Append("<td>&nbsp;</td>");
                            //sb.Append("<td>&nbsp;</td>");
                            //sb.Append("<td>&nbsp;</td>");
                            //sb.Append("</tr>");

                            //sb.Append("<tr><td colspan='20' >" + headerRow + "</td></tr>");

                            ////row1
                            //sb.Append("<tr align='left'>");

                            //dsHeader.Tables[0].DefaultView.RowFilter = "";
                            //dsHeader.Tables[0].DefaultView.RowFilter = "ObjectValue='PN'";

                            //if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
                            //{
                            //    sb.Append("<td colspan='3' valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;font-weight:bold;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</td>");
                            //    sb.Append("<td colspan='9' valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td>");
                            //}
                            //else
                            //{
                            //    sb.Append("<td colspan='3' valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;'>&nbsp;</td>");
                            //    sb.Append("<td colspan='9' valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;'>&nbsp;</td>");
                            //}

                            //dsHeader.Tables[0].DefaultView.RowFilter = "";
                            //dsHeader.Tables[0].DefaultView.RowFilter = "ObjectValue='RN'";

                            //if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
                            //{
                            //    sb.Append("<td colspan='3' valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;font-weight:bold;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</td>");
                            //    sb.Append("<td colspan='5' valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td>");
                            //}
                            //else
                            //{
                            //    sb.Append("<td colspan='3' valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;'>&nbsp;</td>");
                            //    sb.Append("<td colspan='5' valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;'>&nbsp;</td>");
                            //}
                            //sb.Append("</tr>");

                            ////sb.Append("<tr><td colspan='20'>" + headerRow + "</td></tr>");

                            ////Next Header
                            //sbNextHeader = new StringBuilder();
                            //sbNextHeader.Append(sb.ToString() + "<tr><td colspan='20' >" + headerRow + "</td></tr>");
                            //sbNextHeader.Append("</table>");

                            ////row2
                            //sb.Append("<tr align='left'>");

                            //dsHeader.Tables[0].DefaultView.RowFilter = "";
                            //dsHeader.Tables[0].DefaultView.RowFilter = "ObjectValue='CDN'";

                            //if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
                            //{
                            //    sb.Append("<td colspan='3' valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;font-weight:bold;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</td>"); //"Consultant" 
                            //    sb.Append("<td colspan='9' valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td>");
                            //}
                            //else
                            //{
                            //    sb.Append("<td colspan='3' valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;'>&nbsp;</td>");
                            //    sb.Append("<td colspan='9' valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;'>&nbsp;</td>");
                            //}

                            //dsHeader.Tables[0].DefaultView.RowFilter = "";
                            //dsHeader.Tables[0].DefaultView.RowFilter = "ObjectValue='EN'";

                            //if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
                            //{
                            //    sb.Append("<td colspan='3' valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;font-weight:bold;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</td>");
                            //    sb.Append("<td colspan='5' valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td>");
                            //}
                            //else
                            //{
                            //    sb.Append("<td colspan='3' valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;'>&nbsp;</td>");
                            //    sb.Append("<td colspan='5' valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;'>&nbsp;</td>");
                            //}

                            //sb.Append("</tr>");

                            ////sb.Append("<tr><td colspan='20'>" + headerRow + "</td></tr>");

                            ////row3
                            //sb.Append("<tr align='left'>");

                            //dsHeader.Tables[0].DefaultView.RowFilter = "";
                            //dsHeader.Tables[0].DefaultView.RowFilter = "ObjectValue='CMN'";

                            //if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
                            //{
                            //    sb.Append("<td colspan='3' valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;font-weight:bold;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</td>");
                            //    sb.Append("<td colspan='9' valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td>");
                            //}
                            //else
                            //{
                            //    sb.Append("<td colspan='3' valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;'>&nbsp;</td>");
                            //    sb.Append("<td colspan='9' valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;'>&nbsp;</td>");
                            //}

                            //dsHeader.Tables[0].DefaultView.RowFilter = "";
                            //dsHeader.Tables[0].DefaultView.RowFilter = "ObjectValue='PAG'";

                            //if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
                            //{
                            //    sb.Append("<td colspan='3' valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;font-weight:bold;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</td>");
                            //    sb.Append("<td colspan='5' valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td>");
                            //}
                            //else
                            //{
                            //    sb.Append("<td colspan='3' valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;'>&nbsp;</td>");
                            //    sb.Append("<td colspan='5' valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;'>&nbsp;</td>");
                            //}
                            //sb.Append("</tr>");

                            ////sb.Append("<tr><td colspan='20'>" + headerRow + "</td></tr>");

                            ////row4
                            //sb.Append("<tr align='left'>");

                            //dsHeader.Tables[0].DefaultView.RowFilter = "";
                            //dsHeader.Tables[0].DefaultView.RowFilter = "ObjectValue='BNO'";

                            //if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
                            //{
                            //    sb.Append("<td colspan='3' valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;font-weight:bold;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</td>");
                            //    sb.Append("<td colspan='9' valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td>");
                            //}
                            //else
                            //{
                            //    sb.Append("<td colspan='3' valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;'>&nbsp;</td>");
                            //    sb.Append("<td colspan='9' valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;'>&nbsp;</td>");
                            //}

                            //dsHeader.Tables[0].DefaultView.RowFilter = "";
                            //dsHeader.Tables[0].DefaultView.RowFilter = "ObjectValue='DOA'";

                            //if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
                            //{
                            //    sb.Append("<td colspan='3' valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;font-weight:bold;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</td>");
                            //    sb.Append("<td colspan='5' valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td>");
                            //}
                            //else
                            //{
                            //    sb.Append("<td colspan='3' valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;'>&nbsp;</td>");
                            //    sb.Append("<td colspan='5' valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;'>&nbsp;</td>");
                            //}

                            //sb.Append("</tr>");

                            ////sb.Append("<tr><td colspan='20'>" + headerRow + "</td></tr>");

                            ////row5
                            //sb.Append("<tr align='left'>");

                            //dsHeader.Tables[0].DefaultView.RowFilter = "";
                            //dsHeader.Tables[0].DefaultView.RowFilter = "ObjectValue='PMN'";

                            //if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
                            //{
                            //    sb.Append("<td colspan='3' valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;font-weight:bold;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</td>");
                            //    sb.Append("<td colspan='9' valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td>");
                            //}
                            //else
                            //{
                            //    sb.Append("<td colspan='3' valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;'>&nbsp;</td>");
                            //    sb.Append("<td colspan='9' valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;'>&nbsp;</td>");
                            //}

                            //dsHeader.Tables[0].DefaultView.RowFilter = "";
                            //dsHeader.Tables[0].DefaultView.RowFilter = "ObjectValue='DOD'";

                            //if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
                            //{
                            //    sb.Append("<td colspan='3' valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;font-weight:bold;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</td>");
                            //    sb.Append("<td colspan='5' valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td>");
                            //}
                            //else
                            //{
                            //    sb.Append("<td colspan='3' valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;'>&nbsp;</td>");
                            //    sb.Append("<td colspan='5' valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;'>&nbsp;</td>");
                            //}

                            //sb.Append("</tr>");

                            ////sb.Append("<tr><td colspan='20'>" + headerRow + "</td></tr>");

                            ////row6
                            //dsHeader.Tables[0].DefaultView.RowFilter = "";
                            //dsHeader.Tables[0].DefaultView.RowFilter = "ObjectValue='PSP'";

                            //if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
                            //{
                            //    sb.Append("<tr align='left'>");

                            //    if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
                            //    {
                            //        sb.Append("<td colspan='3'  valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;font-weight:bold;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</td>");
                            //        sb.Append("<td colspan='17' valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td>");
                            //    }
                            //    else
                            //    {
                            //        sb.Append("<td colspan='3'  valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;'>&nbsp;</td>");
                            //        sb.Append("<td colspan='17' valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;'>&nbsp;</td>");
                            //    }

                            //    sb.Append("</tr>");
                            //}

                            ////sb.Append("<tr><td colspan='20'>" + headerRow + "</td></tr>");

                            ////row7
                            //sb.Append("<tr align='left'>");

                            //dsHeader.Tables[0].DefaultView.RowFilter = "";
                            //dsHeader.Tables[0].DefaultView.RowFilter = "ObjectValue='PADD'";

                            //if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
                            //{
                            //    sb.Append("<td colspan='3'  valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;font-weight:bold;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</td>");
                            //    sb.Append("<td colspan='17' valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td>");
                            //}
                            //else
                            //{
                            //    sb.Append("<td colspan='3'  valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;'>&nbsp;</td>");
                            //    sb.Append("<td colspan='17' valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:11pt;'>&nbsp;</td>");
                            //}

                            //sb.Append("</tr>");

                            //sb.Append("<tr><td colspan='20'>" + headerRow + "</td></tr>");

                            //sb.Append("</table>");




                        }
                        #endregion
                    }
                    else
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
                                    //i = +1;
                                    //sb.Append("<td align='left'>" + common.myStr(dsHeader.Tables[0].Rows[i]["FieldCaption"]) + "</td>");
                                    //sb.Append("<td align='left'>dsadas</td>");
                                    //sb.Append("<td align='left'>" + common.myStr(dsHeader.Tables[0].Rows[i]["FieldCaption"]) + "</td>");
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
                        //Change palendra
                        sb.Append("<tr><td><br/></td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr>");
                        sb.Append("<table cellpadding='0' cellspacing='0'><tr><td>" + headerRow + "</td></tr></table>");
                    }

                }
            }

            #endregion

            sbtopheader.Append(sbTitle.ToString());

            sbtopheader.Append(sb.ToString());



            if (common.myStr(ViewState["IsShowOtherDoctorInDischargeSummary"]).ToUpper().Equals("Y"))
            {
                sbtopheader.Append(getOtherDoctorInDischargeSummary(true));
                sbtopheader.Append(headerRow);
            }

            // DataSet dsFooter = objHospital.GetReportFooter(SOURCE, common.myInt(LABNO));
            // StringBuilder sb1 = new StringBuilder();

            #region Footer section
            //if (dsFooter.Tables.Count > 0)
            //{
            //    int row = 0;
            //    if (dsFooter.Tables[0].Rows.Count > 0)
            //    {
            //        sb1.Append("<table border=0 cellpadding=0 cellspacing=0>");
            //        foreach (DataRow dr in dsFooter.Tables[0].Rows)
            //        {                       
            //            sb1.Append("<tr>");
            //            sb1.Append("<td>" + common.myStr(dr["LeftDoctorName"]) + "</td>");
            //            sb1.Append("<td>" + common.myStr(dr["CenterDoctorName"]) + "</td>");
            //            sb1.Append("<td>" + common.myStr(dr["RightDoctorName"]) + "</td>");
            //            sb1.Append("</tr>");
            //            sb1.Append("<tr>");
            //            sb1.Append("<td>" + common.myStr(dr["Leftdesignation"]) + "</td>");
            //            sb1.Append("<td>" + common.myStr(dr["CenterDsignation"]) + "</td>");
            //            sb1.Append("<td>" + common.myStr(dr["RightDesignation"]) + "</td>");
            //            sb1.Append("</tr>");
            //        }
            //    }
            //}

            #endregion

            #region Lowest Footer section

            //StringBuilder sb2 = new StringBuilder();// this is use for lowest footer
            //if (dsFooter.Tables.Count > 1)
            //{
            //    if (dsFooter.Tables[1].Rows.Count > 0)
            //    {
            //        sb2.Append(" " + common.myStr(ds.Tables[0].Rows[0]["FooterName"]).Trim());
            //        sb2.Append("\n  " + common.myStr(ds.Tables[0].Rows[1]["FooterName"]).Trim());
            //        sb2.Append("\n   " + common.myStr(ds.Tables[0].Rows[2]["FooterName"]).Trim());
            //    }
            //    else
            //    {
            //        sb2.Append(" Aster DM Healthcare Pvt Ltd ");
            //        sb2.Append("\n Kuttisahib Road,Near Kothad Bridge,South Chittor PO,Cheranallor,Kochi 682027,Kerala,India ");
            //        sb2.Append("\n T +91 484 6699999 E info@astermedcity.com W astermedcity.com ");
            //    }
            //}
            //else
            //{
            //    sb2.Append(" Aster DM Healthcare Pvt Ltd ");
            //    sb2.Append("\n Kuttisahib Road,Near Kothad Bridge,South Chittor PO,Cheranallor,Kochi 682027,Kerala,India ");
            //    sb2.Append("\n T +91 484 6699999 E info@astermedcity.com W astermedcity.com ");
            //}

            #endregion

            #region FooterN
            StringBuilder sbFooterLabel = new StringBuilder();


            if (common.myStr(Session["FacilityName"]).ToUpper().Contains("VENKATESHWAR"))
            {
                sbFooterLabel.Append("<table cellpadding='0' cellspacing='0'>");

                sbFooterLabel.Append("<tr><td valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + "; font-size:11pt;font-style:italic;'>Please note that document cannot be altered or modified</td></tr>");
                sbFooterLabel.Append("<tr><td>&nbsp;</td></tr>");

                if (ds.Tables[0].Rows.Count > 0)
                {
                    sbFooterLabel.Append("<tr><td valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + "; font-size:11pt;'><b>In case of need, contact : </b> " + common.myStr(ds.Tables[0].Rows[0]["DepartmentContactNo"]) + "<b> | Email : </b>" + common.myStr(ds.Tables[0].Rows[0]["DepartmentEmailId"]) + " </td></tr>");

                    sbFooterLabel.Append("<tr><td>&nbsp;</td></tr>");

                    sbFooterLabel.Append("<tr><td valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + "; font-size:11pt;'><span style='font-weight:bold'>" + common.myStr(ds.Tables[0].Rows[0]["FacilityName"]) + ",</span> " + common.myStr(ds.Tables[0].Rows[0]["LocationDescription"]) + "</td></tr>");
                }

                sbFooterLabel.Append("</table>");
            }

            #endregion

            if (ds.Tables[0].Rows.Count > 0)
            {
                StringBuilder sbHospitalHeader = new StringBuilder();
                sbHospitalHeader.Append(getReportHeader(common.myInt(Request.QueryString["ReportId"])));


                clsIVF objIVF = new clsIVF(sConString);

                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                string EmpNo = common.myStr(objDl.ExecuteScalar(CommandType.Text, "select e.EmployeeNo from Employee e WITH (NOLOCK) inner join Users u WITH (NOLOCK) on e.ID=u.EmpID where  u.id=" + common.myInt(Session["UserId"])));

                string Addendum = string.Empty;

                if (common.myLen(ViewState["Addendum"]) > 0)
                {
                    Addendum = "</br> <b>ADDENDUM </b>: " + common.myStr(ViewState["Addendum"]).Trim();
                }

                StringBuilder sbPatientSummary = new StringBuilder();

                sbPatientSummary.Append("<div style='font-size:" + hdnFontSize.Value + "pt;'>");
                sbPatientSummary.Append(common.myStr(ds.Tables[0].Rows[0]["PatientSummary"]) + Addendum);
                sbPatientSummary = sbPatientSummary.Replace("font-weight: bold; font-family: " + common.myStr(hdnFontName.Value).ToLower() + ";", "font-size:9pt; font-family: " + common.myStr(hdnFontName.Value).ToLower() + "; font-weight:bold;");
                sbPatientSummary = sbPatientSummary.Replace("font-weight: 700;", "font-size:" + hdnFontSize.Value + "pt;");

                sbPatientSummary = sbPatientSummary.Replace("..", "&#46;&#46;");
                sbPatientSummary = sbPatientSummary.Replace("..", "&#46;&#46;");
                sbPatientSummary = sbPatientSummary.Replace("...", "&#46;&#46;&#46;");
                sbPatientSummary = sbPatientSummary.Replace(".", "&#46;");
                sbPatientSummary.Append("</div>");
                clsParsePDFDischarge cPrint = new clsParsePDFDischarge();
                cPrint.GetFontName();



                if (common.myStr(Session["FacilityName"]).ToUpper().Contains("NEMCARE"))
                {
                    cPrint.Html = sbPatientSummary.ToString() + GetSignatureDetails() + sbFooterLabel.ToString();
                }
                else
                {
                    if (common.myStr(ViewState["SignaturePrintOnlyInFinalization"]).Equals("Y"))
                    {
                        if (common.myStr(Request.QueryString["Finalize"]).Equals("DF"))
                        {
                            if (common.myBool(ViewState["IsPrintDoctorSignature"]))
                            {
                                cPrint.Html = sbPatientSummary.ToString() + getReportsSignature(true) + sbFooterLabel.ToString();
                            }
                            else
                            {
                                cPrint.Html = sbPatientSummary.ToString() + getReportsSignature(false) + sbFooterLabel.ToString();
                            }
                        }
                        else
                        {
                            cPrint.Html = sbPatientSummary.ToString() + getReportsSignature(false) + sbFooterLabel.ToString();
                        }
                    }
                    else
                    {
                        if (common.myBool(ViewState["IsPrintDoctorSignature"]))
                        {
                            //ViewState["SavedReportFormat"]
                            //Awadhesh

                            if (ImageTextPrintAfterDoctorSignatureonDischargeSummary == "Y")
                            {
                                cPrint.Html = sbPatientSummary.ToString() + getReportsSignature(true) + GetImageText(common.myInt(Request.QueryString["ReportId"])) + sbFooterLabel.ToString();
                            }
                            else
                            {
                                cPrint.Html = sbPatientSummary.ToString() + GetImageText(common.myInt(Request.QueryString["ReportId"])) + getReportsSignature(true) + sbFooterLabel.ToString();
                            }

                        }
                        else
                        {
                            cPrint.Html = sbPatientSummary.ToString() + GetImageText(common.myInt(Request.QueryString["ReportId"])) + getReportsSignature(false) + sbFooterLabel.ToString();
                        }
                    }
                }

                if (common.myStr(Request.QueryString["For"]).ToUpper().Equals("DISSUM")
                    && common.myStr(ViewState["IsShowAllDepartmentDoctorsInDischargeSummary"]).ToUpper().Equals("Y"))
                {
                    StringBuilder sbShowAllDepartmentDoctors = new StringBuilder();

                    sbShowAllDepartmentDoctors.Append(ShowAllDepartmentDoctors(common.myInt(ds.Tables[0].Rows[0]["ConsultingDoctorDepartmentId"]),
                                                common.myStr(ds.Tables[0].Rows[0]["ConsultingDoctorDepartment"]),
                                                common.myStr(ds.Tables[0].Rows[0]["EntrySite"])));

                    if (sbShowAllDepartmentDoctors.ToString() != string.Empty)
                    {
                        sbFooterLabel = new StringBuilder();

                        if (common.myStr(Session["FacilityName"]).ToUpper().Contains("HEALTHWORLD"))
                        {
                            sbFooterLabel.Append("<table cellpadding='0' cellspacing='0' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>");
                            sbFooterLabel.Append("<tr><td valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + "; font-size:10pt;'>Discharge summary explained by: . . . . . . . . . . . . . . . &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Discharge summary received by: . . . . . . . . . . . . . . . </td></tr>");
                            sbFooterLabel.Append("</table>");
                        }

                        cPrint.Html = cPrint.Html + "<br/>" + sbShowAllDepartmentDoctors.ToString() + sbFooterLabel.ToString();

                    }
                }

                //cPrint.FirstPageHeaderHtml = sbHospitalHeader.ToString() + sbtopheader.ToString();//sb.ToString();
                // if(common.myBool( ds.Tables[0].Rows[0]["IsPrint"]))//Printed by
                //cPrint.FooterLeftText = "Printed By: " + EmpNo;// common.myStr(Session["EmployeeName"]);

                //if (common.myBool(ds.Tables[0].Rows[0]["IsPrintDate"]))//Prinded data 
                //cPrint.FooterCenterText = "Printed Date: " + common.myStr(DateTime.Now.ToString("dd/MM/yyyy hh:mm tt"));

                //cPrint.FirstPageFooterLeft = "LeftText";
                //cPrint.FooterLeft = "LeftText";
                //cPrint.FirstPageFooterMiddle = "CenterText";
                //cPrint.FooterMiddle = "CenterText";
                //cPrint.FooterRight = "PageNofN";
                //cPrint.FirstPageFooterRight = "PageNofN";
                //cPrint.LowestFooter = sb2.ToString();// lowest footer
                //cPrint.FirstPageFooterHtml = sb2.ToString();


                switch (common.myStr(Request.QueryString["For"]))
                {
                    case "DISSUM":
                        cPrint.FooterLeftText = common.myStr(ds.Tables[0].Rows[0]["DISSUMNo"]);
                        break;

                    case "DthSum":
                        cPrint.FooterLeftText = common.myStr(ds.Tables[0].Rows[0]["DthSumNo"]);
                        break;
                }

                //yogesh 9/05/2022

                if (common.myStr(Session["FacilityName"]).ToUpper().Contains("BALAJI") || common.myStr(Session["FacilityName"]).ToUpper().Contains("SRI�LAJI�TION MEDICAL INSTITUTE") || common.myStr(Session["FacilityName"]).ToUpper().Contains("ACTION CANCER HOSPITAL"))
                {
                    cPrint.FooterLeftText = cPrint.FooterLeftText + " Prepared By: " + common.myStr(ViewState["PreparedByName"]);

                    if (common.myInt(ds.Tables[0].Rows[0]["FinalizeBy"]) > 0)
                    {
                        if (common.myBool(ds.Tables[0].Rows[0]["Finalize"]))
                        {
                            cPrint.FooterCenterText = "FinalizeBy : " + common.myStr(ViewState["FinalizeByName"]);
                        }
                    }
                }
                else
                {
                    if (common.myBool(ViewState["ShowPrintByInPageFooter"]))
                        cPrint.FooterLeftText = cPrint.FooterLeftText + " Printed By: " + common.myStr(Session["UserName"]);

                    if (common.myBool(ViewState["ShowLastUpdatedByNameInPageFooter"]))
                        cPrint.FooterLeftText = cPrint.FooterLeftText + "\n" + " Prepared By: " + common.myStr(ViewState["LastUpdatedByName"]);

                    if (common.myBool(ViewState["ShowLastUpdatedDateInPageFooter"]))
                        cPrint.FooterLeftText = cPrint.FooterLeftText + "\n" + " Prepared Date: " + common.myStr(ViewState["LastUpdatedDate"]);

                    if (common.myBool(ViewState["ShowPrintDateInPageFooter"]))
                        cPrint.FooterCenterText = "Print Date & Time: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm");

                    // yogesh 14/06/2022
                    if (Session["FacilityName"].ToString().Equals("Regency Hospital Ltd.") || Session["FacilityName"].ToString().Equals("Regency City Clinic") ||
                       Session["FacilityName"].ToString().Equals("Regency Hospital, Govind Nagar") || Session["FacilityName"].ToString().Equals("Regency Renal Hospital") ||
                       Session["FacilityName"].ToString().Equals("Regency Hospital, Lucknow"))
                    {
                        cPrint.FooterLeftText = cPrint.FooterLeftText + "\n" + "Encoded By:" + " " + ViewState["PreparedByName"].ToString();
                        cPrint.FooterCenterText = cPrint.FooterCenterText + "\n" + " " + "Encoded Date:" + " " + ViewState["LastUpdatedDate"].ToString();
                    }


                }

                int MarginTop = 0;
                int MarginBottom = 0;
                int MarginLeft = 0;
                int MarginRight = 0;
                string PageSize = "A4";

                objIVF.getReportSetupMargin(common.myInt(Request.QueryString["ReportId"]), out MarginTop, out MarginBottom, out MarginLeft, out MarginRight, out PageSize);

                if (MarginTop.Equals(0))
                {
                    cPrint.MarginTop = 60;
                }
                if (MarginBottom.Equals(0))
                {
                    cPrint.MarginBottom = 60;
                }
                if (MarginLeft.Equals(0))
                {
                    cPrint.MarginLeft = 22;
                }
                if (MarginRight.Equals(0))
                {
                    cPrint.MarginRight = 13;
                }
                if (common.myLen(PageSize).Equals(0))
                {
                    PageSize = "A4";
                }

                if (common.myStr(Session["FacilityName"]).ToUpper().Contains("QRG MEDICARE LTD") || common.myStr(Session["FacilityName"]).ToUpper().Contains("QRG CENTRAL HOSPITAL & RESEARCH CENTRE LTD")
            || common.myStr(Session["FacilityName"]).ToUpper().Contains("BALAJI") || common.myStr(Session["FacilityName"]).ToUpper().Contains("SRI�LAJI�TION MEDICAL INSTITUTE") || common.myStr(Session["FacilityName"]).ToUpper().Contains("ACTION CANCER HOSPITAL"))
                {
                    cPrint.HeaderHtml = sbHospitalHeader.ToString() + sbtopheader.ToString();
                    cPrint.FirstPageHeaderHtml = sbHospitalHeader.ToString() + sbtopheader.ToString();
                }

                else if (common.myStr(ViewState["ShowHeaderOnAllPagesOfDischargeSummary"]).Equals("Y"))
                {
                    cPrint.HeaderHtml = sbHospitalHeader.ToString() + sbNextHeader.ToString();
                    cPrint.FirstPageHeaderHtml = sbHospitalHeader.ToString() + sbtopheader.ToString();

                }
                else if (common.myStr(ViewState["ShowHeaderAllDetailOnAllPagesOfDischargeSummary"]).Equals("Y"))
                {
                    cPrint.HeaderHtml = sbHospitalHeader.ToString() + sbtopheader.ToString();
                    cPrint.FirstPageHeaderHtml = sbHospitalHeader.ToString() + sbtopheader.ToString();

                }
                else if (common.myStr(Session["FacilityName"]).ToUpper().Contains("QRG MEDICARE LTD") || common.myStr(Session["FacilityName"]).ToUpper().Contains("QRG CENTRAL HOSPITAL & RESEARCH CENTRE LTD"))
                {
                    cPrint.HeaderHtml = sbHospitalHeader.ToString() + sbtopheader.ToString();
                    cPrint.FirstPageHeaderHtml = sbHospitalHeader.ToString() + sbtopheader.ToString();
                }
                else
                {
                    cPrint.HeaderHtml = string.Empty;
                    cPrint.FirstPageHeaderHtml = sbHospitalHeader.ToString() + sbtopheader.ToString();
                }

                if (common.myLen(ViewState["ReportFooterText"]) > 0 && common.myBool(ViewState["ShowPrintFooterImage"]).Equals(false))
                {
                    cPrint.LowestFooter = common.myStr(ViewState["ReportFooterText"]) + Environment.NewLine + " ";
                }

                //Change Palendra
                else if (common.myBool(ViewState["ShowPrintFooterImage"]).Equals(true))
                {
                    string Path = Server.MapPath("" + ViewState["PrintFooterImagePath"] + "");
                    cPrint.LowestFooterImage = Path;
                }

                else
                {
                    cPrint.LowestFooter = " " + Environment.NewLine + " ";

                }

                string strHTML = cPrint.Html;

                strHTML = common.removeUnusedHTML(strHTML);
                cPrint.Html = strHTML;
                //cPrint.MarginLeft = 22;
                //cPrint.MarginRight = 13;
                //cPrint.MarginTop = 60;
                //cPrint.MarginBottom = 60;

                cPrint.MarginLeft = MarginLeft;
                cPrint.MarginRight = MarginRight;
                cPrint.MarginTop = MarginTop;
                cPrint.MarginBottom = MarginBottom;
                cPrint.Size = iTextSharp.text.PageSize.GetRectangle(PageSize);
                MemoryStream m = cPrint.ParsePDF();
                Response.Clear();
                Response.ClearContent();
                Response.ClearHeaders();

                if (common.myBool(Request.QueryString["ExportToWord"]))
                {
                    #region Download Word 
                    var strBody = new StringBuilder();
                    //-- add required formatting to html
                    AddFormatting(strBody, sbHospitalHeader.ToString() + sbtopheader.ToString() + sbPatientSummary.ToString() + getReportsSignature(true) + sbFooterLabel.ToString());
                    //-- download file.. of you can write code to save word in any application folder
                    DownloadWord(strBody.ToString());

                    #endregion
                }
                else
                {
                    #region Download PDF 
                    Response.ContentType = "application/pdf";
                    Response.AppendHeader("Content-Disposition", "inline;filename=download_report.pdf");
                    Response.AppendHeader("Content-Length", m.GetBuffer().Length.ToString());

                    Response.OutputStream.Write(m.GetBuffer(), 0, m.GetBuffer().Length);
                    Response.OutputStream.Flush();
                    Response.OutputStream.Close();
                    Response.End();
                    #endregion
                }

            }

        }
    }

    private void DownloadWord(string strBody)
    {
        HttpContext.Current.Response.Clear();
        HttpContext.Current.Response.Charset = "";
        HttpContext.Current.Response.ContentType = "application/msword";
        string strFileName = "DischargeSummary_" + common.myStr(Session["RegistrationNo"]) + "_" + common.myStr(DateTime.Now) + ".doc";
        HttpContext.Current.Response.AddHeader("Content-Disposition", "inline;filename=" + strFileName);

        HttpContext.Current.Response.Write(strBody);
        HttpContext.Current.Response.End();
        HttpContext.Current.Response.Flush();
    }


    private void AddFormatting(StringBuilder strBody, string yourHtmlContent)
    {
        strBody.Append("<html " +
            "xmlns:o='urn:schemas-microsoft-com:office:office' " +
            "xmlns:w='urn:schemas-microsoft-com:office:word'" +
            "xmlns='http://www.w3.org/TR/REC-html40'>" +
            "<head><title>Time</title>");

        //The setting specifies document's view after it is downloaded as Print
        //instead of the default Web Layout
        strBody.Append("<!--[if gte mso 9]>" +
            "<xml>" +
            "<w:WordDocument>" +
            "<w:View>Print</w:View>" +
            "<w:Zoom>90</w:Zoom>" +
            "<w:DoNotOptimizeForBrowser/>" +
            "</w:WordDocument>" +
            "</xml>" +
            "<![endif]-->");

        strBody.Append("<style>" +
            "<!-- /* Style Definitions */" +
            "@page Section1" +
            "   {size:8.5in 11.0in; " +
            "   margin:1.0in 1.25in 1.0in 1.25in ; " +
            "   mso-header-margin:.5in; " +
            "   mso-footer-margin:.5in; mso-paper-source:0;}" +
            " div.Section1" +
            "   {page:Section1;}" +
            "-->" +
            "</style></head>");

        strBody.Append("<body lang=EN-US style='tab-interval:.5in'>" +
            "<div class=Section1>");
        strBody.Append(yourHtmlContent);
        strBody.Append("</div></body></html>");
    }

    private string GetSignatureDetails()
    {
        string sImage = "";
        StringBuilder sbSign = new StringBuilder();
        DoctorSignImage(common.myInt(ViewState["SignDoctorID"]));

        if (common.myInt(ViewState["SignDoctorID"]) > 0)
        {
            //sbSign.Append("<div  style='vertical-align:bottom;'>");
            sbSign.Append("<table width='100%' cellspacing='0' cellpadding='5' border='0' style='font-family: Arial;font-size:10pt;'>");
            sbSign.Append("<tr><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>");

            //sbSign.Append("<tr>");
            //sbSign.Append("<td></td><td></td><td></td><td></td>");
            //sbSign.Append("<td colspan='3' align='right'>");
            //sbSign.Append(hdnDoctorImage.Value.ToString());
            //sbSign.Append("</td>");
            //sbSign.Append("</tr>");

            //sbSign.Append("<tr>");
            //sbSign.Append("<td></td><td></td><td></td><td></td>");
            //sbSign.Append("<td colspan='3' valign='top' align='right'>");
            //sbSign.Append(common.myStr(ViewState["SignDoctorName"]).Trim() + " " + ", <SPAN style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:9pt;'>" + common.myStr(ViewState["Education"]).Trim() + "</SPAN>");
            //sbSign.Append("</td>");
            //sbSign.Append("</tr>");


            //sbSign.Append("<td></td><td></td><td></td><td></td>");
            //sbSign.Append("<tr>");
            //sbSign.Append("<td style='font-family: "+ common.myStr(hdnFontName.Value) +";font-size:9pt;'>");
            //sbSign.Append("(" + common.myStr(ViewState["Education"]) + ")");
            //sbSign.Append("</td>");
            //sbSign.Append("</tr>");

            sbSign.Append("<tr>");
            sbSign.Append("<td align='left' colspan='6'>Signature</td>");
            sbSign.Append("<td colspan='3' align='right'>Signature</td>");
            sbSign.Append("</tr>");

            sbSign.Append("<tr>");
            sbSign.Append("<td align='left' colspan='9'>&nbsp;</td>");
            sbSign.Append("</tr>");


            sbSign.Append("<tr>");
            sbSign.Append("<td align='left' colspan='6'>CONSULTANT 1</td>");
            sbSign.Append("<td colspan='3' align='right'>RESIDENT</td>");
            sbSign.Append("</tr>");


            sbSign.Append("<tr>");
            sbSign.Append("<td align='left' colspan='9'>&nbsp;</td>");
            sbSign.Append("</tr>");

            sbSign.Append("<tr>");
            sbSign.Append("<td align='left' colspan='9'>&nbsp;</td>");
            sbSign.Append("</tr>");

            sbSign.Append("<tr>");
            sbSign.Append("<td align='left' colspan='9'>Signature</td>");
            sbSign.Append("</tr>");

            sbSign.Append("<tr>");
            sbSign.Append("<td align='left' colspan='9'>&nbsp;</td>");
            sbSign.Append("</tr>");

            sbSign.Append("<tr>");
            sbSign.Append("<td align='left' colspan='9'>CONSULTANT 2</td>");
            sbSign.Append("</tr>");

            //if (common.myInt(ViewState["SignDoctorID"]) != common.myInt(ViewState["PreparedById"]) && common.myInt(ViewState["PreparedById"]) > 0)
            //{

            sbSign.Append("<tr>");
            sbSign.Append("<td align='left' colspan='9'>&nbsp;</td>");
            sbSign.Append("</tr>");

            sbSign.Append("<tr>");
            sbSign.Append("<td colspan='9' align='left' >Prepared By : " + common.myStr(ViewState["PreparedByName"]) + "</td>");
            sbSign.Append("</tr>");
            //}

            sbSign.Append("</table>");
            sImage = common.myStr(sbSign);
        }
        return sImage;
    }

    private void DoctorSignImageOld()
    {
        if (common.myStr(ViewState["SignDoctorID"]) != "")
        {
            StringBuilder strSQL = new StringBuilder();
            String strSingImagePath = "";
            Hashtable hstInput = new Hashtable();
            string SignImage = "";
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            strSQL.Append("SELECT top 1 esi.ImageId, dd.DoctorId, isnull(e.FirstName,'') + isnull(' ' + e.MiddleName,'') +  isnull(' ' + e.LastName,'') as EmployeeName,");
            strSQL.Append(" SignatureImage, ImageType, Education, GETUTCDATE() as SignatureWithDateTime ");
            strSQL.Append(" FROM Users u WITH (NOLOCK) Inner Join Employee e WITH (NOLOCK) On u.EmpId = e.Id ");
            strSQL.Append(" Left Join EmployeeSignatureImage esi WITH (NOLOCK) On esi.EmployeeId = e.ID ");
            strSQL.Append(" Left Join DoctorDetails dd WITH (NOLOCK) On e.ID = dd.DoctorId  where e.ID = @intId and esi.Active=1  order by esi.ImageId desc");

            hstInput.Add("@intId", common.myInt(ViewState["SignDoctorID"]));

            DataSet ds = dl.FillDataSet(CommandType.Text, strSQL.ToString(), hstInput);
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
        }
    }

    private void DoctorSignImage(int doctorid)
    {
        BaseC.clsLISPhlebotomy lis = new BaseC.clsLISPhlebotomy(sConString);
        DataTable dt = new DataTable();
        DataSet ds = new DataSet();
        clsIVF objivf = new clsIVF(sConString);
        ds = new DataSet();

        dt = objivf.getDoctorSignatureDetails(doctorid, common.myInt(Session["FacilityId"]), common.myInt(Session["HospitalLocationID"])).Tables[0];

        DataSet ds1 = lis.getDoctorImageDetails(doctorid, common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
                                          common.myInt(ViewState["EncounterId"]));

        string EMRFTPSignatureImage = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]),
                   common.myInt(Session["FacilityId"]), "IsEMRFTPDoctorSignature", sConString);
        DataRow dr = ds1.Tables[0].Rows[0] as DataRow;
        string FileName = common.myStr(dr["ImageType"]).Trim();
        if (ds1.Tables[0].Rows[0]["SignatureImage"].ToString() != "" && Convert.IsDBNull((ds1.Tables[0].Rows[0]["SignatureImage"])) != true)
        {
            if (EMRFTPSignatureImage.Equals("Y"))
            {
                try
                {

                    string Splitter = ConfigurationManager.AppSettings["Split"];
                    if (common.myLen(Splitter).Equals(0))
                    {
                        Splitter = "!";
                    }

                    var csplitter = Splitter.ToCharArray();
                    string ftp = ftppath.Split(csplitter)[0].ToString();

                    string Filepath = FileFolder + "DoctorImages/" + FileName;
                    //ftppath + FileFolder + FileName
                    FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftp + Filepath);
                    request.Method = WebRequestMethods.Ftp.DownloadFile;

                    // FTP Server credentials.
                    request.Credentials = new NetworkCredential(ftppath.Split(csplitter)[1].ToString(), ftppath.Split(csplitter)[2].ToString());
                    request.UsePassive = true;
                    request.UseBinary = true;
                    request.EnableSsl = false;
                    //Fetch the Response and read it into a MemoryStream object.

                    if (File.Exists(Server.MapPath("~/" + Filepath)))
                    {
                        File.Delete(Server.MapPath("~/" + Filepath));
                    }
                    //yogesh  10/05/2022
                    bool responseResult = true;
                    try
                    {
                        FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                        using (MemoryStream stream = new MemoryStream())
                        {
                            response.GetResponseStream().CopyTo(stream);
                            string base64String = Convert.ToBase64String(stream.ToArray(), 0, stream.ToArray().Length);
                            byte[] imageBytes = Convert.FromBase64String(base64String);
                            string filePath = Server.MapPath("~/" + Filepath);
                            File.WriteAllBytes(filePath, imageBytes);
                            responseResult = true;
                            int SignDoctorHeight = common.myInt(ViewState["SignDoctorHeight"].ToString());
                            int SignDoctorWidth = common.myInt(ViewState["SignDoctorWidth"].ToString());
                            if ((common.myInt(ViewState["SignDoctorHeight"].ToString()) > 0) && (common.myInt(ViewState["SignDoctorWidth"].ToString()) > 0))
                            {

                                hdnDoctorImage.Value = "<img width='" + SignDoctorWidth + "px' height='" + SignDoctorHeight + "px' src='.../PatientDocuments/DoctorImages/" + FileName + "' />";
                            }
                            else
                            {
                                hdnDoctorImage.Value = "<img width='" + SignDoctorWidth + "px' height='" + SignDoctorHeight + "px' src='.../PatientDocuments/DoctorImages/" + FileName + "' />";
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                Stream strm;
                Object img = dr["SignatureImage"];
                strm = new MemoryStream((byte[])img);
                byte[] buffer = new byte[strm.Length];
                int byteSeq = strm.Read(buffer, 0, System.Convert.ToInt32(strm.Length));
                FileStream fs = new FileStream(Server.MapPath("/PatientDocuments/DoctorImages/" + FileName), FileMode.Create, FileAccess.Write);
                fs.Write(buffer, 0, byteSeq);
                fs.Dispose();
                int SignDoctorHeight = common.myInt(ViewState["SignDoctorHeight"].ToString());
                int SignDoctorWidth = common.myInt(ViewState["SignDoctorWidth"].ToString());
                if ((common.myInt(ViewState["SignDoctorHeight"].ToString()) > 0) && (common.myInt(ViewState["SignDoctorWidth"].ToString()) > 0))
                {

                    hdnDoctorImage.Value = "<img width='" + SignDoctorWidth + "px' height='" + SignDoctorHeight + "px' src='.../PatientDocuments/DoctorImages/" + FileName + "' />";
                }
                else
                {
                    hdnDoctorImage.Value = "<img width='" + SignDoctorWidth + "px' height='" + SignDoctorHeight + "px' src='.../PatientDocuments/DoctorImages/" + FileName + "' />";
                }
            }
        }
        else
        {
            Alert.ShowAjaxMsg("Warning: Doctor Signature Image not uploaded !!", Page);
        }
    }

    private void facilityImage()
    {
        StringBuilder strSQL = new StringBuilder();
        String strSingImagePath = "";
        Hashtable hstInput = new Hashtable();
        string SignImage = "";
        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

        hstInput.Add("@intFacilityId", common.myInt(Session["FacilityId"]));
        hstInput.Add("@intHospId", common.myInt(Session["HospitalLocationId"]));

        strSQL.Append("select LogoImagePath from FacilityMaster WITH (NOLOCK) where Active=1 and FacilityID=@intFacilityId and HospitalLocationID=@intHospId");

        DataSet ds = dl.FillDataSet(CommandType.Text, strSQL.ToString(), hstInput);
        if (ds.Tables[0].Rows.Count > 0)
        {
            FileName = common.myStr(ds.Tables[0].Rows[0]["LogoImagePath"]);
            if (FileName != "")
            {
                SignImage = "<img width='145px' height='66px' src='" + Server.MapPath("~") + FileName + "' />";
                strSingImagePath = Server.MapPath("~") + FileName;
            }
        }
        if (File.Exists(strSingImagePath))
        {
            hdnFacilityImage.Value = SignImage;
        }
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
                    case "PMN"://Patient Mobile No
                        value = common.myStr(ds.Tables[0].Rows[0]["PatientMobileNo"]);
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
                            //value = common.myStr(ds.Tables[0].Rows[0]["DOD"]);
                            if (common.myStr(ds.Tables[0].Rows[0]["DOD"]).Length > 10)
                            {
                                value = common.myDate(ds.Tables[0].Rows[0]["DOD"]).ToString("dd/MM/yyyy hh:mm tt");
                            }
                            else
                            {
                                value = common.myStr(ds.Tables[0].Rows[0]["DOD"]);
                            }
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
                    case "PG":
                        value = common.myStr(ds.Tables[0].Rows[0]["Gender"]);
                        break;
                    case "WBN":
                        value = common.myStr(ds.Tables[0].Rows[0]["WardNameBedNo"]);
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

    public StringBuilder AddTopHeader(int DepartmentId, string ConsultingDoctorDepartment, string EntrySite)
    {
        StringBuilder sbTopHeader = new StringBuilder();
        string headerRow = "<hr width='100%' size='1' />";
        bool printLogo = false;
        bool printTopHeader = false;
        string DischargeSummaryTopHeaderSetup = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]),
             common.myInt(Session["FacilityId"]), "DischargeSummaryTopHeaderSetup", sConString);

        if (DischargeSummaryTopHeaderSetup == "HA")
        {
            return sbTopHeader;
        }
        else if (DischargeSummaryTopHeaderSetup == "SOL")
        {
            printLogo = true;
            printTopHeader = false;
        }
        else if (DischargeSummaryTopHeaderSetup == "SOT")
        {
            printLogo = false;
            printTopHeader = true;
        }
        else if (DischargeSummaryTopHeaderSetup == "SA")
        {
            printLogo = true;
            printTopHeader = true;
        }


        if (printLogo)
        {
            facilityImage();

            sbTopHeader.Append("<table width='96%' cellpadding='0' cellspacing='0' style='font-size:9pt;font-family:" + common.myStr(hdnFontName.Value) + ";'>");
            sbTopHeader.Append("<tr><td align='right'>" + hdnFacilityImage.Value + "</td></tr>");
            sbTopHeader.Append("</table>");
        }

        if (printTopHeader)
        {
            clsIVF objI = new clsIVF(sConString);
            //DataSet dsSpec = new DataSet();

            if (common.myStr(Session["FacilityName"]).ToUpper().Contains("NEMCARE"))
            {
            }
            else
            {
                sbTopHeader.Append(headerRow);
                sbTopHeader.Append("<table width='100%' cellpadding='0' cellspacing='0' style='font-size:9pt;font-family:" + common.myStr(hdnFontName.Value) + ";'>");
                //sbTopHeader.Append("<tr><td valign='top' align='left' style='font-size:9pt;font-family:"+ common.myStr(hdnFontName.Value) +"; Font-Weight:bold;'>Center of Excellence  :  " + EntrySite + " </td></tr>");
                //sbTopHeader.Append("<tr><td>" + headerRow + "</td></tr>");
                sbTopHeader.Append("<tr><td valign='top' align='left' style='font-size:9pt;font-family:" + common.myStr(hdnFontName.Value) + "; Font-Weight:bold;'>Department  :  " + ConsultingDoctorDepartment + "</td></tr>");
                sbTopHeader.Append("<tr><td>" + headerRow + "</td></tr>");

                //sbTopHeader.Append("<tr><td valign='top' colspan='3' align='left' style='width:10px;font-size:9pt;'>Names of Doctors</td><td valign='top' align='right'  ></td>");
                ////sbTopHeader.Append("<td colspan='18' valign='top'  align='left' style='width:10px;font-size:9pt;'>: " + DoctorName + "</td></tr>");
                //sbTopHeader.Append("<td colspan='18' valign='top'  align='left' style='width:10px;font-size:9pt;'>: </td></tr>");
                sbTopHeader.Append("</table>");
            }

            //dsSpec = objI.getDoctorSpecialisation(common.myInt(Session["DoctorId"]));

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable hshInput = new Hashtable();
            hshInput.Add("@intDepartmentId", DepartmentId);
            hshInput.Add("@intFacilityId", common.myInt(Session["FacilityId"]));

            if (common.myInt(Request.QueryString["EncId"]) > 0
                && common.myStr(Request.QueryString["For"]).ToUpper().Equals("DISSUM"))
            {
                hshInput.Add("@intEncounterId", common.myInt(Request.QueryString["EncId"]));
            }

            DataSet dsDoctorDepartmentDetails = new DataSet();
            dsDoctorDepartmentDetails = objI.getDoctorDepartmentDetails(common.myInt(Request.QueryString["EncId"]), common.myInt(Session["FacilityId"]));

            if (dsDoctorDepartmentDetails.Tables.Count > 0)
            {
                if (dsDoctorDepartmentDetails.Tables[0].Rows.Count > 0)
                {
                    hshInput.Add("@intSpecialisationId", common.myInt(dsDoctorDepartmentDetails.Tables[0].Rows[0]["SpecialisationId"]));
                }
            }

            DataSet ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspGetDepartmentDoctorsList", hshInput);

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataView DV = new DataView();
                DataTable tbl = new DataTable();
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
                    sbTopHeader.Append("<table border='0' cellpadding='0' cellspacing='0'>");
                    sbTopHeader.Append("<tr>");

                    for (int idx = 0; idx < colSeq.Count; idx++)
                    {
                        if (idx > 2)//print number of columns
                        {
                            break;
                        }

                        DV = ds.Tables[0].DefaultView;
                        DV.RowFilter = "";

                        DV.RowFilter = "ColumnSequenceNo=" + common.myInt(colSeq[idx]);

                        tbl = DV.ToTable();

                        sbTopHeader.Append("<td valign='top' align='left'>");

                        if (tbl.Rows.Count > 0)
                        {
                            sbTopHeader.Append("<table border='0' cellpadding='0' cellspacing='0'>");

                            foreach (DataRow DR in tbl.Rows)
                            {
                                sbTopHeader.Append("<tr>");
                                sbTopHeader.Append("<td colspan='1' valign='top' align='left' ><span style='font-family:" + common.myStr(hdnFontName.Value) + "; font-weight: bold; font-size:9pt;'>" + common.myStr(DR["ShortDoctorName"]).Trim() + "</span>, <span style='font-family:" + common.myStr(hdnFontName.Value) + ";  font-size:8pt;'>" + common.myStr(DR["Education"]).Trim() + "</span></td>");
                                sbTopHeader.Append("</tr>");
                            }

                            sbTopHeader.Append("</table>");
                        }

                        sbTopHeader.Append("</td>");
                    }

                    sbTopHeader.Append("</tr>");
                    sbTopHeader.Append("</table>");
                }

                sbTopHeader.Append("<table border='0' cellpadding='0' cellspacing='0'><tr><td>" + headerRow + "</td></tr></table>");
            }
        }
        return sbTopHeader;
    }

    public StringBuilder AddTopHeaderNew(int DepartmentId, string ConsultingDoctorDepartment, string EntrySite)
    {
        StringBuilder sbTopHeader = new StringBuilder();
        string headerRow = "<hr width='100%' size='1' />";
        bool printLogo = false;
        bool printTopHeader = false;
        string DischargeSummaryTopHeaderSetup = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]),
                                                common.myInt(Session["FacilityId"]), "DischargeSummaryTopHeaderSetup", sConString);

        if (common.myStr(DischargeSummaryTopHeaderSetup).ToUpper().Equals("HA"))
        {
            return sbTopHeader;
        }
        else if (common.myStr(DischargeSummaryTopHeaderSetup).ToUpper().Equals("SOL"))
        {
            printLogo = true;
            printTopHeader = false;
        }
        else if (common.myStr(DischargeSummaryTopHeaderSetup).ToUpper().Equals("SOT"))
        {
            printLogo = false;
            printTopHeader = true;
        }
        else if (common.myStr(DischargeSummaryTopHeaderSetup).ToUpper().Equals("SA"))
        {
            printLogo = true;
            printTopHeader = true;
        }

        if (printLogo)
        {
            facilityImage();

            sbTopHeader.Append("<table width='96%' cellpadding='0' cellspacing='0' style='font-size:9pt;font-family:" + common.myStr(hdnFontName.Value) + ";'>");
            sbTopHeader.Append("<tr><td align='right'>" + hdnFacilityImage.Value + "</td></tr>");
            sbTopHeader.Append("</table>");
        }

        if (printTopHeader)
        {
            clsIVF objI = new clsIVF(sConString);
            //DataSet dsSpec = new DataSet();

            ////sbTopHeader.Append(headerRow);

            if (common.myStr(ViewState["FlagShowDepartmentInDischargeSummary"]).ToUpper().Equals("Y"))
            {
                if (common.myStr(Session["FacilityName"]).ToUpper().Contains("HEALTHWORLD"))
                {
                    sbTopHeader.Append("<table width='100%' cellpadding='0' cellspacing='0' style='font-size:14pt;font-family:Calibri;'>");
                    sbTopHeader.Append("<tr><td valign='top' align='center' style='font-size:14pt;font-family:Calibri;font-weight:bold;'>DEPARTMENT OF " + ConsultingDoctorDepartment.ToUpper() + "</td></tr>");
                    sbTopHeader.Append("</table>");
                }
                else
                {
                    sbTopHeader.Append("<table width='100%' cellpadding='0' cellspacing='0' style='font-size:12pt;font-family:" + common.myStr(hdnFontName.Value) + ";'>");
                    ////sbTopHeader.Append("<tr><td valign='top' align='left' style='font-size:9pt;font-family:"+ common.myStr(hdnFontName.Value) +"; Font-Weight:bold;'>Center of Excellence  :  " + EntrySite + " </td></tr>");
                    ////sbTopHeader.Append("<tr><td>" + headerRow + "</td></tr>");
                    sbTopHeader.Append("<tr><td valign='top' align='center' style='font-size:12pt;font-family:" + common.myStr(hdnFontName.Value) + ";font-weight:bold;'>DEPARTMENT OF " + ConsultingDoctorDepartment.ToUpper() + "</td></tr>");
                    ////sbTopHeader.Append("<tr><td>" + headerRow + "</td></tr>");

                    ////sbTopHeader.Append("<tr><td valign='top' colspan='3' align='left' style='width:10px;font-size:9pt;'>Names of Doctors</td><td valign='top' align='right'  ></td>");
                    //////sbTopHeader.Append("<td colspan='18' valign='top'  align='left' style='width:10px;font-size:9pt;'>: " + DoctorName + "</td></tr>");
                    ////sbTopHeader.Append("<td colspan='18' valign='top'  align='left' style='width:10px;font-size:9pt;'>: </td></tr>");
                    sbTopHeader.Append("</table>");
                }
            }

            if (!common.myStr(Request.QueryString["For"]).ToUpper().Equals("DISSUM"))
            {
                //dsSpec = objI.getDoctorSpecialisation(common.myInt(Session["DoctorId"]));

                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                Hashtable hshInput = new Hashtable();
                hshInput.Add("@intDepartmentId", DepartmentId);
                hshInput.Add("@intFacilityId", common.myInt(Session["FacilityId"]));

                if (common.myInt(Request.QueryString["EncId"]) > 0
                    && common.myStr(Request.QueryString["For"]).ToUpper().Equals("DISSUM"))
                {
                    hshInput.Add("@intEncounterId", common.myInt(Request.QueryString["EncId"]));
                }

                DataSet dsDoctorDepartmentDetails = new DataSet();
                dsDoctorDepartmentDetails = objI.getDoctorDepartmentDetails(common.myInt(Request.QueryString["EncId"]), common.myInt(Session["FacilityId"]));

                if (dsDoctorDepartmentDetails.Tables.Count > 0)
                {
                    if (dsDoctorDepartmentDetails.Tables[0].Rows.Count > 0)
                    {
                        hshInput.Add("@intSpecialisationId", common.myInt(dsDoctorDepartmentDetails.Tables[0].Rows[0]["SpecialisationId"]));
                    }
                }

                DataSet ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspGetDepartmentDoctorsList", hshInput);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataView DV = new DataView();
                    DataTable tbl = new DataTable();
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
                        sbTopHeader.Append("<table border='0' cellpadding='0' cellspacing='0'>");
                        sbTopHeader.Append("<tr>");

                        for (int idx = 0; idx < colSeq.Count; idx++)
                        {
                            if (idx > 2)//print number of columns
                            {
                                break;
                            }

                            DV = ds.Tables[0].DefaultView;
                            DV.RowFilter = "";

                            DV.RowFilter = "ColumnSequenceNo=" + common.myInt(colSeq[idx]);

                            tbl = DV.ToTable();

                            sbTopHeader.Append("<td valign='top' align='left'>");

                            if (tbl.Rows.Count > 0)
                            {
                                sbTopHeader.Append("<table border='0' cellpadding='0' cellspacing='0'>");

                                foreach (DataRow DR in tbl.Rows)
                                {
                                    sbTopHeader.Append("<tr>");
                                    sbTopHeader.Append("<td colspan='1' valign='top' align='left' ><span style='font-family:" + common.myStr(hdnFontName.Value) + "; font-weight: bold; font-size:9pt;'>" + common.myStr(DR["ShortDoctorName"]).Trim() + "</span>, <span style='font-family:" + common.myStr(hdnFontName.Value) + ";  font-size:8pt;'>" + common.myStr(DR["Education"]).Trim() + "</span></td>");
                                    sbTopHeader.Append("</tr>");
                                }

                                sbTopHeader.Append("</table>");
                            }

                            sbTopHeader.Append("</td>");
                        }

                        sbTopHeader.Append("</tr>");
                        sbTopHeader.Append("</table>");
                    }

                    //sbTopHeader.Append("<table border='0' cellpadding='0' cellspacing='0'><tr><td>" + headerRow + "</td></tr></table>");
                }
            }
        }
        return sbTopHeader;
    }

    private StringBuilder getReportsSignature(bool IsPrintDoctorSignature)
    {
        string sImage = "";
        StringBuilder sbSign = new StringBuilder();


        if (common.myStr(ViewState["IsShowOtherDoctorInDischargeSummary"]).ToUpper().Equals("Y"))
        {
            return sbSign;
        }

        try
        {

            //  DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            clsIVF objivf = new clsIVF(sConString);
            ds = new DataSet();
            // dt = objivf.getDoctorSignatureDetails(common.myInt(Session["DoctorId"]), common.myInt(Session["FacilityId"]), common.myInt(Session["HospitalLocationID"])).Tables[0];
            if (IsPrintDoctorSignature)

            {
                string IsShowJuniorDoctorInDischargeSummary = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), "IsShowJuniorDoctorInDischargeSummary", sConString);
                string IsShowSecondaryDoctorInDischargeSummary = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), "IsShowSecondaryDoctorInDischargeSummary", sConString);

                DoctorSignImage(common.myInt(ViewState["SignDoctorID"]));
                //clsIVF objIVF = new clsIVF(sConString);
                //string url = objIVF.GetReportImageUrl(6);
                //sbSign.Append("<table cellspacing='0' cellpadding='0'>");
                //sbSign.Append("<tr><td><img src='"+ url + "' /></td></tr>");
                //sbSign.Append("</table>");
                sbSign.Append("<table cellspacing='0' cellpadding='0'>");
                sbSign.Append("<tr><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>");
                sbSign.Append("<tr>");
                if (IsShowJuniorDoctorInDischargeSummary.Equals("Y"))
                {
                    #region SignJuniorDoctorID
                    if (common.myInt(ViewState["SignJuniorDoctorID"]) > 0)
                    {
                        DoctorSignImage(common.myInt(ViewState["SignJuniorDoctorID"]));

                        sbSign.Append("<td colspan='3' valign='top'>");
                        sbSign.Append("<table  cellspacing='0' cellpadding='0' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'  >");
                        if (common.myLen(ViewState["SignJuniorDoctorName"]) > 0)
                        {
                            sbSign.Append("<tr>");
                            sbSign.Append("<td>" + hdnDoctorImage.Value.ToString() + "</td>");
                            sbSign.Append("</tr>");

                            sbSign.Append("<tr>");
                            sbSign.Append("<td>" + common.myStr(ViewState["SignJuniorDoctorName"]).Trim() + "</td>");
                            sbSign.Append("</tr>");
                        }

                        if (common.myStr(ViewState["JuniorSignatureLine1"]).Trim().Length > 0)
                        {
                            sbSign.Append("<tr>");
                            sbSign.Append("<td>" + common.myStr(ViewState["JuniorSignatureLine1"]).Trim() + "</td>");
                            sbSign.Append("</tr>");
                        }
                        if (common.myStr(ViewState["JuniorSignatureLine2"]).Trim().Length > 0)
                        {
                            sbSign.Append("<tr>");
                            sbSign.Append("<td>" + common.myStr(ViewState["JuniorSignatureLine2"]).Trim() + "</td>");
                            sbSign.Append("</tr>");
                        }
                        if (common.myStr(ViewState["JuniorSignatureLine3"]).Trim().Length > 0)
                        {
                            sbSign.Append("<tr>");
                            sbSign.Append("<td>" + common.myStr(ViewState["JuniorSignatureLine3"]).Trim() + "</td>");
                            sbSign.Append("</tr>");
                        }
                        if (common.myStr(ViewState["JuniorSignatureLine4"]).Trim().Length > 0)
                        {
                            sbSign.Append("<tr>");
                            sbSign.Append("<td>" + common.myStr(ViewState["JuniorSignatureLine4"]).Trim() + "</td>");
                            sbSign.Append("</tr>");
                        }

                        sbSign.Append("</table>");
                        sImage = common.myStr(sbSign);
                        sbSign.Append("</td>");
                    }
                    else
                    {
                        sbSign.Append("<td colspan='3' valign='top'></td>");
                    }
                    #endregion
                }
                else
                {
                    sbSign.Append("<td colspan='3' valign='top'></td>");
                }

                if (IsShowSecondaryDoctorInDischargeSummary.Equals("Y"))
                {
                    #region SignSecondaryDoctorID
                    if (common.myInt(ViewState["SignSecondaryDoctorID"]) > 0)
                    {
                        DoctorSignImage(common.myInt(ViewState["SignSecondaryDoctorID"]));

                        sbSign.Append("<td colspan='3' valign='top'>");
                        sbSign.Append("<table  cellspacing='0' cellpadding='0' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'  >");
                        if (common.myLen(ViewState["SignSecondaryDoctorName"]) > 0)
                        {
                            sbSign.Append("<tr>");
                            sbSign.Append("<td>" + hdnDoctorImage.Value.ToString() + "</td>");
                            sbSign.Append("</tr>");

                            sbSign.Append("<tr>");
                            sbSign.Append("<td>" + common.myStr(ViewState["SignSecondaryDoctorName"]).Trim() + "</td>");
                            sbSign.Append("</tr>");
                        }

                        if (common.myStr(ViewState["SecondarySignatureLine1"]).Trim().Length > 0)
                        {
                            sbSign.Append("<tr>");
                            sbSign.Append("<td>" + common.myStr(ViewState["SecondarySignatureLine1"]).Trim() + "</td>");
                            sbSign.Append("</tr>");
                        }
                        if (common.myStr(ViewState["SecondarySignatureLine2"]).Trim().Length > 0)
                        {
                            sbSign.Append("<tr>");
                            sbSign.Append("<td>" + common.myStr(ViewState["SecondarySignatureLine2"]).Trim() + "</td>");
                            sbSign.Append("</tr>");
                        }
                        if (common.myStr(ViewState["SecondarySignatureLine3"]).Trim().Length > 0)
                        {
                            sbSign.Append("<tr>");
                            sbSign.Append("<td>" + common.myStr(ViewState["SecondarySignatureLine3"]).Trim() + "</td>");
                            sbSign.Append("</tr>");
                        }
                        if (common.myStr(ViewState["SecondarySignatureLine4"]).Trim().Length > 0)
                        {
                            sbSign.Append("<tr>");
                            sbSign.Append("<td>" + common.myStr(ViewState["SecondarySignatureLine4"]).Trim() + "</td>");
                            sbSign.Append("</tr>");
                        }

                        sbSign.Append("</table>");
                        sImage = common.myStr(sbSign);
                        sbSign.Append("</td>");
                    }
                    else
                    {
                        sbSign.Append("<td colspan='3' valign='top'></td>");
                    }
                    #endregion
                }
                else
                {
                    sbSign.Append("<td colspan='3' valign='top'></td>");
                }

                #region SignDoctorID
                if (common.myInt(ViewState["SignDoctorID"]) > 0)
                {
                    DoctorSignImage(common.myInt(ViewState["SignDoctorID"]));

                    sbSign.Append("<td colspan='3' valign='top'>");
                    sbSign.Append("<table  cellspacing='0' cellpadding='0' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'  >");
                    if (common.myLen(ViewState["SignDoctorName"]) > 0)
                    {
                        sbSign.Append("<tr>");
                        sbSign.Append("<td>" + hdnDoctorImage.Value.ToString() + "</td>");
                        sbSign.Append("</tr>");

                        sbSign.Append("<tr>");
                        sbSign.Append("<td>" + common.myStr(ViewState["SignDoctorName"]).Trim() + "(" + ViewState["SpecializationName"] + ")" + "</td>");
                        sbSign.Append("</tr>");
                    }

                    if (common.myStr(ViewState["SignatureLine1"]).Trim().Length > 0)
                    {
                        sbSign.Append("<tr>");
                        sbSign.Append("<td>" + common.myStr(ViewState["SignatureLine1"]).Trim() + "</td>");
                        sbSign.Append("</tr>");
                    }
                    if (common.myStr(ViewState["SignatureLine2"]).Trim().Length > 0)
                    {
                        sbSign.Append("<tr>");
                        sbSign.Append("<td>" + common.myStr(ViewState["SignatureLine2"]).Trim() + "</td>");
                        sbSign.Append("</tr>");
                    }
                    if (common.myStr(ViewState["SignatureLine3"]).Trim().Length > 0)
                    {
                        sbSign.Append("<tr>");
                        sbSign.Append("<td>" + common.myStr(ViewState["SignatureLine3"]).Trim() + "</td>");
                        sbSign.Append("</tr>");
                    }
                    if (common.myStr(ViewState["SignatureLine4"]).Trim().Length > 0)
                    {
                        sbSign.Append("<tr>");
                        sbSign.Append("<td>" + common.myStr(ViewState["SignatureLine4"]).Trim() + "</td>");
                        sbSign.Append("</tr>");
                    }

                    sbSign.Append("</table>");
                    sImage = common.myStr(sbSign);
                    sbSign.Append("</td>");
                }
                #endregion
                sbSign.Append("</tr></table>");
            }
            else
            {
                StringBuilder sb = new StringBuilder();

                DataTable dt = objivf.getDoctorSignatureDetails(common.myInt(Session["DoctorId"]), common.myInt(Session["FacilityId"]), common.myInt(Session["HospitalLocationID"])).Tables[0];


                if (dt.Rows.Count > 0)
                {
                    sb.Append(" <table border='0' width='100%' style='border-collapse:collapse;' cellpadding='0' cellspacing='0'  >");
                    if (common.myStr(dt.Rows[0]["DoctorName"]).Trim().Length > 0)
                    {
                        sb.Append("<tr>");
                        sb.Append("<td align ='right'><b>" + common.myStr(dt.Rows[0]["DoctorName"]).Trim() + "</b></td>");
                        sb.Append("</tr>");
                    }

                    if (common.myStr(dt.Rows[0]["SignatureLine1"]).Trim().Length > 0)
                    {
                        sb.Append("<tr>");
                        sb.Append("<td align ='right'>" + common.myStr(dt.Rows[0]["SignatureLine1"]).Trim() + "</td>");
                        sb.Append("</tr>");
                    }
                    if (common.myStr(dt.Rows[0]["SignatureLine2"]).Trim().Length > 0)
                    {
                        sb.Append("<tr>");
                        sb.Append("<td align ='right'>" + common.myStr(dt.Rows[0]["SignatureLine2"]).Trim() + "</td>");
                        sb.Append("</tr>");
                    }
                    if (common.myStr(dt.Rows[0]["SignatureLine3"]).Trim().Length > 0)
                    {
                        sb.Append("<tr>");
                        sb.Append("<td align ='right'>" + common.myStr(dt.Rows[0]["SignatureLine3"]).Trim() + "</td>");
                        sb.Append("</tr>");
                    }
                    if (common.myStr(dt.Rows[0]["SignatureLine4"]).Trim().Length > 0)
                    {
                        sb.Append("<tr>");
                        sb.Append("<td align ='right'>" + common.myStr(dt.Rows[0]["SignatureLine4"]).Trim() + "</td>");
                        sb.Append("</tr>");
                    }

                }
                sb.Append("</table>");
                sb.Append("<br />");
                sbSign.Append(sb);

            }

            return sbSign;
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            sbSign = new StringBuilder();
            return sbSign;
        }
    }

    private StringBuilder getOtherDoctorInDischargeSummary(bool IsPrintDoctorSignature)
    {
        //yogesh 11/05/2022
        StringBuilder sbSign1 = new StringBuilder(); // for 1
        StringBuilder sbSign2 = new StringBuilder(); // for 2
        StringBuilder sbSign3 = new StringBuilder(); // for 3
        StringBuilder sbSign4 = new StringBuilder(); // for 4 
        StringBuilder sbSign5 = new StringBuilder(); // total string of sign1+sign2
        StringBuilder sbSign6 = new StringBuilder(); // total string of sign3+sign4
        try
        {
            DataSet ds = new DataSet();
            clsIVF objivf = new clsIVF(sConString);
            ds = new DataSet();

            if (IsPrintDoctorSignature)
            {

                #region SignJuniorDoctorID
                if (common.myInt(ViewState["SignJuniorDoctorID"]) > 0)
                {
                    DoctorSignImage(common.myInt(ViewState["SignJuniorDoctorID"]));

                    sbSign1.Append("<td valign='top'>");
                    sbSign1.Append("<table  cellspacing='0' cellpadding='0' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>");
                    if (common.myLen(ViewState["SignJuniorDoctorName"]) > 0)
                    {
                        //sbSign1.Append("<tr><td>" + hdnDoctorImage.Value.ToString() + "</td></tr>");
                        sbSign1.Append("<tr><td><b>" + common.myStr(ViewState["SignJuniorDoctorName"]).Trim() + "</b></td></tr>");
                    }

                    if (common.myStr(ViewState["JuniorSignatureLine1"]).Trim().Length > 0)
                    {
                        sbSign1.Append("<tr><td>" + common.myStr(ViewState["JuniorSignatureLine1"]).Trim() + "</td></tr>");
                    }
                    if (common.myStr(ViewState["JuniorSignatureLine2"]).Trim().Length > 0)
                    {
                        sbSign1.Append("<tr><td>" + common.myStr(ViewState["JuniorSignatureLine2"]).Trim() + "</td></tr>");
                    }
                    if (common.myStr(ViewState["JuniorSignatureLine3"]).Trim().Length > 0)
                    {
                        sbSign1.Append("<tr><td>" + common.myStr(ViewState["JuniorSignatureLine3"]).Trim() + "</td></tr>");
                    }
                    if (common.myStr(ViewState["JuniorSignatureLine4"]).Trim().Length > 0)
                    {
                        sbSign1.Append("<tr><td>" + common.myStr(ViewState["JuniorSignatureLine4"]).Trim() + "</td></tr>");
                    }

                    sbSign1.Append("</table>");
                    sbSign1.Append("</td>");
                }
                #endregion

                #region SignDoctorID
                if (common.myInt(ViewState["SignDoctorID"]) > 0)
                {
                    DoctorSignImage(common.myInt(ViewState["SignDoctorID"]));

                    sbSign2.Append("<td valign='top'>");
                    sbSign2.Append("<table  cellspacing='0' cellpadding='0' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>");
                    if (common.myLen(ViewState["SignDoctorName"]) > 0)
                    {
                        //sbSign2.Append("<tr><td>" + hdnDoctorImage.Value.ToString() + "</td></tr>");                        
                        sbSign2.Append("<tr><td><b>" + common.myStr(ViewState["SignDoctorName"]).Trim() + "(" + ViewState["SpecializationName"] + ")" + "</b></td></tr>");
                    }
                    if (common.myStr(ViewState["SignatureLine1"]).Trim().Length > 0)
                    {
                        sbSign2.Append("<tr><td>" + common.myStr(ViewState["SignatureLine1"]).Trim() + "</td></tr>");
                    }
                    if (common.myStr(ViewState["SignatureLine2"]).Trim().Length > 0)
                    {
                        sbSign2.Append("<tr><td>" + common.myStr(ViewState["SignatureLine2"]).Trim() + "</td></tr>");
                    }
                    if (common.myStr(ViewState["SignatureLine3"]).Trim().Length > 0)
                    {
                        sbSign2.Append("<tr><td>" + common.myStr(ViewState["SignatureLine3"]).Trim() + "</td></tr>");
                    }
                    if (common.myStr(ViewState["SignatureLine4"]).Trim().Length > 0)
                    {
                        sbSign2.Append("<tr><td>" + common.myStr(ViewState["SignatureLine4"]).Trim() + "</td></tr>");
                    }

                    sbSign2.Append("</table>");
                    sbSign2.Append("</td>");
                }
                #endregion

                //yogesh 11/05/2022
                if (!sbSign1.ToString().Equals(string.Empty)
                 || !sbSign2.ToString().Equals(string.Empty))
                {
                    sbSign5.Append("<table cellspacing='0' cellpadding='0'>");

                    if (!sbSign1.ToString().Equals(string.Empty) || !sbSign2.ToString().Equals(string.Empty))
                    {
                        sbSign5.Append("<tr>");
                        sbSign5.Append(sbSign1.ToString());
                        sbSign5.Append("</br>");
                        sbSign5.Append(sbSign2.ToString() + "</br>");
                        sbSign5.Append("</tr>");
                    }

                    sbSign5.Append("</table>");
                }
                //yogesh 11/05/2022
                #region SignThirdDoctorID
                if (common.myInt(ViewState["SignThirdDoctorID"]) > 0)
                {
                    DoctorSignImage(common.myInt(ViewState["SignThirdDoctorID"]));
                    sbSign3.Append("<td valign='top'>");
                    sbSign3.Append("<table  cellspacing='0' cellpadding='0' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>");
                    if (common.myLen(ViewState["SignThirdDoctorName"]) > 0)
                    {
                        sbSign3.Append("<tr><td><b>" + common.myStr(ViewState["SignThirdDoctorName"]).Trim() + "</b></td></tr>");
                    }
                    if (common.myStr(ViewState["ThirdDoctorSignatureLine1"]).Trim().Length > 0)
                    {
                        sbSign3.Append("<tr><td>" + common.myStr(ViewState["ThirdDoctorSignatureLine1"]).Trim() + "</td></tr>");
                    }
                    if (common.myStr(ViewState["ThirdDoctorSignatureLine2"]).Trim().Length > 0)
                    {
                        sbSign3.Append("<tr><td>" + common.myStr(ViewState["ThirdDoctorSignatureLine2"]).Trim() + "</td></tr>");
                    }
                    if (common.myStr(ViewState["ThirdDoctorSignatureLine3"]).Trim().Length > 0)
                    {
                        sbSign3.Append("<tr><td>" + common.myStr(ViewState["ThirdDoctorSignatureLine3"]).Trim() + "</td></tr>");
                    }
                    if (common.myStr(ViewState["ThirdDoctorSignatureLine4"]).Trim().Length > 0)
                    {
                        sbSign3.Append("<tr><td>" + common.myStr(ViewState["ThirdDoctorSignatureLine4"]).Trim() + "</td></tr>");
                    }

                    sbSign3.Append("</table>");
                    sbSign3.Append("</td>");
                }
                #endregion

                #region SignFourthDoctorID
                if (common.myInt(ViewState["SignFourthDoctorID"]) > 0)
                {
                    DoctorSignImage(common.myInt(ViewState["SignFourthDoctorID"]));

                    sbSign4.Append("<td valign='top'>");
                    sbSign4.Append("<table  cellspacing='0' cellpadding='0' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>");
                    if (common.myLen(ViewState["SignFourthDoctorName"]) > 0)
                    {
                        //sbSign4.Append("<tr><td>" + hdnDoctorImage.Value.ToString() + "</td></tr>");                        
                        sbSign4.Append("<tr><td><b>" + common.myStr(ViewState["SignFourthDoctorName"]).Trim() + "</b></td></tr>");
                    }
                    if (common.myStr(ViewState["FourthDoctorSignatureLine1"]).Trim().Length > 0)
                    {
                        sbSign4.Append("<tr><td>" + common.myStr(ViewState["FourthDoctorSignatureLine1"]).Trim() + "</td></tr>");
                    }
                    if (common.myStr(ViewState["FourthDoctorSignatureLine2"]).Trim().Length > 0)
                    {
                        sbSign4.Append("<tr><td>" + common.myStr(ViewState["FourthDoctorSignatureLine2"]).Trim() + "</td></tr>");
                    }
                    if (common.myStr(ViewState["FourthDoctorSignatureLine3"]).Trim().Length > 0)
                    {
                        sbSign4.Append("<tr><td>" + common.myStr(ViewState["FourthDoctorSignatureLine3"]).Trim() + "</td></tr>");
                    }
                    if (common.myStr(ViewState["FourthDoctorSignatureLine4"]).Trim().Length > 0)
                    {
                        sbSign4.Append("<tr><td>" + common.myStr(ViewState["FourthDoctorSignatureLine4"]).Trim() + "</td></tr>");
                    }

                    sbSign4.Append("</table>");
                    sbSign4.Append("</td>");
                }
                #endregion
            }


            //yogesh 11/05/2022
            if (!sbSign3.ToString().Equals(string.Empty)
                    || !sbSign4.ToString().Equals(string.Empty))
            {
                sbSign6.Append("<table cellspacing='0' cellpadding='0'>");

                if (!sbSign3.ToString().Equals(string.Empty) || !sbSign4.ToString().Equals(string.Empty))
                {
                    sbSign6.Append("<tr>");
                    sbSign6.Append(sbSign3.ToString());
                    sbSign6.Append("</br>");
                    sbSign6.Append(sbSign4.ToString() + "</br>");
                    sbSign6.Append("</tr>");
                }

                sbSign6.Append("</table>");
            }

            return sbSign5.Append(sbSign6);
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            sbSign5 = new StringBuilder();
            sbSign6 = new StringBuilder();
            return sbSign5.Append(sbSign6);
        }
    }

    //private StringBuilder getReportHeader(int ReportId)
    //{
    //    StringBuilder sb = new StringBuilder();
    //    try
    //    {

    //        DataSet ds = new DataSet();

    //        bool IsPrintHospitalHeader = false;
    //        clsIVF objivf = new clsIVF(sConString);
    //        ds = objivf.EditReportName(ReportId);

    //        ViewState["IsPrintDoctorSignature"] = common.myBool(ds.Tables[0].Rows[0]["IsPrintDoctorSignature"]);

    //        if (ds.Tables.Count > 0)
    //        {
    //            if (ds.Tables[0].Rows.Count > 0)
    //            {
    //                IsPrintHospitalHeader = common.myBool(ds.Tables[0].Rows[0]["IsPrintHospitalHeader"]);
    //            }
    //        }

    //        ds = new DataSet();
    //        ds = objivf.getFacility(common.myInt(Session["FacilityId"]), common.myInt(Session["HospitalLocationID"]));

    //        sb.Append("<div>");

    //        if (IsPrintHospitalHeader)
    //        {
    //            if (ds.Tables[0].Rows.Count > 0)
    //            {
    //                sb.Append("<table border='0' width='100%' cellpadding='0' cellspacing='0' style='font-size:small'>");
    //                for (int idx = 0; idx < ds.Tables[0].Rows.Count; idx++)
    //                {
    //                    DataRow DR = ds.Tables[0].Rows[idx];

    //                    sb.Append("<tr>");

    //                    sb.Append("<td align ='center'>");
    //                    sb.Append("<table border='0' cellpadding='0' cellspacing='0'>");
    //                    sb.Append("<tr>");
    //                    sb.Append("<td></td><td></td><td></td><td></td><td></td>");
    //                    sb.Append("</tr>");
    //                    sb.Append("<tr>");
    //                    //sb.Append("<td colspan='2' align ='right' valign='middle' style='font-size:9px'><img src='../Icons/SmallLogo.jpg' border='0' width='30px' height='25px'  alt='Image'/></td>");
    //                    sb.Append("<td colspan='2' align ='right' valign='middle' style='font-size:9px'><img src='" + Server.MapPath("/Icons/SmallLogo.jpg") + "' border='0' width='30px' height='25px'  alt='Image'/></td>");
    //                    sb.Append("<td colspan='3' align ='left' valign='middle' style='font-size:9px'><b>" + common.myStr(DR["FacilityName"]).Trim() + "</b></td>");
    //                    sb.Append("</tr>");
    //                    sb.Append("</table>");
    //                    sb.Append("</td>");

    //                    sb.Append("</tr>");

    //                    sb.Append("<tr>");
    //                    sb.Append("<td align ='center'  style='font-size:9px'>" + common.myStr(DR["Address1"]).Trim() + ", " + common.myStr(DR["Address2"]).Trim() + "</td>");
    //                    sb.Append("</tr>");

    //                    //sb.Append("<tr>");
    //                    //sb.Append("<td align ='center'>" + common.myStr(DR["CityName"]) + "(" + common.myStr(DR["PinNo"]) + "), " + common.myStr(DR["StateName"]) + "</td>");
    //                    //sb.Append("</tr>");

    //                    sb.Append("<tr>");
    //                    sb.Append("<td align ='center'  style='font-size:9px'>Phone : " + common.myStr(DR["Phone"]) + " Fax : " + common.myStr(DR["Fax"]) + "</td>");
    //                    sb.Append("</tr>");
    //                }
    //                sb.Append("</table>");
    //            }
    //        }
    //        else
    //        {
    //            //sb.Append("<br />");
    //            //sb.Append("<br />");
    //            //sb.Append("<br />");
    //        }

    //        // sb.Append("<br />");
    //        sb.Append("<table border='0' width='100%' style='text-align:center;'  cellpadding='2' cellspacing='3' ><tr>");
    //        //sb.Append("<td align=center><U>" + common.myStr(ddlReport.SelectedItem.Text) + "</U></td>");
    //        sb.Append("<td align=center><U>" + common.myStr(ViewState["reportname"]) + "</U></td>");
    //        sb.Append("</tr></table></div>");

    //        return sb;
    //    }

    //    catch (Exception Ex)
    //    {
    //        clsExceptionLog objException = new clsExceptionLog();
    //        sb = new StringBuilder();
    //        return sb;
    //    }
    //}
    private void ImageAlert()
    {

    }

    private StringBuilder getReportHeader(int ReportId)
    {
        string ShowDischargeSummaryNABHLogoImage = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]),
        common.myInt(Session["FacilityId"]), "ShowDischargeSummaryNABHLogoImage", sConString);
        DataSet ds = new DataSet();

        bool IsPrintHospitalHeader = false;
        clsIVF objivf = new clsIVF(sConString);
        ds = objivf.EditReportName(ReportId);

        ViewState["IsPrintDoctorSignature"] = common.myBool(ds.Tables[0].Rows[0]["IsPrintDoctorSignature"]);

        if (ds.Tables.Count > 0)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                IsPrintHospitalHeader = common.myBool(ds.Tables[0].Rows[0]["IsPrintHospitalHeader"]);
            }
        }
        DataView dvDoctorSignature = new DataView(ds.Tables[1]);
        dvDoctorSignature.RowFilter = "ImageCode='DS'";
        if (dvDoctorSignature.ToTable().Rows.Count > 0)
        {
            ViewState["SignDoctorHeight"] = common.myStr(dvDoctorSignature.ToTable().Rows[0]["Height"]);
            ViewState["SignDoctorWidth"] = common.myStr(dvDoctorSignature.ToTable().Rows[0]["Width"]);
        }
        //2 Hospita lLogo
        DataView dvHospitalLogo = new DataView(ds.Tables[1]);
        dvHospitalLogo.RowFilter = "ImageCode='HOSL'";
        if (dvHospitalLogo.ToTable().Rows.Count > 0)
        {
            ViewState["HospitalLogoHeight"] = common.myStr(dvHospitalLogo.ToTable().Rows[0]["Height"]);
            ViewState["HospitalLogoWidth"] = common.myStr(dvHospitalLogo.ToTable().Rows[0]["Width"]);
        }
        //3 NABH LOGO
        DataView dvNABHLOGO = new DataView(ds.Tables[1]);
        dvNABHLOGO.RowFilter = "ImageCode='NABH'";
        if (dvNABHLOGO.ToTable().Rows.Count > 0)
        {
            ViewState["NABHLOGOHeight"] = common.myStr(dvNABHLOGO.ToTable().Rows[0]["Height"]);
            ViewState["NABHLOGOWidth"] = common.myStr(dvNABHLOGO.ToTable().Rows[0]["Width"]);
        }
        ds = new DataSet();
        if (common.myStr(Session["FacilityName"]).ToUpper().Contains("PRACHI"))
        {
            ds = objivf.getFacility(common.myInt(Session["FacilityId"]), common.myInt(Session["HospitalLocationID"]), true);
        }
        else
        {
            ds = objivf.getFacility(common.myInt(Session["FacilityId"]), common.myInt(Session["HospitalLocationID"]));
        }


        //Change Palendra
        if (common.myBool(ViewState["ShowPrintHeaderImage"]).Equals(true))
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<table border='0' width='100%' style='margin-top:-30px' cellpadding='0' cellspacing='0' style='font-size:small'>");
            sb.Append("<tr>");
            sb.Append("<td align=right>");
            sb.Append("<img src='" + Server.MapPath("" + ViewState["PrintHeaderImagePath"] + "") + "' border='0' width='480px' height='80px'  alt='Image'/>");
            //sb.Append("<img src='" + Server.MapPath("~") + FileNameLogoImagePath + "' border='0' width='105px' height='105px'  alt='Image'/>");
            sb.Append("</td></tr>");
            sb.Append("<tr>");
            sb.Append("<td align=right>" + ViewState["PrintVersionCode"] + "</td>");
            sb.Append("</tr></table>");
            sb.Append("<table border='0' width='100%' style='text-align:center;'  cellpadding='2' cellspacing='3' ><tr>");
            //sb.Append("<td align=center><U>" + common.myStr(ddlReport.SelectedItem.Text) + "</U></td>");
            sb.Append("<td align=center><U>" + common.myStr(ViewState["reportname"]) + "</U></td>");
            sb.Append("</tr></table>");
            return sb;
        }
        if (ShowDischargeSummaryNABHLogoImage.Equals("Y"))
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                sb.Append("<div>");

                if (IsPrintHospitalHeader)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {

                        //SignImage = "<img width='145px' height='66px' src='" + Server.MapPath("~") + FileName + "' />";
                        //strSingImagePath = Server.MapPath("~") + FileName;
                        string FileNameLogoImagePath = common.myStr(ds.Tables[0].Rows[0]["LogoImagePath"]);
                        string FileNameNABHLogoImagePath = common.myStr(ds.Tables[0].Rows[0]["NABHLogoImagePath"]);
                        sb.Append("<table border='0' width='100%' cellpadding='0' cellspacing='0' style='font-size:small'>");
                        sb.Append("<tr>");
                        sb.Append("<td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td colspan='2'>");
                        //sb.Append("<img src='" + Server.MapPath("/Icons/SmallLogo.jpg") + "' border='0' width='105px' height='105px'  alt='Image'/>");
                        sb.Append("<img src='" + Server.MapPath("~") + FileNameLogoImagePath + "' border='0' width='" + ViewState["HospitalLogoWidth"] + "px' height='" + ViewState["HospitalLogoHeight"] + "px' alt='Image'/>");
                        sb.Append("</td>");

                        sb.Append("<td colspan='6' >");

                        for (int idx = 0; idx < ds.Tables[0].Rows.Count; idx++)
                        {
                            DataRow DR = ds.Tables[0].Rows[idx];

                            sb.Append("<table border='0' cellpadding='0' cellspacing='0' style='font-size:small;margin-left: 10px !important;'>");
                            sb.Append("<tr>");
                            sb.Append("<td  align ='center' style='font-size:16pt' ><b>" + common.myStr(DR["FacilityName"]).Trim() + "</b></td>");
                            sb.Append("</tr>");

                            sb.Append("<tr>");
                            sb.Append("<td align ='center' style='font-size:10pt'>" + common.myStr(DR["Address1"]).Trim() + ", " + common.myStr(DR["Address2"]).Trim() + "</td>");
                            sb.Append("</tr>");

                            sb.Append("<tr>");
                            sb.Append("<td align ='center' style='font-size:10pt'>Mobile : " + common.myStr(DR["Phone"]) + " Phone : " + common.myStr(DR["Fax"]) + "</td>");
                            sb.Append("</tr>");

                            sb.Append("<tr>");
                            sb.Append("<td align ='center' style='font-size:10pt'>E-mail : " + common.myStr(DR["EmailId"]) + " Website : " + common.myStr(DR["WebSite"]) + "</td>");
                            sb.Append("</tr>");
                            sb.Append("</table>");
                        }

                        sb.Append("</td>");
                        sb.Append("<td colspan='2'>");
                        //sb.Append("<img src='" + Server.MapPath("/Icons/SmallLogo.jpg") + "' border='0' width='100px' height='50px'  alt='Image'/>");
                        sb.Append("<img src='" + Server.MapPath("~") + FileNameNABHLogoImagePath + "' border='0'  width='" + ViewState["NABHLOGOWidth"] + "px' height='" + ViewState["NABHLOGOHeight"] + "px' alt='Image'/>");
                        sb.Append("</td>");


                        sb.Append("</tr>");
                        sb.Append("<tr>");
                        sb.Append("<td colspan='10' align=right>" + ViewState["PrintVersionCode"] + "</ td>");
                        sb.Append("</tr>");
                        sb.Append("</table>");
                    }
                }

                sb.Append("<table border='0' width='100%' style='text-align:center;'  cellpadding='2' cellspacing='3' ><tr>");
                sb.Append("<td align=center><U>" + common.myStr(ViewState["reportname"]) + "</U></td>");
                sb.Append("</tr></table></div>");

                return sb;
            }

            catch (Exception Ex)
            {
                return sb;
            }
        }
        else
        {
            StringBuilder sb = new StringBuilder();
            StringBuilder sbSub = new StringBuilder();
            try
            {

                if (IsPrintHospitalHeader)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        sbSub.Append("<table border='0' width='100%' cellpadding='0' cellspacing='0' style='font-size:small'>");
                        sbSub.Append("<tr>");
                        sbSub.Append("<td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td>");
                        sbSub.Append("</tr>");

                        sbSub.Append("<tr>");
                        sbSub.Append("<td colspan='2'>");
                        sbSub.Append("<img src='" + Server.MapPath("/Icons/SmallLogo.jpg") + "' border='0'  width='" + ViewState["HospitalLogoWidth"] + "px' height='" + ViewState["HospitalLogoHeight"] + "px'  alt='Image'/>");
                        sbSub.Append("</td>");

                        sbSub.Append("<td colspan='8'>");

                        for (int idx = 0; idx < ds.Tables[0].Rows.Count; idx++)
                        {
                            DataRow DR = ds.Tables[0].Rows[idx];

                            sbSub.Append("<table border='0' cellpadding='0' cellspacing='0' style='font-size:small'>");
                            sbSub.Append("<tr>");
                            sbSub.Append("<td  align ='left' style='font-size:9pt' ><b>" + common.myStr(DR["FacilityName"]).Trim() + "</b></td>");
                            sbSub.Append("</tr>");

                            sbSub.Append("<tr>");
                            sbSub.Append("<td align ='left' style='font-size:9pt'>" + common.myStr(DR["Address1"]).Trim() + ", " + common.myStr(DR["Address2"]).Trim() + "</td>");
                            sbSub.Append("</tr>");

                            sbSub.Append("<tr>");
                            sbSub.Append("<td align ='left' style='font-size:9pt'>Phone : " + common.myStr(DR["Phone"]) + " Fax : " + common.myStr(DR["Fax"]) + "</td>");
                            sbSub.Append("</tr>");
                            sbSub.Append("</table>");
                        }

                        sbSub.Append("</td>");

                        sbSub.Append("</tr>");
                        sbSub.Append("<tr>");
                        sbSub.Append("<td colspan='10' align=right>" + ViewState["PrintVersionCode"] + "</ td>");
                        sbSub.Append("</tr>");
                        sbSub.Append("</table>");
                    }
                }

                if (sbSub.ToString() != string.Empty || common.myLen(ViewState["reportname"]) > 0)
                {
                    sb.Append("<div>");
                    sb.Append(sbSub.ToString());
                    if (common.myLen(ViewState["reportname"]) > 0)
                    {
                        sb.Append("<table border='0' width='100%' style='text-align:center;'  cellpadding='2' cellspacing='3' ><tr>");
                        sb.Append("<td align='center'><U>" + common.myStr(ViewState["reportname"]) + "</U></td>");
                        sb.Append("</tr></table>");
                    }
                    sb.Append("</div>");
                }

                return sb;
            }

            catch (Exception Ex)
            {
                clsExceptionLog objException = new clsExceptionLog();
                sb = new StringBuilder();
                return sb;
            }
        }

    }

    public StringBuilder ShowAllDepartmentDoctors(int DepartmentId, string ConsultingDoctorDepartment, string EntrySite)
    {
        StringBuilder sbTable = new StringBuilder();
        StringBuilder sbRow = new StringBuilder();
        clsIVF objI = new clsIVF(sConString);
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

        Hashtable hshInput = new Hashtable();
        hshInput.Add("@intDepartmentId", DepartmentId);
        hshInput.Add("@intFacilityId", common.myInt(Session["FacilityId"]));

        if (common.myInt(Request.QueryString["EncId"]) > 0
            && common.myStr(Request.QueryString["For"]).ToUpper().Equals("DISSUM"))
        {
            hshInput.Add("@intEncounterId", common.myInt(Request.QueryString["EncId"]));
        }

        DataSet dsDoctorDepartmentDetails = new DataSet();
        dsDoctorDepartmentDetails = objI.getDoctorDepartmentDetails(common.myInt(Request.QueryString["EncId"]), common.myInt(Session["FacilityId"]));

        if (dsDoctorDepartmentDetails.Tables.Count > 0)
        {
            if (dsDoctorDepartmentDetails.Tables[0].Rows.Count > 0)
            {
                hshInput.Add("@intSpecialisationId", common.myInt(dsDoctorDepartmentDetails.Tables[0].Rows[0]["SpecialisationId"]));
            }
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
                //            sbRow.Append("<td valign='top' align='left'><span style='font-family:" + common.myStr(hdnFontName.Value) + "; font-weight: bold; font-size:9pt;'>" + common.myStr(DR["ShortDoctorName"]).Trim() + "</span>, <span style='font-family:" + common.myStr(hdnFontName.Value) + ";  font-size:8pt;'>" + common.myStr(DR["SignatureLine"]).Trim() + "</span></td>");
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
                            sbRow.Append("<td valign='top' align='left'><span style='font-family:" + common.myStr(hdnFontName.Value) + "; font-weight: bold; font-size:9pt;'>" + common.myStr(tblCol1.Rows[rowIdx]["ShortDoctorName"]).Trim() + "</span>, <span style='font-family:" + common.myStr(hdnFontName.Value) + ";  font-size:8pt;'>" + common.myStr(tblCol1.Rows[rowIdx]["SignatureLine"]).Trim() + "</span></td>");
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
                            sbRow.Append("<td valign='top' align='left'><span style='font-family:" + common.myStr(hdnFontName.Value) + "; font-weight: bold; font-size:9pt;'>" + common.myStr(tblCol2.Rows[rowIdx]["ShortDoctorName"]).Trim() + "</span>, <span style='font-family:" + common.myStr(hdnFontName.Value) + ";  font-size:8pt;'>" + common.myStr(tblCol2.Rows[rowIdx]["SignatureLine"]).Trim() + "</span></td>");
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
        return sbTable;
    }

    public string GetImageText(int Id)
    {
        clsIVF objIVF = new clsIVF(sConString);
        StringBuilder imgsb = new StringBuilder();

        try
        {
            if (Id > 0)
            {
                string FileName = objIVF.GetReportImageUrl(Id);
                if (common.myLen(FileName) > 0)
                {
                    imgsb.Append("</br>");
                    imgsb.Append("</br>");
                    imgsb.Append("<table cellspacing='0' cellpadding='0'>");
                    imgsb.Append("<tr><td><img src='.../ReportImage/" + FileName + "' width='500px'  /></td></tr>");
                    imgsb.Append("</table>");
                    imgsb.Append("</br>");
                }
            }

        }
        catch (Exception)
        {
        }
        finally
        {
        }
        return imgsb.ToString();
    }



}
