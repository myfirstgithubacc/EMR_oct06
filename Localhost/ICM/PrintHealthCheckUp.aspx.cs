using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text;
using System.Web.UI.WebControls.WebParts;

public partial class EMRReports_PrintHealthCheckUp : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    private static String FileName = "";
    private static string strimgData = "";
    private static String Education = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            StringBuilder sbNextHeader = new StringBuilder();
            StringBuilder sb = new StringBuilder();
            string SOURCE = common.myStr(Request.QueryString["SOURCE"]);
            string LABNO = common.myStr(Request.QueryString["LABNO"]);

            BindSummary bnotes = new BindSummary(sConString);
            DataSet ds = bnotes.GetPrintDischargeSummary(common.myInt(Request.QueryString["EncId"]), common.myInt(HttpContext.Current.Session["HospitalLocationId"]),
                                           common.myInt(Request.QueryString["RegId"]), common.myInt(Request.QueryString["ReportId"]),
                                           common.myInt(HttpContext.Current.Session["FacilityId"]));


            // Details of top header and doctors name 
            StringBuilder sbtopheader = new StringBuilder();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ViewState["SignDoctorID"] = common.myStr(ds.Tables[0].Rows[0]["SignDoctorID"]);
                ViewState["PreparedById"] = common.myStr(ds.Tables[0].Rows[0]["PreparedById"]);
                ViewState["PreparedByName"] = common.myStr(ds.Tables[0].Rows[0]["PreparedByName"]);
                ViewState["SignDoctorName"] = common.myStr(ds.Tables[0].Rows[0]["SignDoctorName"]);
                ViewState["signdesg"] = common.myStr(ds.Tables[0].Rows[0]["signdesg"]);
                ViewState["DepartmentName"] = common.myStr(ds.Tables[0].Rows[0]["DepartmentName"]);
                ViewState["Education"] = common.myStr(ds.Tables[0].Rows[0]["Education"]);

                ViewState["SpecializationName"] = common.myStr(ds.Tables[0].Rows[0]["SpecializationName"]);
                ViewState["UPIN"] = common.myStr(ds.Tables[0].Rows[0]["UPIN"]).Trim();

            }
            BaseC.Hospital objHospital = new BaseC.Hospital(sConString);
            DataSet dsHeader = objHospital.GetReportHeader("HC");
            DataTable dtHeaderMerge = dsHeader.Tables[0];

            #region Title header
            StringBuilder sbTitle = new StringBuilder();
            if (ds.Tables[0].Rows.Count > 0)
                sbTitle.Append("<table cellpadding='0' cellspacing='0'><tr ><td valign='top' align='center' style='border-style:none;width:10px;font-size:12pt;font-weight:bold;font-family: courier new;text-decoration: underline;'>" + common.myStr(ds.Tables[0].Rows[0]["HeadingName"]) + "</td></tr></table>");
            else
                sbTitle.Append("<table cellpadding='0' cellspacing='0'><tr ><td valign='top' align='center' style='border-style:none;width:10px;font-size:12pt;font-weight:bold;font-family: courier new;text-decoration: underline;'>Discharge Summary</td></tr></table>");

            #endregion
            #region Header table
            string Fonts = getFontName("2", "11");
            string bold = "", italic = "";

            string Ft = "style='" + Fonts + ";" + bold + italic;

            DAL.DAL DlObj = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            DataSet dsHead = new DataSet();
            Hashtable hsSub = new Hashtable();
            hsSub = new Hashtable();
            hsSub.Add("@intHeaderId", 21);
            hsSub.Add("@chrAppointmentDate", Convert.ToDateTime(DateTime.Now.Date).ToString("yyyy/MM/dd"));
            hsSub.Add("@intEmployeeId", common.myInt(Request.QueryString["DoctorId"]) == 0 ? common.myInt(Session["DoctorId"]) : common.myInt(Request.QueryString["DoctorId"]));
            hsSub.Add("@intRegistrationId", common.myInt(Request.QueryString["RegId"]));
            hsSub.Add("@intFacilityId", common.myInt(Session["FacilityId"]));
            hsSub.Add("@intEncounterId", common.myStr(Request.QueryString["EncId"]));
            dsHead = DlObj.FillDataSet(CommandType.StoredProcedure, "uspGetDataObjectDetails", hsSub);
            if (dsHead.Tables[0].Rows.Count > 0)
            {
                DataRow dr = dsHead.Tables[0].Rows[0];
                if (dsHead.Tables[0].Rows[0]["ShowBorder"].ToString().Trim() == "True")
                {
                    sb.Append("<table width='99%' style='" + Fonts + "; border-collapse:collapse;' border='1' cellpadding='2' cellspacing='2'>");//</font></td></tr>");
                    int cc = 0;
                    for (int r = 1; r <= 8; r++)
                    {
                        DataView dvR = new DataView(dsHead.Tables[0]);
                        dvR.RowFilter = "RowNo=" + r.ToString();
                        DataTable dtR = dvR.ToTable();
                        if (dtR.Rows.Count != 0)
                        {
                            sb.Append("<tr align='left'>");
                            for (int c = 1; c <= dtR.Rows.Count; c++)
                            {
                                if (!Convert.ToBoolean(dsHead.Tables[0].Rows[cc]["IsBinary"]))
                                {
                                    if (dsHead.Tables[0].Rows[cc]["FieldCaption"].ToString() != "")
                                    {
                                        sb.Append("<td width=10% align='left' valign='top' style='border-style:none;width:10px;font-size:10pt; font-weight:bold;'>" + dsHead.Tables[0].Rows[cc]["FieldCaption"].ToString() + " : " + dsHead.Tables[0].Rows[cc]["Value"].ToString() + "</td>");
                                    }
                                    else
                                    {
                                        sb.Append("<td width=20% align='left' style='border-style:none;width:10px;font-size:10pt; font-weight:bold;'>:" + dsHead.Tables[0].Rows[cc]["Value"].ToString() + "</td>");
                                    }
                                }
                                else
                                {
                                    if (ds.Tables[0].Rows[cc]["FieldCaption"].ToString() != "")
                                    {

                                        sb.Append("<td width=10% align='left' valign='top' style='border-style:none;width:10px;font-size:10pt; font-weight:bold;' ><u>" + dsHead.Tables[0].Rows[cc]["FieldCaption"].ToString() + " : " + "<td valign='top' ><img src='/PatientDocuments/PatientImages/BioChem.bmp' border='0'></img></td>" + "</u></td>");
                                    }
                                    else
                                    {

                                        sb.Append("<td width=20% align='left' style='border-style:none;width:10px;font-size:10pt; font-weight:bold;'><u>" + "<td valign='top' ><img src='/PatientDocuments/PatientImages/BioChem.bmp' border='0'></img></td>" + "</u></td>");
                                    }
                                }
                                cc++;

                            }

                            sb.Append("</tr>");

                            if (r == 1)
                            {
                                //Next Header
                                sbNextHeader = new StringBuilder();
                                sbNextHeader.Append(sb.ToString());
                                sbNextHeader.Append("</table>");
                            }
                        }
                    }
                    sb.Append("</table>");
                }
                else
                {
                    sb.Append("<table width='99%' style='" + Fonts + "; border-collapse:collapse;' border='0' cellpadding='5' cellspacing='1'>");//</font></td></tr>");

                    int cc = 0;
                    for (int r = 1; r <= 8; r++)
                    {
                        DataView dvR = new DataView(dsHead.Tables[0]);

                        dvR.RowFilter = "RowNo=" + r.ToString();
                        DataTable dtR = dvR.ToTable();

                        if (dtR.Rows.Count != 0)
                        {
                            sb.Append("<tr align='left'>");

                            for (int c = 1; c <= dtR.Rows.Count; c++)
                            {
                                if (!Convert.ToBoolean(dsHead.Tables[0].Rows[cc]["IsBinary"]))
                                {
                                    sb.Append("<td width=10% align='left' valign='top' style='border-style:none;width:10px;font-size:10pt; font-weight:bold;'>" + dsHead.Tables[0].Rows[cc]["FieldCaption"].ToString() + " - " + dsHead.Tables[0].Rows[cc]["Value"].ToString() + "</u></td>");
                                }
                                else
                                {
                                    sb.Append("<td width=20% align='left' style='border-style:none;width:10px;font-size:10pt; font-weight:bold;'>:" + dsHead.Tables[0].Rows[cc]["FieldCaption"].ToString() + " - " + "<td valign='top' ><img src='/PatientDocuments/PatientImages/BioChem.bmp' border='0'></img></td>" + "</u></td>");
                                }
                                cc++;
                            }
                            sb.Append("</tr>");
                        }
                    }
                    sb.Append("</table><br/><br/>");

                }
            }
            #endregion
            #region Header Section of Report
            //if (dsHeader.Tables.Count > 0)
            //{
            //    int RowNo = 0;

            //    if (dsHeader.Tables[0].Rows.Count > 0)
            //    {
            //        sb.Append("<table border=1 cellpadding=5 cellspacing=1><tr><td>");
            //        sb.Append("<table border=0 cellpadding=0 cellspacing=0>");
            //        int ColumnCount = common.myInt(dsHeader.Tables[0].Compute("MAX(ColNo)", string.Empty));
            //        for (int i = 0; i < dsHeader.Tables[0].Rows.Count; i++)
            //        {

            //            if (RowNo == 0)
            //            {
            //                RowNo = common.myInt(dsHeader.Tables[0].Rows[i]["Rowno"]);
            //                sb.Append("<tr align='left'>");
            //                if (ColumnCount == 1)
            //                {

            //                    //sb.Append("<td align='left'>" + common.myStr(dsHeader.Tables[0].Rows[i]["FieldCaption"]) + "</td>");
            //                    //sb.Append("<td align='left'></td></tr>");
            //                    dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=1 AND Rowno=" + RowNo;
            //                    if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
            //                    {
            //                        sb.Append("<td width=10% align='left' valign='top' style='border-style:none;width:10px;font-size:8pt;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</td>");
            //                        sb.Append("<td width=20% align='left' style='border-style:none;width:10px;font-size:8pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["Value"]), ds) + "</td>");
            //                    }
            //                    else
            //                    {
            //                        sb.Append("<td width=10% align='left' valign='top' style='border-style:none;width:10px;font-size:8pt;'></td>");
            //                        sb.Append("<td width=20% align='left' style='border-style:none;width:10px;font-size:8pt;'>: </td>");
            //                    }
            //                }

            //                if (ColumnCount == 2)
            //                {
            //                    //i = +1;
            //                    //sb.Append("<td align='left'>" + common.myStr(dsHeader.Tables[0].Rows[i]["FieldCaption"]) + "</td>");
            //                    //sb.Append("<td align='left'>dsadas</td>");
            //                    //sb.Append("<td align='left'>" + common.myStr(dsHeader.Tables[0].Rows[i]["FieldCaption"]) + "</td>");
            //                    //sb.Append("<td align='left'>Male</td></tr>");
            //                    i += 1;
            //                    dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=1 AND Rowno=" + RowNo;
            //                    if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
            //                    {
            //                        sb.Append("<td width=10% valign='top' align='left' style='border-style:none;width:10px;font-size:8pt;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</td>");
            //                        sb.Append("<td width=20% align='left' style='border-style:none;width:10px;font-size:8pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["Value"]), ds) + "</td>");
            //                    }
            //                    else
            //                    {
            //                        sb.Append("<td width=10% valign='top' align='left' style='border-style:none;width:10px;font-size:8pt;'></td>");
            //                        sb.Append("<td width=20% align='left' style='border-style:none;width:10px;font-size:8pt;'>: </td>");
            //                    }
            //                    dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=2 AND Rowno=" + RowNo;
            //                    if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
            //                    {
            //                        sb.Append("<td align='left' valign='top' style='border-style:none;width:10px;font-size:8pt;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</td>");
            //                        sb.Append("<td align='left' style='border-style:none;width:10px;font-size:8pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["Value"]), ds) + "</td>");
            //                    }
            //                    else
            //                    {
            //                        sb.Append("<td align='left' style='border-style:none;width:10px;font-size:8pt;'></td>");
            //                        sb.Append("<td align='left' style='border-style:none;width:10px;font-size:8pt;'>:</td>");
            //                    }

            //                }
            //                if (ColumnCount == 3)
            //                {
            //                    i += 2;
            //                    dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=1 AND Rowno=" + RowNo;
            //                    if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
            //                    {
            //                        sb.Append("<td width=10% valign='top' align='left' style='border-style:none;width:10px;font-size:8pt;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</td>");
            //                        dtHeaderMerge.DefaultView.RowFilter = "ColNo=2 AND Rowno=" + RowNo;
            //                        if (dtHeaderMerge.DefaultView.ToTable().Rows.Count > 0)// this for merge td
            //                        {
            //                            dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=1 AND Rowno=" + RowNo;
            //                            sb.Append("<td width=20% align='left' style='border-style:none;width:10px;font-size:8pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td>");
            //                        }
            //                        else
            //                        {
            //                            dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=1 AND Rowno=" + RowNo;
            //                            sb.Append("<td  colspan='3' align='left' style='border-style:none;width:10px;font-size:8pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td>");
            //                        }
            //                    }
            //                    else
            //                    {
            //                        sb.Append("<td width=10% valign='top' align='left' style='border-style:none;width:10px;font-size:8pt;'></td>");
            //                        sb.Append("<td width=20% align='left' style='border-style:none;width:10px;font-size:8pt;'>: </td>");
            //                    }
            //                    dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=2 AND Rowno=" + RowNo;
            //                    if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
            //                    {
            //                        sb.Append("<td align='left' valign='top' style='border-style:none;width:10px;font-size:8pt;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</td>");
            //                        sb.Append("<td align='left' style='border-style:none;width:10px;font-size:8pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td>");
            //                    }
            //                    //else
            //                    //{
            //                    //    sb.Append("<td align='left' style='border-style:none;width:10px;font-size:8pt;'></td>");
            //                    //    sb.Append("<td align='left' style='border-style:none;width:10px;font-size:8pt;'></td>");
            //                    //}
            //                    dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=3 AND Rowno=" + RowNo;
            //                    if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
            //                    {
            //                        sb.Append("<td align='left' valign='top' style='border-style:none;width:10px;font-size:8pt;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</td>");
            //                        sb.Append("<td align='left' style='border-style:none;width:10px;font-size:8pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td></tr>");
            //                    }
            //                    else
            //                    {
            //                        sb.Append("<td align='left' style='border-style:none;width:10px;font-size:8pt;'></td>");
            //                        sb.Append("<td align='left' style='border-style:none;width:10px;font-size:8pt;'>: </td>");
            //                    }
            //                }
            //            }
            //            else if (RowNo == common.myInt(dsHeader.Tables[0].Rows[i]["Rowno"]))
            //            {
            //                sb.Append("<tr align='left'>");
            //                if (ColumnCount == 1)
            //                {
            //                    dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=1 AND Rowno=" + RowNo;
            //                    if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
            //                    {
            //                        sb.Append("<td width=10% valign='top' align='left' style='border-style:none;width:10px;font-size:8pt;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</td>");
            //                        sb.Append("<td width=20% align='left' style='border-style:none;width:10px;font-size:8pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td>");
            //                    }
            //                    else
            //                    {
            //                        sb.Append("<td width=10% valign='top' align='left' style='border-style:none;width:10px;font-size:8pt;'></td>");
            //                        sb.Append("<td width=20% align='left' style='border-style:none;width:10px;font-size:8pt;'>: </td>");
            //                    }

            //                }
            //                if (ColumnCount == 2)
            //                {

            //                    i += 1;
            //                    dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=1 AND Rowno=" + RowNo;
            //                    if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
            //                    {
            //                        sb.Append("<td width=10% valign='top' align='left' style='border-style:none;width:10px;font-size:8pt;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</td>");
            //                        sb.Append("<td width=20% align='left' style='border-style:none;width:10px;font-size:8pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td>");
            //                    }
            //                    else
            //                    {
            //                        sb.Append("<td width=10% valign='top' align='left' style='border-style:none;width:10px;font-size:8pt;'></td>");
            //                        sb.Append("<td width=20% align='left' style='border-style:none;width:10px;font-size:8pt;'>: </td>");
            //                    }
            //                    dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=2 AND Rowno=" + RowNo;
            //                    if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
            //                    {
            //                        sb.Append("<td align='left' valign='top' style='border-style:none;width:10px;font-size:8pt;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</td>");
            //                        sb.Append("<td align='left' style='border-style:none;width:10px;font-size:8pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td>");
            //                    }
            //                    else
            //                    {
            //                        sb.Append("<td align='left' valign='top' style='border-style:none;width:10px;font-size:8pt;'></td>");
            //                        sb.Append("<td align='left' style='border-style:none;width:10px;font-size:8pt;'>: </td>");
            //                    }

            //                }
            //                if (ColumnCount == 3)
            //                {
            //                    i += 2;

            //                    dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=1 AND Rowno=" + RowNo;
            //                    if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
            //                    {
            //                        sb.Append("<td width=10% valign='top' align='left' style='border-style:none;width:10px;font-size:8pt;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</td>");

            //                        dtHeaderMerge.DefaultView.RowFilter = "ColNo=2 AND Rowno=" + RowNo;
            //                        if (dtHeaderMerge.DefaultView.ToTable().Rows.Count > 0)// this for merge td
            //                        {
            //                            dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=1 AND Rowno=" + RowNo;
            //                            sb.Append("<td width=20% align='left' style='border-style:none;width:10px;font-size:8pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td>");
            //                        }
            //                        else
            //                        {
            //                            dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=1 AND Rowno=" + RowNo;
            //                            sb.Append("<td colspan='3' align='left' style='border-style:none;width:10px;font-size:8pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td>");
            //                            // sb.Append("<td  colspan='3' align='left' style='border-style:none;width:10px;font-size:8pt;'></td>");
            //                        }
            //                    }
            //                    else
            //                    {
            //                        sb.Append("<td width=10% valign='top' align='left' style='border-style:none;width:10px;font-size:8pt;'></td>");
            //                        sb.Append("<td width=20% align='left' style='border-style:none;width:10px;font-size:8pt;'>: </td>");
            //                    }
            //                    dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=2 AND Rowno=" + RowNo;
            //                    if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
            //                    {
            //                        sb.Append("<td align='left' valign='top' style='border-style:none;width:10px;font-size:8pt;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</td>");
            //                        sb.Append("<td align='left'  valign='top' style='border-style:none;width:10px;font-size:8pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td>");

            //                    }
            //                    //else
            //                    //{
            //                    //    sb.Append("<td align='left' style='border-style:none;width:10px;font-size:8pt;'></td>");
            //                    //    sb.Append("<td align='left' style='border-style:none;width:10px;font-size:8pt;'></td>");
            //                    //}
            //                    dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=3 AND Rowno=" + RowNo;
            //                    if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
            //                    {
            //                        sb.Append("<td align='left' valign='top' style='border-style:none;width:10px;font-size:8pt;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</td>");
            //                        sb.Append("<td align='left' valign='top' style='border-style:none;width:10px;font-size:8pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td></tr>");
            //                    }
            //                    else
            //                    {
            //                        sb.Append("<td align='left' valign='top' style='border-style:none;width:10px;font-size:8pt;'></td>");
            //                        sb.Append("<td align='left' style='border-style:none;width:10px;font-size:8pt;'>: </td>");
            //                    }
            //                }
            //            }
            //            else
            //            {
            //                sb.Append("<tr align='left'>");
            //                if (ColumnCount == 1)
            //                {
            //                    //sb.Append("<td align='left'>" + common.myStr(dr["FieldCaption"]) + "</td>");
            //                    //sb.Append("<td align='left'></td></tr>");
            //                    dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=1 AND Rowno=" + RowNo;
            //                    if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
            //                    {
            //                        sb.Append("<td width=10% valign='top' align='left' style='border-style:none;width:10px;font-size:8pt;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</td>");
            //                        sb.Append("<td width=20% align='left' style='border-style:none;width:10px;font-size:8pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td>");
            //                    }
            //                    else
            //                    {
            //                        sb.Append("<td width=10% valign='top' align='left' style='border-style:none;width:10px;font-size:8pt;'></td>");
            //                        sb.Append("<td width=20% align='left' style='border-style:none;width:10px;font-size:8pt;'>: </td>");
            //                    }

            //                }
            //                if (ColumnCount == 2)
            //                {
            //                    //i = +1;
            //                    //sb.Append("<td align='left'>" + common.myStr(dr["FieldCaption"]) + "</td>");
            //                    //sb.Append("<td align='left'>dsadas</td>");
            //                    //sb.Append("<td align='left'>" + common.myStr(dr["FieldCaption"]) + "</td>");
            //                    //sb.Append("<td align='left'>Male</td></tr>");
            //                    i += 1;
            //                    dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=1 AND Rowno=" + RowNo;
            //                    if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
            //                    {
            //                        sb.Append("<td width=10% valign='top' align='left' style='border-style:none;width:10px;font-size:8pt;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</td>");
            //                        sb.Append("<td width=20% align='left' style='border-style:none;width:10px;font-size:8pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td>");
            //                    }
            //                    else
            //                    {
            //                        sb.Append("<td width=10% valign='top' align='left' style='border-style:none;width:10px;font-size:8pt;'></td>");
            //                        sb.Append("<td width=20% align='left' style='border-style:none;width:10px;font-size:8pt;'>: </td>");
            //                    }
            //                    dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=2 AND Rowno=" + RowNo;
            //                    if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
            //                    {
            //                        sb.Append("<td align='left' valign='top' style='border-style:none;width:10px;font-size:8pt;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</td>");
            //                        sb.Append("<td align='left' style='border-style:none;width:10px;font-size:8pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td>");
            //                    }
            //                    else
            //                    {
            //                        sb.Append("<td align='left' valign='top' style='border-style:none;width:10px;font-size:8pt;'></td>");
            //                        sb.Append("<td align='left' style='border-style:none;width:10px;font-size:8pt;'>: </td>");
            //                    }

            //                }
            //                if (ColumnCount == 3)
            //                {
            //                    i += 2;
            //                    dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=1 AND Rowno=" + RowNo;
            //                    if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
            //                    {
            //                        sb.Append("<td width=10% valign='top' align='left' style='border-style:none;width:10px;font-size:8pt;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</td>");
            //                        dtHeaderMerge.DefaultView.RowFilter = "ColNo=2 AND Rowno=" + RowNo;
            //                        if (dtHeaderMerge.DefaultView.ToTable().Rows.Count > 0)// this for merge td
            //                        {
            //                            dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=1 AND Rowno=" + RowNo;
            //                            sb.Append("<td width=20% align='left' style='border-style:none;width:10px;font-size:8pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td>");
            //                        }
            //                        else
            //                        {
            //                            dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=1 AND Rowno=" + RowNo;
            //                            sb.Append("<td  colspan='3' align='left' style='border-style:none;width:10px;font-size:8pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td>");
            //                        }
            //                    }
            //                    else
            //                    {
            //                        sb.Append("<td width=10% valign='top' align='left' style='border-style:none;width:10px;font-size:8pt;'></td>");
            //                        sb.Append("<td width=20% align='left' style='border-style:none;width:10px;font-size:8pt;'>: </td>");
            //                    }
            //                    dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=2 AND Rowno=" + RowNo;
            //                    if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
            //                    {
            //                        sb.Append("<td align='left' valign='top' style='border-style:none;width:10px;font-size:8pt;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</td>");
            //                        sb.Append("<td align='left' style='border-style:none;width:10px;font-size:8pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td>");
            //                    }
            //                    //else
            //                    //{
            //                    //    sb.Append("<td align='left' style='border-style:none;width:10px;font-size:8pt;'></td>");
            //                    //    sb.Append("<td align='left' style='border-style:none;width:10px;font-size:8pt;'></td>");
            //                    //}
            //                    dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=3 AND Rowno=" + RowNo;
            //                    if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
            //                    {
            //                        sb.Append("<td align='left' valign='top' style='border-style:none;width:10px;font-size:8pt;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</td>");
            //                        sb.Append("<td align='left' valign='top' style='border-style:none;width:10px;font-size:8pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td></tr>");
            //                    }
            //                    else
            //                    {
            //                        sb.Append("<td align='left' valign='top' style='border-style:none;width:10px;font-size:8pt;'></td>");
            //                        sb.Append("<td align='left' style='border-style:none;width:10px;font-size:8pt;'>: </td>");
            //                    }
            //                }

            //            }
            //            //if (i+1 != common.myInt(dsHeader.Tables[0].Rows.Count))
            //            if (i + 1 < common.myInt(dsHeader.Tables[0].Rows.Count))
            //            {
            //                RowNo = common.myInt(dsHeader.Tables[0].Rows[i + 1]["Rowno"]);
            //            }
            //        }
            //        sb.Append("</table>");
            //        sb.Append("</tr></td></table>");
            //    }
            //}


            #endregion

            //facilityImage();

            //StringBuilder sbLogo = new StringBuilder();
            //sbLogo.Append("<table width='96%' border='0' cellpadding='0' cellspacing='0' style='font-size:10pt;font-family:Courier New;'>");
            //sbLogo.Append("<tr><td align='right'>" + hdnFacilityImage.Value + "</td></tr>");
            //sbLogo.Append("</table>");
            //sbLogo.Append("<br />");

            // sbtopheader.Append(sbLogo.ToString());


            StringBuilder sb1 = new StringBuilder(); // Blank Table careate for below header  
            sb1.Append("<table width='96%' border='0' cellpadding='0' cellspacing='0' style='font-size:10pt;font-family:Courier New;'>");
            sb1.Append("<tr><td align='right'>");
            sb1.Append("<br />");
            sb1.Append("</td></tr>");
            sb1.Append("</table>");




            sbtopheader.Append(sb);

            sbtopheader.Append(sbTitle);
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

            if (ds.Tables[0].Rows.Count > 0)
            {
                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                string EmpNo = common.myStr(objDl.ExecuteScalar(CommandType.Text, "select e.EmployeeNo from Employee e inner join Users u on e.ID=u.EmpID where  u.id=" + common.myInt(Session["UserId"])));

                StringBuilder sbPatientSummary = new StringBuilder();

                sbPatientSummary.Append(common.myStr(ds.Tables[0].Rows[0]["PatientSummary"]) + GetSignatureDetails());

                sbPatientSummary = sbPatientSummary.Replace("font-weight: 700; font-family: courier new;", "font-size:10pt; font-family: Courier New; font-weight:bold;");
                sbPatientSummary = sbPatientSummary.Replace("font-weight: 700;", "font-size:10pt;");

                StringBuilder sbPS = new StringBuilder();
                sbPS.Append(sbPatientSummary);

                sbPatientSummary = new StringBuilder();

                sbPatientSummary = sbPatientSummary.Append("<span style='font-size:10pt; font-family: Courier New; '>" + sbPS.ToString() + "</span>");


                clsParsePDFDischargeMHC cPrint = new clsParsePDFDischargeMHC();
                // cPrint.Html =  common.myStr(ds.Tables[0].Rows[0]["ValueWordProcessor"]);

                string strHTML = sbPatientSummary.ToString();

                strHTML = common.removeUnusedHTML(strHTML);
                cPrint.Html = strHTML;

                // cPrint.Html = common.myStr(ds.Tables[0].Rows[0]["PatientSummary"]);
                cPrint.FirstPageHeaderHtml = sbtopheader.ToString();//     sb.ToString();    

                // if(common.myBool( ds.Tables[0].Rows[0]["IsPrint"]))// Printed by
                cPrint.FooterLeftText = "Printed By: " + EmpNo;// common.myStr(Session["EmployeeName"]);
                                                               //  if (common.myBool(ds.Tables[0].Rows[0]["IsPrintDate"]))//Prinded data 

                //cPrint.LowestFooter = "Aster Medcity" + Environment.NewLine +
                //                      "Kuttisahib Road, Near Kothad Bridge, South Chitoor PO, Cheranalloor, Kochi 682027, Kerala, India" + Environment.NewLine +
                //                      "T +91 484 6699999 E info@astermedcity.com W astermedcity.com";

                cPrint.FooterCenterText = "Printed Date: " + common.myStr(DateTime.Now.ToString("dd/MM/yyyy hh:mm tt"));

                //cPrint.FirstPageFooterLeft = "LeftText";
                //cPrint.FooterLeft = "LeftText";
                //cPrint.FirstPageFooterMiddle = "CenterText";
                //cPrint.FooterMiddle = "CenterText";
                // cPrint.FooterRight = "PageNofN";
                // cPrint.FirstPageFooterRight = "PageNofN";
                // cPrint.LowestFooter = sb2.ToString();// lowest footer
                //cPrint.FirstPageFooterHtml = sb2.ToString();

                cPrint.HeaderHtml = sb.ToString() + sb1.ToString();
                cPrint.MarginLeft = 20;
                cPrint.MarginRight = 15;
                cPrint.MarginTop = 105;
                cPrint.MarginBottom = 20;
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
            sFontSize += " ; font-size:12pt; ";
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
    private string GetSignatureDetails()
    {
        string sImage = "";
        StringBuilder sbSign = new StringBuilder();
        getDoctorImage();
        if (common.myInt(ViewState["SignDoctorID"]) > 0)
        {
            //sbSign.Append("<div  style='vertical-align:bottom;'>");
            sbSign.Append("<table cellspacing='0' cellpadding='0' border='0' style='font-family: Courier New;font-size:9pt;'  >");
            sbSign.Append("<tr><td></td><td></td><td></td><td></td><td></td></tr>");

            sbSign.Append("<tr>");
            sbSign.Append("<td></td><td></td>");
            sbSign.Append("<td colspan='3' align='left' style='font-family: Courier New;font-size:10pt;'>");
            sbSign.Append(hdnDoctorImage.Value.ToString());
            sbSign.Append("</td>");
            sbSign.Append("</tr>");

            sbSign.Append("<tr>");
            sbSign.Append("<td></td><td></td>");
            sbSign.Append("<td colspan='3' valign='top' align='left' style='font-family: Courier New;font-size:10pt; font-weight: bold;'>");
            //sbSign.Append(common.myStr(ViewState["SignDoctorName"]).Trim() + " " + ", <SPAN style='font-family: Courier New;font-size:9pt;'>" + common.myStr(ViewState["Education"]).Trim() + "</SPAN>");
            sbSign.Append(common.myStr(ViewState["SignDoctorName"]).Trim());

            sbSign.Append("</td>");
            sbSign.Append("</tr>");


            sbSign.Append("<tr>");
            sbSign.Append("<td></td><td></td>");
            sbSign.Append("<td  colspan='3'  style='font-family: Courier New;font-size:9pt;'>");
            sbSign.Append("(" + common.myStr(ViewState["Education"]) + ")");
            sbSign.Append("</td>");
            sbSign.Append("</tr>");

            sbSign.Append("<tr>");
            sbSign.Append("<td></td><td></td>");
            sbSign.Append("<td colspan='3' align='left' style='font-family: Courier New;font-size:10pt;'>");
            sbSign.Append(common.myStr(ViewState["signdesg"]).Trim() + " " + common.myStr(ViewState["SpecializationName"]).Trim());
            //sbSign.Append(common.myStr(ViewState["DepartmentName"]));
            sbSign.Append("</td>");
            sbSign.Append("</tr>");

            //sbSign.Append("<tr>");
            //sbSign.Append("<td></td><td></td><td></td>");
            //sbSign.Append("<td colspan='3' align='left' style='font-family: Courier New;font-size:9pt;'>");
            //sbSign.Append("Regn. No. : " + common.myStr(ViewState["UPIN"]).Trim());
            ////sbSign.Append(common.myStr(ViewState["DepartmentName"]));
            //sbSign.Append("</td>");
            //sbSign.Append("</tr>");

            if (common.myInt(ViewState["SignDoctorID"]) != common.myInt(ViewState["PreparedById"]) && common.myInt(ViewState["PreparedById"]) > 0)
            {
                sbSign.Append("<tr>");
                sbSign.Append("<td colspan='5' align='left' style='font-family: Courier New;font-size:10pt;' >Prepared By : " + common.myStr(ViewState["PreparedByName"]) + "</td>");
                sbSign.Append("</tr>");
            }

            sbSign.Append("</table>");
            sImage = common.myStr(sbSign);
        }
        return sImage;
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
        ViewState["EncounterId"] = common.myInt(Request.QueryString["EncId"]) == 0 ? common.myInt(Session["EncounterId"]) : common.myInt(Request.QueryString["EncId"]);
        string strimgData = string.Empty;
        try
        {
            if (common.myInt(ViewState["SignDoctorID"]) > 0)
            {
                ds = lis.getDoctorImageDetails(common.myInt(ViewState["SignDoctorID"]), common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
                                               common.myInt(ViewState["EncounterId"]));
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
                        //RTF1.Snippets.Add("<img width='100px' height='80px' src='/PatientDocuments/DoctorImages/" + FileName.Trim() + "' />", "<img src='/PatientDocuments/DoctorImages/" + FileName + "' />");
                        SignImage = "<img width='100px' height='80px' src='.../PatientDocuments/DoctorImages/" + FileName + "' />";
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
                            // RTF1.Snippets.Add("<img width='100px' height='80px' src='/PatientDocuments/DoctorImages/" + FileName + "' />", "<img src='/PatientDocuments/DoctorImages/" + FileName + "' />");
                            SignImage = "<img width='100px' height='80px' src='.../PatientDocuments/DoctorImages/" + FileName + "' />";
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
                            // RTF1.Snippets.Add("<img width='100px' height='80px' src='/PatientDocuments/DoctorImages/" + FileName + "' />", "<img src='/PatientDocuments/DoctorImages/" + FileName + "' />");
                            SignImage = "<img width='100px' height='80px' src='.../PatientDocuments/DoctorImages/" + FileName + "' />";
                            strSingImagePath = Server.MapPath("~") + @"\PatientDocuments\DoctorImages\" + FileName;

                            strimgData = common.myStr(dr["ImageId"]);
                        }
                    }
                }
                if (File.Exists(strSingImagePath))
                {
                    hdnDoctorImage.Value = DivStartTag + "<table  border='0' cellpadding='0' cellspacing='0' style='font-size:10pt; font-family:Tahoma;'><tbody><tr><td align='left'>" + SignImage + "</td></tr></tbody></table><br />";
                }
            }
        }
        catch (Exception ex)
        {
            //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //lblMessage.Text = "Error: " + ex.Message;
            //objException.HandleException(ex);
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
    private void facilityImage()
    {
        StringBuilder strSQL = new StringBuilder();
        String strSingImagePath = "";
        Hashtable hstInput = new Hashtable();
        string SignImage = "";
        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

        hstInput.Add("@intFacilityId", common.myInt(Session["FacilityId"]));
        hstInput.Add("@intHospId", common.myInt(Session["HospitalLocationId"]));

        strSQL.Append("select LogoImagePath from FacilityMaster where FacilityID=@intFacilityId and HospitalLocationID=@intHospId and Active=1");

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
        string value = "";
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (common.myStr(Caption) == "PN")//Patient Name-
            {
                value = common.myStr(ds.Tables[0].Rows[0]["PatientName"]);
            }
            if (common.myStr(Caption) == "RN")//Registration Number-
            {
                value = common.myStr(ds.Tables[0].Rows[0]["RegistrationNo"]);
            }
            if (common.myStr(Caption) == "SD")//Sample Collected Date
            {
                value = common.myStr(ds.Tables[0].Rows[0]["SampleCollectedDate"]);
            }
            if (common.myStr(Caption) == "PGA")// Patient Age
            {
                value = common.myStr(ds.Tables[0].Rows[0]["Age"]);
            }
            if (common.myStr(Caption) == "EN")//Encounter Number-
            {
                value = common.myStr(ds.Tables[0].Rows[0]["EncounterNo"]);
            }
            if (common.myStr(Caption) == "RD")//Result Date
            {
                value = common.myStr(ds.Tables[0].Rows[0]["ResultDate"]);
            }
            if (common.myStr(Caption) == "BNW") //Ward
            {
                value = common.myStr(ds.Tables[0].Rows[0]["Ward"]);
            }
            if (common.myStr(Caption) == "LN")//Lab No
            {
                value = common.myStr(ds.Tables[0].Rows[0]["LabNo"]);
            }
            if (common.myStr(Caption) == "RS")//Report Status
            {
                value = common.myStr(ds.Tables[0].Rows[0]["ReportStatus"]);
            }
            if (common.myStr(Caption) == "RB")//Referred By
            {
                value = common.myStr(ds.Tables[0].Rows[0]["Referred By"]);
            }
            if (common.myStr(Caption) == "PM")//Nationality Name
            {
                value = common.myStr(ds.Tables[0].Rows[0]["Nationality_Name"]);
            }
            if (common.myStr(Caption) == "CN")//Company Name
            {
                value = common.myStr(ds.Tables[0].Rows[0]["CompanyName"]);
            }
            //  -- 
            if (common.myStr(Caption) == "DOA")//Date Of Admission-
            {
                value = common.myStr(ds.Tables[0].Rows[0]["DOA"]);
            }
            if (common.myStr(Caption) == "DOD")//Date Of Discharge-
            {
                value = common.myStr(ds.Tables[0].Rows[0]["DOD"]);
            }
            if (common.myStr(Caption) == "CDN")//Consulting Doctor Name-
            {
                value = common.myStr(ds.Tables[0].Rows[0]["ConsultingDoctor"]);
            }
            if (common.myStr(Caption) == "CDD")//Consulting Doctor Designation
            {
                value = common.myStr(ds.Tables[0].Rows[0]["ConsultingDoctorDesignation"]);
            }
            if (common.myStr(Caption) == "CND")//Consulting Doctor Department
            {
                value = common.myStr(ds.Tables[0].Rows[0]["ConsultingDoctorDepartment"]);
            }
            if (common.myStr(Caption) == "PAG")//Patient Of Gender-
            {
                value = common.myStr(ds.Tables[0].Rows[0]["Age/Gender"]);
            }
            if (common.myStr(Caption) == "BNO")//Bed Number-
            {
                value = common.myStr(ds.Tables[0].Rows[0]["BedNo"]);
            }
            if (common.myStr(Caption) == "BCM")//Bed Category-
            {
                value = common.myStr(ds.Tables[0].Rows[0]["BedCategoryName"]);
            }
            if (common.myStr(Caption) == "PADD")//Patient Address-
            {
                value = common.myStr(ds.Tables[0].Rows[0]["PatientAddress"]);
            }
        }
        return value;
    }
    public StringBuilder AddTopHeader(string DepartmentId, string ConsultingDoctorDepartment, string EntrySite)
    {
        StringBuilder sbTopHeader = new StringBuilder();
        sbTopHeader.Append("<div ><hr width='100%' size='1' /></div>");

        sbTopHeader.Append("<table cellpadding='2' cellspacing=1 style='border-style:none;font-size:10pt;font-weight:bold;font-family:Courier New;'>");
        sbTopHeader.Append("<tr  ><td valign='top'  align='left'  >Center of Excellence  :  " + EntrySite + " </td>");
        sbTopHeader.Append("<td valign='top' align='left'  >Department  :  " + ConsultingDoctorDepartment + "</td></tr>");


        //sbTopHeader.Append("<tr><td valign='top' colspan='3' align='left' style='border-style:none;width:10px;font-size:8pt;'>Names of Doctors</td><td valign='top' align='right'  ></td>");
        ////sbTopHeader.Append("<td colspan='18' valign='top'  align='left' style='border-style:none;width:10px;font-size:8pt;'>: " + DoctorName + "</td></tr>");
        //sbTopHeader.Append("<td colspan='18' valign='top'  align='left' style='border-style:none;width:10px;font-size:8pt;'>: </td></tr>");
        sbTopHeader.Append("</table>");

        clsIVF objI = new clsIVF(sConString);
        DataSet dsSpec = new DataSet();
        dsSpec = objI.getDoctorSpecialisation(common.myInt(Session["DoctorId"]));

        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        Hashtable hshInput = new Hashtable();
        hshInput.Add("@intDepartmentId", DepartmentId);
        if (dsSpec.Tables.Count > 0)
        {
            if (dsSpec.Tables[0].Rows.Count > 0)
            {
                hshInput.Add("@intSpecialisationId", common.myInt(dsSpec.Tables[0].Rows[0]["SpecialisationId"]));
            }
        }
        DataSet ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspGetDepartmentDoctorsList", hshInput);
        if (ds.Tables[0].Rows.Count > 0)
        {
            int columncount = common.myInt(ds.Tables[1].Rows[0]["ColumnNo"]);
            int rowcount = common.myInt(ds.Tables[1].Rows[0]["RowNo"]);
            if (ds.Tables[0].Rows.Count > rowcount * columncount)// this section increase the row
            {
                rowcount = (common.myInt(ds.Tables[0].Rows.Count) / columncount);
                if (common.myInt(ds.Tables[0].Rows.Count) % columncount > 0)
                    rowcount = rowcount + 1;
            }
            sbTopHeader.Append("<table   border=0 cellpadding=1 cellspacing=0>");
            int row = 0;
            for (int i = 0; i < rowcount; i++)
            {
                if (row < ds.Tables[0].Rows.Count)
                {
                    if (i == 0)
                        sbTopHeader.Append("<tr><td  valign='top' colspan='1'  align='left' style='border-style:none;font-size:8pt;width : 24%;'>&nbsp;&nbsp;&nbsp;&nbsp;" + common.myStr(ds.Tables[0].Rows[row]["DoctorName"]) + "</td>");
                    else
                        sbTopHeader.Append("<td  valign='top' colspan='1' align='left' style='border-style:none;font-size:8pt;width : 24%;'>&nbsp;&nbsp;&nbsp;&nbsp;" + common.myStr(ds.Tables[0].Rows[row]["DoctorName"]) + "</td>");

                    row = row + 1;
                    for (int j = 1; j < columncount; j++)
                    {
                        if ((rowcount * j) + (i + 1) <= ds.Tables[0].Rows.Count)
                        {
                            sbTopHeader.Append("<td  valign='top' colspan='1' align='left' style='border-style:none;font-size:8pt;width : 24%;'>&nbsp;&nbsp;&nbsp;&nbsp;" + common.myStr(ds.Tables[0].Rows[row]["DoctorName"]) + "</td>");
                            row = row + 1;
                        }
                        else
                        {
                            sbTopHeader.Append("<td  valign='top' colspan='1'  align='left' style='border-style:none;font-size:8pt;width : 24%;'></td>");
                        }
                    }
                    sbTopHeader.Append("</tr>");
                }
            }
            sbTopHeader.Append("</table>");
        }
        return sbTopHeader;
    }
}
