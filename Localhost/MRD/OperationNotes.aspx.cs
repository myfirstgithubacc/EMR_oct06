using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows.Forms;
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
using Telerik.Web.UI;
using System.Security.Principal;
using System.Net;

public partial class MRD_OperationNotes : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    //private Hashtable hstInput;
    clsExceptionLog objException = new clsExceptionLog();
    string sFontSize = "";
    bool SignStatus = false;
    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";

        if (common.myStr(Request.QueryString["From"]) == "POPUP")
        {
            this.MasterPageFile = "~/Include/Master/BlankMaster.master";
        }
        else
        {
            this.MasterPageFile = "~/Include/Master/EMRMaster.master";
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserID"] == null)
        {
            Response.Redirect("/Login.aspx?Logout=1", false);
        }

        if (Request.QueryString["Mpg"] != null)
        {
            Session["CurrentNode"] = common.myStr(Request.QueryString["Mpg"]);
        }
        if (common.myInt(Session["RegistrationId"]).Equals(0) && common.myStr(Request.QueryString["callby"]).Equals(string.Empty))
        {
            Response.Redirect("/Default.aspx?RegNo=0", false);
        }

        if (!IsPostBack)
        {
            #region set view state values

            ViewState["RegistrationId"] = common.myInt(Request.QueryString["RegId"]);
            ViewState["EncounterId"] = common.myInt(Request.QueryString["EncId"]);
            ViewState["EREncounterId"] = common.myInt(Request.QueryString["EREncounterId"]);
            ViewState["DoctorId"] = common.myInt(Request.QueryString["DoctorId"]);
            ViewState["OPIP"] = common.myStr(Request.QueryString["OPIP"]).Trim();
            ViewState["EncounterDate"] = common.myStr(Request.QueryString["EncounterDate"]).Trim();
            ViewState["From"] = common.myStr(Request.QueryString["From"]).Trim();
            ViewState["callby"] = common.myStr(Request.QueryString["callby"]).Trim();

            DataSet ds = new DataSet();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable HshIn = new Hashtable();
            StringBuilder qry = new StringBuilder();

            HshIn.Add("@intFacilityId", common.myInt(Session["FacilityId"]));

            qry.Append("SELECT spt.TemplateId from SecModulePageTemplateGroup stg INNER JOIN  ");
            qry.Append(" SecModulePageTemplates spt on stg.GroupId = spt.GroupId   ");
            qry.Append(" where stg.GroupCode = 'OT' And stg.Active=1 and spt.Active =1 And stg.FacilityId=@intFacilityId ");

            ds = objDl.FillDataSet(CommandType.Text, qry.ToString(), HshIn);
            string strtemplateId = "";
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                if (i == 0)
                {
                    strtemplateId = common.myStr(ds.Tables[0].Rows[i]["TemplateId"]);
                }
                else
                {
                    strtemplateId = strtemplateId + "," + common.myStr(ds.Tables[0].Rows[i]["TemplateId"]);
                }
            }
            ViewState["StrTemplate"] = strtemplateId;




            ViewState["SignStatus"] = false;
            ViewState["iPrevId"] = "0";

            #endregion

            Session["DisplayEnteredByInCaseSheet"]=null;

            Session["DisplayEnteredByInCaseSheet"] = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]),
            common.myInt(Session["FacilityId"]), "DisplayEnteredByInCaseSheet", sConString);




            Session["SelectFindPatient"] = null;
            GetFonts();
            bindTemplateControl();
            getDoctorImage();

            bindDetails();
            BindPatientHiddenDetails();
            pnlPrint.Visible = false;
            pnlCaseSheet.Visible = true;
            RTF1.RealFontSizes.Clear();
            RTF1.RealFontSizes.Add("9pt");
            RTF1.RealFontSizes.Add("11pt");
            RTF1.RealFontSizes.Add("12pt");
            RTF1.RealFontSizes.Add("14pt");
            RTF1.RealFontSizes.Add("18pt");
            RTF1.RealFontSizes.Add("20pt");
            RTF1.RealFontSizes.Add("24pt");
            RTF1.RealFontSizes.Add("26pt");
            RTF1.RealFontSizes.Add("36pt");

            RTF1.EnsureToolsFileLoaded();
            RemoveButton("FindAndReplace");
            RemoveButton("Cut");
            RemoveButton("Copy");
            RemoveButton("Paste");


            // bindReportList();
            RTF1.EditModes = EditModes.Preview;

            if (common.myInt(ViewState["EncounterId"]) > 0)
            {
                lblMessage.Text = "";
            }
            else if (ViewState["callby"] == string.Empty)
            {
                btnDictionary.Enabled = false;
                btnSentenceGallery.Enabled = false;
                //btnSigned.Enabled = false;
                //btnSeenByDoctor.Enabled = false;
                //btnSave.Enabled = false;
                RTF1.Enabled = false;
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Text = "No Active Encounter Exist for this Appointment";
            }
        }
    }
    public void RemoveButton(string name)
    {
        foreach (Telerik.Web.UI.EditorToolGroup group in RTF1.Tools)
        {
            EditorTool tool = group.FindTool(name);
            if (tool != null)
            {
                group.Tools.Remove(tool);
            }
        }
    }
    protected void btnCheck_Onclick(object sender, EventArgs e)
    {
        if (common.myStr(Session["rtfText"]) != string.Empty)
        {
            String Msg = "<br/>Date & Time: " + common.myStr(System.DateTime.Now) + "<br/>";
            Msg += "Added by: " + common.myStr(Session["EmpName"]) + "<br/>";
            RTF1.Content = RTF1.Content + Msg + common.myStr(Session["rtfText"]);
        }
        //btnSave.Enabled = true;
    }

    protected void btnAddSen_Onclick(object sender, EventArgs e)
    {
        // if (Session["rtfText"] != null)
        RTF1.Content = RTF1.Content + hdSen.Value + "<br />";
     //   btnSave.Enabled = true;
    }

    void BindPatientHiddenDetails()
    {
        try
        {
            if (common.myStr(Request.QueryString["RPD"]) != "")
            {
                lblPatientDetail.Text = common.myStr(Session["RelationPatientDetailString"]);
            }
            else if (Session["PatientDetailString"] != null)
            {
                lblPatientDetail.Text = common.myStr(Session["PatientDetailString"]);
            }
        }
        catch (Exception ex)
        {
        }
    }

    private void bindDetails()
    {
        try
        {
          //  SignStatus = GetStatus();
            //if (SignStatus && ddlTemplatePatient.SelectedValue == "0")
            //{
                //BaseC.EMRMasters objbc = new BaseC.EMRMasters(sConString);
                //DataSet ds = new DataSet();
                //ds = objbc.GetViewHistory(common.myInt(ViewState["RegistrationId"]), common.myInt(ViewState["EncounterId"]));
                //if (ds.Tables[0].Rows.Count > 0)
                //{
                //    RTF1.Content = common.myStr(ds.Tables[0].Rows[0]["patientsummary"]);

                //}
           // }
            //else
            //{
              RTF1.Content = BindPatientIllustrationImages(SignStatus);
            //}
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
        finally
        {

        }
    }

   

    private StringBuilder getReportHeader()
    {
        StringBuilder sb = new StringBuilder();
        DataSet ds = new DataSet();

        clsIVF objivf = new clsIVF(sConString);

        ds = objivf.getFacility(common.myInt(Session["FacilityId"]), common.myInt(Session["HospitalLocationID"]));

        sb.Append("<div>");

        if (ds.Tables[0].Rows.Count > 0)
        {
            sb.Append("<table border='0' cellpadding='0' cellspacing='2' style='font-size:smaller' >");
            for (int idx = 0; idx < ds.Tables[0].Rows.Count; idx++)
            {
                DataRow DR = ds.Tables[0].Rows[idx];

                sb.Append("<tr>");
                sb.Append("<td  width='60%'></td>");
                sb.Append("<td align ='right' width='40%' ><h3><b>" + common.myStr(DR["FacilityName"]).Trim() + "</h3></b></td>");
                sb.Append("</tr>");

                sb.Append("<tr>");
                sb.Append("<td  width='60%'></td>");
                sb.Append("<td align ='right' width='40%'>" + common.myStr(DR["Address1"]).Trim() + ", " + common.myStr(DR["Address2"]).Trim() + "</td>");
                sb.Append("</tr>");

                sb.Append("<tr>");
                sb.Append("<td  width='60%'></td>");
                sb.Append("<td align ='right' width='40%'>" + common.myStr(DR["CityName"]) + "(" + common.myStr(DR["PinNo"]) + "), " + common.myStr(DR["StateName"]) + "</td>");

                sb.Append("</tr>");

                sb.Append("<tr>");
                sb.Append("<td  width='60%'></td>");
                sb.Append("<td align ='right' width='40%'>Phone : " + common.myStr(DR["Phone"]) + " Fax : " + common.myStr(DR["Fax"]) + "</td>");
                sb.Append("</tr>");
            }
            sb.Append("</table>");
        }
        sb.Append("<br />");

        sb.Append("<table border='0' width='100%' style='text-align:center;'  cellpadding='2' cellspacing='3' ><tr>");
        sb.Append("<td align=center><U>" + common.myStr("") + "</U></td>");
        sb.Append("</tr></table></div>");

        return sb;
    }

    private StringBuilder getIVFPatient()
    {
        StringBuilder sb = new StringBuilder();
        DataSet ds = new DataSet();

        clsIVF objivf = new clsIVF(sConString);

        ds = objivf.getIVFPatient(common.myInt(ViewState["RegistrationId"]), 0);

        if (ds.Tables[0].Rows.Count > 0)
        {
            DataView DV = ds.Tables[0].Copy().DefaultView;
            DV.RowFilter = "RegistrationId=" + common.myInt(ViewState["RegistrationId"]);

            DataTable tbl = DV.ToTable();

            if (tbl.Rows.Count > 0)
            {
                DataRow DR = tbl.Rows[0];

                DataView DVSpouse = ds.Tables[0].Copy().DefaultView;
                DVSpouse.RowFilter = "RegistrationId<>" + common.myInt(ViewState["RegistrationId"]);
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

   

    protected string CreateReportString(DataSet objDs, int iRootId, string iRootName, string TabularType, int NoOfBlankRows)
    {
        StringBuilder objStr = new StringBuilder();
        DataView objDv = null;
        DataTable objDt = null;
        DataSet dsMain = new DataSet();
        StringBuilder objStrTmp = new StringBuilder();
        int MaxLength = 0;
        int FieldsLength = 0;
        string sFontSize = string.Empty;

        if (objDs != null)
        {
            if (common.myBool(TabularType))
            {
                if (objDs.Tables[0].Rows.Count > 0)
                {
                    //changes start
                    DataView dvValues = new DataView(objDs.Tables[1]);
                    DataRow dr2;
                    foreach (DataRow dr in objDs.Tables[0].Rows)
                    {
                        dvValues.RowFilter = "FieldId = " + common.myStr(dr["FieldId"]);

                        //MaxLength = dvValues.ToTable().Rows.Count;

                        MaxLength = common.myInt(dvValues.ToTable().Compute("MAX(RowNo)", string.Empty));

                        if (MaxLength > 0)
                        {
                            dr2 = dr;
                            break;
                        }
                    }

                    if (MaxLength != 0)
                    {
                        int tableBorder = 1;

                        int TRows = 0;
                        int SectionId = 0;
                        if (objDs.Tables[0].Rows.Count > 0)
                        {
                            TRows = common.myInt(objDs.Tables[0].Rows[0]["TRows"]);
                            SectionId = common.myInt(objDs.Tables[0].Rows[0]["SectionId"]);
                        }

                        if (SectionId == 4608
                            || SectionId == 4610
                            || SectionId == 4611)
                        {
                            tableBorder = 0;
                        }

                        objStr.Append("<br /><table border='" + tableBorder + "' style='border-color:#000000; border-collapse:collapse; " + sFontSize + "'  cellpadding='4' cellspacing='4' ><tr align='center'>");

                        FieldsLength = objDs.Tables[0].Rows.Count;

                        #region header row - tabular with rows defination

                        DataSet dsR = new DataSet();
                        if (TRows > 0)
                        {
                            //border-bottom-color:Black; border-bottom-style:solid; border-bottom-width:1px;
                            objStr.Append("<th align='center' style=' " + sFontSize + " ' >" + "+" + "</th>");
                        }
                        #endregion

                        for (int i = 0; i < FieldsLength; i++)   // it makes table header
                        {
                            //border-bottom-color:Black; border-bottom-style:solid; border-bottom-width:1px;

                            string strHeader = common.myStr(objDs.Tables[0].Rows[i]["FieldName"]);

                            objStr.Append("<th align='center' style=' " + sFontSize + " ' >" + strHeader + "</th>");
                            dr2 = objDs.Tables[0].Rows[i];

                            dvValues.RowFilter = "";
                            dvValues = new DataView(objDs.Tables[1]);
                            dvValues.RowFilter = "FieldId='" + common.myStr(dr2["FieldId"]) + "'";
                            dsMain.Tables.Add(dvValues.ToTable("table" + i.ToString()));

                            if (dvValues.ToTable().Rows.Count > MaxLength)
                            {
                                MaxLength = dvValues.ToTable().Rows.Count;
                            }
                        }

                        objStr.Append("</tr>");
                        if (MaxLength == 0)
                        {
                        }
                        else
                        {
                            for (int i = 0; i < MaxLength; i++)
                            {
                                StringBuilder sbTR = new StringBuilder();
                                bool isDataFound = false;

                                for (int j = 0; j < dsMain.Tables.Count; j++)
                                {
                                    DataView dvM = dsMain.Tables[j].DefaultView;
                                    dvM.RowFilter = "RowNo=" + (i + 1);
                                    dvM.Sort = "RowNo ASC";

                                    DataTable tbl = dvM.ToTable();

                                    if (TRows > 0 && j == 0)
                                    {
                                        if (tbl.Rows.Count > 0)
                                        {
                                            if (common.myLen(tbl.Rows[0]["RowCaption"]) > 0)
                                            {
                                                sbTR.Append("<td style=' " + sFontSize.Replace("bold", "normal") + "'>" + common.myStr(tbl.Rows[0]["RowCaption"]) + "</td>");
                                            }
                                        }
                                        else
                                        {
                                            if (dsMain.Tables.Count > (j + 1))
                                            {
                                                DataView dvM2 = dsMain.Tables[j + 1].DefaultView;
                                                dvM2.RowFilter = "RowNo=" + (i + 1);
                                                dvM2.Sort = "RowNo ASC";

                                                DataTable tblH = dvM2.ToTable();
                                                if (tblH.Rows.Count > 0)
                                                {
                                                    if (common.myLen(tblH.Rows[0]["RowCaption"]) > 0)
                                                    {
                                                        sbTR.Append("<td style=' " + sFontSize.Replace("bold", "normal") + "'>" + common.myStr(tblH.Rows[0]["RowCaption"]) + "</td>");
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    if (tbl.Rows.Count > 0)
                                    {
                                        isDataFound = true;
                                        sbTR.Append("<td style=' " + sFontSize.Replace("bold", "normal") + "'>" + common.myStr(tbl.Rows[0]["TextValue"]) + "</td>");
                                    }
                                    else
                                    {
                                        sbTR.Append("<td style=' " + sFontSize.Replace("bold", "normal") + "'>&nbsp;</td>");
                                    }
                                }

                                if (isDataFound)
                                {
                                    objStr.Append("<tr valign='top'>");
                                    objStr.Append(sbTR.ToString());
                                    objStr.Append("</tr>");
                                }
                            }

                            for (int rIdx = 0; rIdx < NoOfBlankRows; rIdx++)
                            {
                                objStr.Append("<tr valign='top'>");

                                for (int cIdx = 0; cIdx < dsMain.Tables.Count; cIdx++)
                                {
                                    objStr.Append("<td style=' " + sFontSize.Replace("bold", "normal") + "' align='right'>&nbsp;</td>");
                                }
                                objStr.Append("</tr>");
                            }

                            objStr.Append("</table>");
                            //}
                        }
                    }
                    //changes end



                    //DataRow dr = objDs.Tables[0].Rows[0];
                    //DataView dvValues = new DataView(objDs.Tables[1]);
                    //dvValues.RowFilter = "FieldId='" + common.myStr(dr["FieldId"]) + "'";

                    //MaxLength = dvValues.ToTable().Rows.Count;

                    //if (MaxLength != 0)
                    //{
                    //    //objStr.Append("<br /><br /><table border='1' style='border-color:#000000; border:solid;  border-collapse:collapse; " + sFontSize + "'  cellpadding='2' cellspacing='3' ><tr align='center'>");
                    //    objStr.Append("<br /><table border='1' style='border-color:#000000; border:solid;  border-collapse:collapse; " + sFontSize + "'  cellpadding='2' cellspacing='3' ><tr align='center'>");

                    //    FieldsLength = objDs.Tables[0].Rows.Count;

                    //    for (int i = 0; i < FieldsLength; i++)   // it makes table
                    //    {
                    //        objStr.Append("<th align='center' style='border-bottom-color:Black; border-bottom-style:solid; border-bottom-width:1px; " + sFontSize + " ' >" + common.myStr(objDs.Tables[0].Rows[i]["FieldName"]) + "</th>");
                    //        dr = objDs.Tables[0].Rows[i];
                    //        dvValues = new DataView(objDs.Tables[1]);
                    //        dvValues.RowFilter = "FieldId='" + common.myStr(dr["FieldId"]) + "'";
                    //        dsMain.Tables.Add(dvValues.ToTable("table" + i.ToString()));


                    //        if (dvValues.ToTable().Rows.Count > MaxLength)
                    //            MaxLength = dvValues.ToTable().Rows.Count;
                    //    }

                    //    objStr.Append("</tr>");
                    //    if (MaxLength == 0)
                    //    {
                    //        //objStr.Append("<tr>");
                    //        //for (int i = 0; i < FieldsLength; i++)
                    //        //{
                    //        //    objStr.Append("<td style=' " + sFontSize + "'> </td>");
                    //        //}
                    //        //objStr.Append("</tr></table>");
                    //    }
                    //    else
                    //    {
                    //        if (dsMain.Tables[0].Rows.Count > 0)
                    //        {
                    //            for (int i = 0; i < MaxLength; i++)
                    //            {
                    //                objStr.Append("<tr>");
                    //                for (int j = 0; j < dsMain.Tables.Count; j++)
                    //                {
                    //                    if (dsMain.Tables[j].Rows.Count > i
                    //                        && dsMain.Tables[j].Rows.Count > 0)
                    //                    {
                    //                        objStr.Append("<td style=' " + sFontSize + "' align='right'>" + common.myStr(dsMain.Tables[j].Rows[i]["TextValue"]) + "</td>");
                    //                    }
                    //                    else
                    //                    {
                    //                        objStr.Append("<td style=' " + sFontSize + "' align='right'>&nbsp;</td>");
                    //                    }
                    //                }
                    //                objStr.Append("</tr>");
                    //            }
                    //            objStr.Append("</table>");
                    //        }
                    //    }
                    //}
                }
            }
            else // For Non Tabular Templates
            {
                string BeginList = "", EndList = "";
                string sBegin = "", sEnd = "";
                int t = 0;

                objStr.Append("<br /><table border='0' style='border-color:#000000; border-collapse:collapse; " + sFontSize + "'  cellpadding='4' cellspacing='4' >");

                foreach (DataRow item in objDs.Tables[0].Rows)
                {
                    objStr.Append("<tr valign='top'>");

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
                        else if (common.myStr(item["FieldsListStyle"]) == "2")
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
                        if (objDt.Rows.Count > 0)
                        {
                            sEnd = "";
                            MakeFont("Fields", ref sBegin, ref sEnd, item);

                            if (sBegin.StartsWith("<br/>"))
                            {
                                if (sBegin.Length > 5)
                                {
                                    sBegin = sBegin.Substring(5, sBegin.Length - 5);
                                }
                            }

                            if (common.myBool(item["DisplayTitle"]))
                            {
                                objStr.Append("<td>");

                                objStr.Append(BeginList + sBegin + common.myStr(item["FieldName"]));
                                // 28/08/2011
                                //if (objDt.Rows.Count > 0)
                                //{
                                if (objStr.ToString() != "")
                                {
                                    //changes
                                    //objStr.Append(sEnd + "</li>");
                                    objStr.Append(sEnd);
                                }
                                //}

                                objStr.Append("</td>");
                            }
                            BeginList = "";
                            sBegin = "";
                        }
                    }
                    else
                    {
                        if (objDt.Rows.Count > 0)
                        {
                            if (common.myBool(item["DisplayTitle"]))
                            {
                                objStr.Append("<td>");
                                objStr.Append(common.myStr(item["FieldName"]));
                                objStr.Append("</td>");
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

                        if (dtFieldType.Rows.Count > 0
                            && objDt.Rows.Count == 0)
                        {
                            string FType = common.myStr(dtFieldType.Rows[0]["FieldType"]);
                            if (FType == "O")
                            {
                                int DataObjectId = common.myInt(dtFieldType.Rows[0]["DataObjectId"]);

                                clsIVF objivf = new clsIVF(sConString);

                                string strOutput = objivf.getDataObjectValue(DataObjectId);

                                if (common.myLen(strOutput) > 0)
                                {
                                    objStr.Append("<td>" + common.myStr(dtFieldType.Rows[0]["FieldName"]) + "</td>");
                                    objStr.Append("<td>" + strOutput + "</td>");
                                }
                            }
                        }

                        if (objDt.Rows.Count > 0)
                        {
                            objStr.Append("<td>");

                            for (int i = 0; i < objDv.ToTable().Rows.Count; i++)
                            {
                                //objStr.Append(": " + objDt.Rows[0]["PropertyValue"].ToString());
                                string FType = common.myStr(dtFieldType.Rows[0]["FieldType"]);
                                if (FType == "C")
                                {
                                    FType = "C";
                                }
                                if (FType == "C" || FType == "D" || FType == "B")
                                {
                                    if (FType == "B")
                                    {
                                        if (common.myStr(objDt.Rows[i]["FieldValue"]) == "1"
                                            || common.myStr(objDt.Rows[i]["FieldValue"]) == "0")
                                        {

                                            DataView dv1 = new DataView(objDs.Tables[1]);
                                            if (common.myStr(objDs.Tables[1].Rows[i]["FieldValue"]) == "1")
                                            {
                                                dv1.RowFilter = "TextValue='Yes'";
                                            }
                                            else
                                            {
                                                dv1.RowFilter = "TextValue='No'";
                                            }

                                            DataTable dt1 = dv1.ToTable();
                                            if (dt1.Rows.Count > 0)
                                            {
                                                if (common.myStr(dt1.Rows[i]["MainText"]).Trim() != "")
                                                {
                                                    if (i == 0)
                                                    {
                                                        objStr.Append(" " + common.myStr(dt1.Rows[i]["MainText"]));
                                                    }
                                                    else
                                                    {
                                                        objStr.Append(", " + common.myStr(dt1.Rows[i]["MainText"]));
                                                    }
                                                }
                                                else
                                                {
                                                    if (i == 0)
                                                    {
                                                        objStr.Append(" " + common.myStr(objDt.Rows[i]["TextValue"]));
                                                    }
                                                    else
                                                    {
                                                        objStr.Append(", " + common.myStr(objDt.Rows[i]["TextValue"]));
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (i == 0)
                                                {
                                                    objStr.Append(" " + common.myStr(objDt.Rows[i]["TextValue"]));
                                                }
                                                else
                                                {
                                                    objStr.Append(", " + common.myStr(objDt.Rows[i]["TextValue"]));
                                                }
                                            }
                                        }
                                        else
                                        {
                                            BindDataValue(objDs, objDt, objStr, i, FType);//check1
                                        }
                                    }
                                    else
                                    {
                                        BindDataValue(objDs, objDt, objStr, i, FType);
                                    }
                                }
                                else if (FType == "T" || FType == "M" || FType == "S" || FType == "W")
                                {
                                    if (common.myStr(ViewState["iTemplateId"]) != "163")
                                    {
                                        if (i == 0)
                                        {
                                            objStr.Append(" " + common.myStr(objDt.Rows[i]["TextValue"]));
                                        }
                                        else
                                        {
                                            objStr.Append(", " + common.myStr(objDt.Rows[i]["TextValue"]));
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
                                if (common.myStr(item["FieldsListStyle"]) == "")
                                {
                                    if (common.myStr(ViewState["iTemplateId"]) != "163")
                                    {
                                        if (FType != "C")
                                        {
                                            objStr.Append("<br />");
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
                            // Cmt 25/08/2011
                            //if (objDt.Rows.Count > 0)
                            //{
                            //    if (objStr.ToString() != "")
                            //        objStr.Append(sEnd + "</li>");
                            //}

                            objStr.Append("</td>");
                        }
                    }
                    //objStr.Append(" <span style=\" font-weight:bold; font-size:medium; color:Gray\">|</span> ");

                    objStr.Append("</tr>");
                }

                if (objStr.ToString() != "")
                {
                    objStr.Append(EndList);
                }

                objStr.Append("</table>");
            }
        }

        return objStr.ToString();
    }

  

    private void bindTemplateControl()
    {
        try
        {
            clsIVF objIVF = new clsIVF(sConString);
            DataSet ds;

            ds = objIVF.getEMRSpecialisationTemplate(common.myInt(Session["UserId"]));

            ddlSpecilityTemplate.DataSource = ds.Tables[0];
            ddlSpecilityTemplate.DataTextField = "TemplateName";
            ddlSpecilityTemplate.DataValueField = "TemplateId";
            ddlSpecilityTemplate.DataBind();

            ddlSpecilityTemplate.Items.Insert(0, new RadComboBoxItem("", "0"));
            ddlSpecilityTemplate.SelectedIndex = 0;

            ds = objIVF.getEMRGeneralTemplate();

            ddlGeneralTemplate.DataSource = ds.Tables[0];
            ddlGeneralTemplate.DataTextField = "TemplateName";
            ddlGeneralTemplate.DataValueField = "TemplateId";
            ddlGeneralTemplate.DataBind();

            ddlGeneralTemplate.Items.Insert(0, new RadComboBoxItem("", "0"));
            ddlGeneralTemplate.SelectedIndex = 0;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    protected void btnclose_OnClick(object sender, EventArgs e)
    {
    }

    protected void btnDictate_OnClick(object sender, EventArgs e)
    {
        RadWindow3.NavigateUrl = "speech.aspx";
        RadWindow3.Height = 600;
        RadWindow3.Width = 800;
        RadWindow3.Top = 20;
        RadWindow3.Left = 20;
        // RadWindowForNew.Title = "Time Slot";
        RadWindow3.OnClientClose = "OnClientClose";
        RadWindow3.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindow3.Modal = true;
        RadWindow3.VisibleStatusbar = false;
    }

    private void GetFonts()
    {
        BaseC.EMRMasters.Fonts fonts = new BaseC.EMRMasters.Fonts();
        DataSet ds = fonts.GetFontDetails("Size");
        if (ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                DataRow dr = ds.Tables[0].Rows[i];
                RTF1.RealFontSizes.Add(common.myStr(dr["FontSize"]));
            }
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
            if (common.myInt(ViewState["DoctorId"]) > 0)
            {
                ds = lis.getDoctorImageDetails(common.myInt(ViewState["DoctorId"]), common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
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
                        fs.Close();
                        fs.Dispose();
                        RTF1.Snippets.Add("<img width='100px' height='80px' src='/PatientDocuments/DoctorImages/" + FileName.Trim() + "' />", "<img src='/PatientDocuments/DoctorImages/" + FileName + "' />");
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
                            RTF1.Snippets.Add("<img width='100px' height='80px' src='/PatientDocuments/DoctorImages/" + FileName + "' />", "<img src='/PatientDocuments/DoctorImages/" + FileName + "' />");
                            SignImage = "<img width='100px' height='80px' src='/PatientDocuments/DoctorImages/" + FileName + "' />";
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
                            fs.Close();
                            fs.Dispose();
                            RTF1.Snippets.Add("<img width='100px' height='80px' src='/PatientDocuments/DoctorImages/" + FileName + "' />", "<img src='/PatientDocuments/DoctorImages/" + FileName + "' />");
                            SignImage = "<img width='100px' height='80px' src='/PatientDocuments/DoctorImages/" + FileName + "' />";
                            strSingImagePath = Server.MapPath("~") + @"\PatientDocuments\DoctorImages\" + FileName;

                            strimgData = common.myStr(dr["ImageId"]);
                        }
                    }
                }
                if (File.Exists(strSingImagePath))
                {
                    hdnDoctorImage.Value = DivStartTag + "<table  border='0' cellpadding='0' cellspacing='0' style='font-size:10pt; font-family:Tahoma;'><tbody><tr><td align='right'>" + SignImage + "</td></tr></tbody></table>" + SignNote + "<br />";
                }
            }
        }
        catch (Exception ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + ex.Message;
            objException.HandleException(ex);
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

    private string BindEditor(Boolean sign)
    {
        StringBuilder sb = new StringBuilder();
        StringBuilder sbTemplateStyle = new StringBuilder();
        StringBuilder TemplateString;
        DataSet ds = new DataSet();
        DataSet dsTemplateStyle = new DataSet();
        DataRow drTemplateStyle = null;
        DataTable dtTemplate = new DataTable();
        DataView dvDataFilter = new DataView();
        string Templinespace = "";
        BindNotes bnotes = new BindNotes(sConString);
        BaseC.DiagnosisDA fun = new BaseC.DiagnosisDA(sConString);
        BaseC.clsEMR emr = new BaseC.clsEMR(sConString);
        DataTable dtGroupDateWiseTemplate = new DataTable();
        StringBuilder sbTemp = new StringBuilder();
        bool bAllergyDisplay = false;

        string sTemplateName = "";// common.myStr(ddlTemplatePatient.SelectedItem.Text) == "ALL" ? "" : common.myStr(ddlTemplatePatient.SelectedItem.Text);

        #region Declare DataSet
        DataSet dsTemplateData = new DataSet();
        #endregion
        #region Call Bind Case Sheet class
        BindCaseSheet BindCaseSheet = new BindCaseSheet(sConString);
        #endregion
        try
        {
            string DoctorId = fun.GetDoctorId(common.myInt(Session["HospitalLocationID"]), Convert.ToInt16(common.myInt(Session["UserID"])));
            dsTemplateStyle = bnotes.GetTemplateStyle(common.myInt(Session["HospitalLocationId"]));

            #region Call Bind Case Sheet method to get data
            //if (ddlTemplatePatient.SelectedValue != "")
            //{
            //    dsTemplateData = emr.getEMRPrintCaseSheetDate(common.myInt(Session["HospitalLocationId"]),
            //        common.myInt(Session["FacilityId"]), common.myInt(ViewState["RegistrationId"]), common.myInt(ViewState["EncounterId"]), common.myDate(txtFromDate.SelectedDate.Value).ToString("yyyy/MM/dd"),
            //        common.myDate(txtToDate.SelectedDate.Value).ToString("yyyy/MM/dd"), sTemplateName, common.myInt(ddlTemplatePatient.SelectedValue), common.myStr(ddlTemplatePatient.SelectedItem.Attributes["TemplateType"]));
            //}
            //else
            //{
                dsTemplateData = emr.getEMRPrintCaseSheetDate(common.myInt(Session["HospitalLocationId"]),
                common.myInt(Session["FacilityId"]), common.myInt(ViewState["RegistrationId"]), common.myInt(ViewState["EncounterId"]), "1900/01/01",
                 "2059/01/01", sTemplateName, 0, "",true,0);
            //}
            #endregion

            dtGroupDateWiseTemplate = emr.GetDateWiseGroupingTemplate(common.myInt(Session["HospitalLocationId"]),
                common.myInt(Session["FacilityId"]), common.myInt(ViewState["RegistrationId"]), common.myInt(ViewState["EncounterId"]), common.myDate("1900/01/01").ToString("yyyy/MM/dd"), common.myDate("2100/01/01").ToString("yyyy/MM/dd"));
            dvDataFilter = new DataView(dsTemplateData.Tables[21]);

            if (dvDataFilter.ToTable().Rows.Count > 0)
            {
                for (int iGp = 0; iGp < dtGroupDateWiseTemplate.Rows.Count; iGp++)
                {
                    //dvDataFilter.RowFilter ="TemplateId in ( " + common.myStr(Request.QueryString["TemplateId"]) + ") And TemplateDate='" + common.myStr(dtGroupDateWiseTemplate.Rows[iGp]["TemplateDate"]) + "' OR ISNULL(TemplateDate,'')=''";
                   dvDataFilter.RowFilter = "TemplateId in ( " + common.myStr(ViewState["StrTemplate"]) + ") And TemplateDate='" + common.myStr(dtGroupDateWiseTemplate.Rows[iGp]["TemplateDate"]) + "' OR ISNULL(TemplateDate,'')=''";
                  
                    dtTemplate = dvDataFilter.ToTable();
                    TemplateString = new StringBuilder();
                    for (int i = 0; i < dtTemplate.Rows.Count; i++)
                    {
                        string templateid = common.myStr(dtTemplate.Rows[i]["TemplateId"]);
                        #region Chief Complaints
                        if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim() == "Chief Complaints"
                             && common.myStr(dtTemplate.Rows[i]["TemplateType"]).Trim() == "S"
                            && common.myStr(dtTemplate.Rows[i]["TemplateDate"]) == common.myStr(dtGroupDateWiseTemplate.Rows[iGp]["TemplateDate"]))
                        {
                            sbTemplateStyle = new StringBuilder();
                            DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                            dv.RowFilter = "PageId =" + common.myInt(dtTemplate.Rows[i]["PageId"]).ToString();
                            if (dv.Count > 0)
                            {
                                drTemplateStyle = dv[0].Row;
                                string sBegin = "", sEnd = "";
                                Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                                MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                            }
                            dv.Dispose();
                            sbTemp = new StringBuilder();
                            if ((common.myInt(templateid) == 0)
                                 || (common.myInt(templateid) == 98))
                            {
                                #region Call Bind Problem data
                                BindCaseSheet.BindProblemsHPI(dsTemplateData.Tables[0], common.myInt(ViewState["RegistrationId"]), common.myInt(ViewState["EncounterId"]), sbTemp, sbTemplateStyle, drTemplateStyle,
                                               Page, common.myStr(dtTemplate.Rows[i]["PageId"]), common.myStr(dtGroupDateWiseTemplate.Rows[iGp]["TemplateDate"]), true);
                                #endregion
                                if (sbTemp.ToString() != "")
                                    TemplateString.Append(sbTemp + "<br/>" + "<br/>");
                                sbTemp = null;
                                sbTemplateStyle = null;
                            }

                            drTemplateStyle = null;
                            Templinespace = "";
                        }
                        #endregion
                        #region Template History Type
                        if (common.myStr(dtTemplate.Rows[i]["TemplateId"]).Trim() != ""
                           && common.myStr(dtTemplate.Rows[i]["TemplateType"]).Trim() == "D"
                           && common.myStr(dtTemplate.Rows[i]["TemplateCode"]).Trim() == "HIS"
                            && common.myStr(dtTemplate.Rows[i]["TemplateDate"]) == common.myStr(dtGroupDateWiseTemplate.Rows[iGp]["TemplateDate"]))
                        {
                            DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                            dv.RowFilter = "PageId =" + common.myInt(dtTemplate.Rows[i]["TemplateId"]).ToString();
                            if (dv.Count > 0)
                            {
                                drTemplateStyle = dv[0].Row;
                                string sBegin = "", sEnd = "";
                                Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                                MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                            }
                            dv.Dispose();
                            sbTemp = new StringBuilder();
                            if ((common.myInt(templateid) == 0)
                                || (common.myInt(templateid) == common.myInt(dtTemplate.Rows[i]["TemplateId"])))
                            {
                                #region Assign Data and call History Type Dynamic Template
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
                                if (common.myInt(templateid) == 0)
                                {
                                    dvDyTable1.RowFilter = "TemplateId=" + common.myStr(dtTemplate.Rows[i]["TemplateId"]);
                                    dvDyTable4.RowFilter = "TemplateId=" + common.myStr(dtTemplate.Rows[i]["TemplateId"]);
                                    dvDyTable6.RowFilter = "TemplateId=" + common.myStr(dtTemplate.Rows[i]["TemplateId"]);
                                    dtDyTempTable = dvDyTable4.ToTable();
                                    dvDyTable4.Sort = "RecordId ASC";
                                }
                                else
                                {
                                    dvDyTable1.RowFilter = "TemplateId=" + common.myStr(templateid);
                                    dvDyTable4.RowFilter = "TemplateId=" + common.myStr(templateid);
                                    dvDyTable6.RowFilter = "TemplateId=" + common.myStr(templateid);
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
                                dvDyTable3.RowFilter = "SectionId IN (" + sSectionId + ") AND GroupDate='" + common.myStr(dtGroupDateWiseTemplate.Rows[iGp]["TemplateDate"]) + "'";
                                dvDyTable4.RowFilter = "GroupDate='" + common.myStr(dtGroupDateWiseTemplate.Rows[iGp]["TemplateDate"]) + "' AND SectionId IN (" + sSectionId + ")";

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
                                    bindData(dsDymanicTemplateData, common.myStr(dtTemplate.Rows[i]["TemplateId"]), sbTemp, common.myStr(dtGroupDateWiseTemplate.Rows[iGp]["TemplateDate"]));
                                    if (sbTemp.Length > 20)
                                        TemplateString.Append(sbTemp + "<br/>");
                                }

                                sbTemp = null;
                                dsDymanicTemplateData.Dispose();
                                dvDyTable1.Dispose();
                                dvDyTable2.Dispose();
                                dvDyTable3.Dispose();
                                dvDyTable4.Dispose();
                                dvDyTable5.Dispose();
                                dvDyTable6.Dispose();
                                dtDyTempTable.Dispose();
                                sSectionId = "";
                                #endregion
                            }

                            Templinespace = "";
                        }
                        #endregion
                        #region Allergy
                        if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim() == "Allergies"
                             && common.myStr(dtTemplate.Rows[i]["TemplateType"]).Trim() == "S" && bAllergyDisplay == false)
                        {
                            sbTemplateStyle = new StringBuilder();
                            DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                            dv.RowFilter = "PageId =" + common.myInt(dtTemplate.Rows[i]["PageId"]).ToString();
                            drTemplateStyle = null;// = dv[0].Row;
                            if (dv.Count > 0)
                            {
                                drTemplateStyle = dv[0].Row;
                                string sBegin = "", sEnd = "";
                                Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                                MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                            }
                            dv.Dispose();
                            sbTemp = new StringBuilder();

                            if ((common.myInt(templateid) == 0)
                                 || (common.myInt(templateid) == 8))
                            {
                                #region Call Allergy template data
                                BindCaseSheet.BindAllergies(dsTemplateData.Tables[1], sbTemp, sbTemplateStyle, drTemplateStyle, Page,
                                           common.myStr(dtTemplate.Rows[i]["PageID"]), 0, "", true);
                                #endregion
                                if (sbTemp.ToString() != "")
                                {
                                    TemplateString.Append(sbTemp + "<br/>");
                                    bAllergyDisplay = true;
                                }
                                sbTemp = null;
                                sbTemplateStyle = null;
                            }
                            drTemplateStyle = null;
                            Templinespace = "";
                        }
                        #endregion
                        #region Vital
                        if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim() == "Vitals"
                             && common.myStr(dtTemplate.Rows[i]["TemplateType"]).Trim() == "S"
                            && common.myStr(dtTemplate.Rows[i]["TemplateDate"]) == common.myStr(dtGroupDateWiseTemplate.Rows[iGp]["TemplateDate"]))
                        {
                            sbTemplateStyle = new StringBuilder();
                            DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                            dv.RowFilter = "PageId =" + common.myInt(dtTemplate.Rows[i]["PageId"]).ToString();
                            if (dv.Count > 0)
                            {
                                drTemplateStyle = dv[0].Row;
                                string sBegin = "", sEnd = "";
                                Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                                MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                            }
                            dv.Dispose();
                            sbTemp = new StringBuilder();

                            if ((common.myInt(templateid) == 0)
                                 || (common.myInt(templateid) == 14))
                            {
                                #region Call Vital Template data
                                BindCaseSheet.BindVitals(dsTemplateData.Tables[11], sbTemp, sbTemplateStyle, drTemplateStyle,
                                                    Page, common.myStr(dtTemplate.Rows[i]["PageId"]), 0,
                                                    common.myStr(dtGroupDateWiseTemplate.Rows[iGp]["TemplateDate"]), common.myInt(ViewState["EncounterId"]), true);

                                #endregion
                                if (sbTemp.ToString() != "")
                                    TemplateString.Append(sbTemp + "<br/>" + "<br/>");
                                sbTemp = null;
                                sbTemplateStyle = null;
                            }

                            drTemplateStyle = null;
                            Templinespace = "";

                        }
                        #endregion
                        #region All the Templates except Hitory and Plan of case
                        if (common.myStr(dtTemplate.Rows[i]["TemplateId"]).Trim() != ""
                            && common.myStr(dtTemplate.Rows[i]["TemplateType"]).Trim() == "D"
                            && common.myStr(dtTemplate.Rows[i]["TemplateCode"]).Trim() != "POC"
                            && common.myStr(dtTemplate.Rows[i]["TemplateCode"]).Trim() != "HIS"
                            && common.myStr(dtTemplate.Rows[i]["TemplateDate"]) == common.myStr(dtGroupDateWiseTemplate.Rows[iGp]["TemplateDate"]))
                        {
                            DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                            dv.RowFilter = "PageId =" + common.myInt(dtTemplate.Rows[i]["TemplateId"]).ToString();
                            if (dv.Count > 0)
                            {
                                drTemplateStyle = dv[0].Row;
                                string sBegin = "", sEnd = "";
                                Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                                MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                            }
                            dv.Dispose();
                            sbTemp = new StringBuilder();
                            if ((common.myInt(templateid) == 0)
                                || (common.myInt(templateid) == common.myInt(dtTemplate.Rows[i]["TemplateId"])))
                            {
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
                                if (common.myInt(templateid) == 0)
                                {
                                    dvDyTable1.RowFilter = "TemplateId=" + common.myStr(dtTemplate.Rows[i]["TemplateId"]);
                                    dvDyTable4.RowFilter = "TemplateId=" + common.myStr(dtTemplate.Rows[i]["TemplateId"]);
                                    dvDyTable6.RowFilter = "TemplateId=" + common.myStr(dtTemplate.Rows[i]["TemplateId"]);
                                    dtDyTempTable = dvDyTable4.ToTable();
                                    dvDyTable4.Sort = "RecordId ASC";
                                }
                                else
                                {
                                    dvDyTable1.RowFilter = "TemplateId=" + common.myStr(templateid);
                                    dvDyTable4.RowFilter = "TemplateId=" + common.myStr(templateid);
                                    dvDyTable6.RowFilter = "TemplateId=" + common.myStr(templateid);
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
                                dvDyTable3.RowFilter = "SectionId IN (" + sSectionId + ") AND GroupDate='" + common.myStr(dtGroupDateWiseTemplate.Rows[iGp]["TemplateDate"]) + "'";

                                dvDyTable4.RowFilter = "GroupDate='" + common.myStr(dtGroupDateWiseTemplate.Rows[iGp]["TemplateDate"]) + "' AND SectionId IN (" + sSectionId + ")";

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
                                    bindData(dsDymanicTemplateData, common.myStr(dtTemplate.Rows[i]["TemplateId"]), sbTemp, common.myStr(dtGroupDateWiseTemplate.Rows[iGp]["TemplateDate"]));
                                    if (sbTemp.Length > 20)
                                        TemplateString.Append(sbTemp + "<br/>");
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
                            }

                            Templinespace = "";
                        }
                        #endregion
                        #region Lab
                        if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim() == "Lab Test Result"
                            && common.myStr(dtTemplate.Rows[i]["DataStatus"]).Trim() == "AVAILABLE")
                        {
                            string strTemplateType = common.myStr(dtTemplate.Rows[i]["PageIdentification"]);
                            strTemplateType = strTemplateType.Substring(0, 1);
                            //sbTemplateStyle = new StringBuilder();
                            DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                            dv.RowFilter = "PageId =" + common.myInt(dtTemplate.Rows[i]["PageId"]).ToString();
                            if (dv.Count > 0)
                            {
                                drTemplateStyle = dv[0].Row;
                                string sBegin = "", sEnd = "";
                                Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                                MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                            }
                            dv.Dispose();
                            sbTemp = new StringBuilder();

                            // bnotes.BindLabTestResult(common.myInt(Session["RegistrationId"]), HospitalId, EncounterId, UserId, DoctorId, sbTemp, sbTemplateStyle, drTemplateStyle, Page, common.myInt(Session["FacilityID"]), common.myStr(dtTemplate.Rows[i]["PageId"]), common.myStr(Session["UserID"]));
                            TemplateString.Append(sbTemp);
                            drTemplateStyle = null;

                            sbTemp = null;
                            Templinespace = "";
                        }
                        #endregion
                        #region Diagnosis
                        if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim() == "Diagnosis"
                           && common.myStr(dtTemplate.Rows[i]["TemplateType"]).Trim() == "S"
                            && common.myStr(dtTemplate.Rows[i]["TemplateDate"]) == common.myStr(dtGroupDateWiseTemplate.Rows[iGp]["TemplateDate"]))
                        {
                            sbTemplateStyle = new StringBuilder();
                            DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                            dv.RowFilter = "PageId =" + common.myInt(dtTemplate.Rows[i]["PageId"]).ToString();
                            if (dv.Count > 0)
                            {
                                drTemplateStyle = dv[0].Row;
                                string sBegin = "", sEnd = "";
                                Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                                MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                            }
                            dv.Dispose();
                            sbTemp = new StringBuilder();

                            if ((common.myInt(templateid) == 0)
                                 || (common.myInt(templateid) == 133))
                            {
                                #region Call Diagnosis Template Data
                                BindCaseSheet.BindAssessments(dsTemplateData.Tables[4], sbTemp, sbTemplateStyle, drTemplateStyle, Page,
                                           common.myStr(dtTemplate.Rows[i]["PageId"]), common.myStr(Session["UserID"]),
                                           0, common.myInt(ViewState["EncounterId"]), common.myStr(dtGroupDateWiseTemplate.Rows[iGp]["TemplateDate"]), "Y", true);
                                #endregion
                                if (sbTemp.ToString() != "")
                                    TemplateString.Append(sbTemp + "<br/>");
                                sbTemp = null;
                                sbTemplateStyle = null;
                            }
                            drTemplateStyle = null;
                            Templinespace = "";
                        }
                        #endregion
                        #region Provisional Diagnosis
                        if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim() == "Provisional Diagnosis"
                             && common.myStr(dtTemplate.Rows[i]["TemplateType"]).Trim() == "S"
                            && common.myStr(dtTemplate.Rows[i]["TemplateDate"]) == common.myStr(dtGroupDateWiseTemplate.Rows[iGp]["TemplateDate"]))
                        {
                            sbTemplateStyle = new StringBuilder();
                            DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                            dv.RowFilter = "PageId =" + common.myInt(dtTemplate.Rows[i]["PageId"]).ToString();
                            if (dv.Count > 0)
                            {
                                drTemplateStyle = dv[0].Row;
                                string sBegin = "", sEnd = "";
                                Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                                MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                            }
                            dv.Dispose();
                            sbTemp = new StringBuilder();
                            if ((common.myInt(templateid) == 0)
                                 || (common.myInt(templateid) == 1085))
                            {
                                #region Call Provisional Diagnosis template data
                                BindCaseSheet.BindPatientProvisionalDiagnosis(dsTemplateData.Tables[2],
                                           Request.QueryString["DoctorId"] != null ? Request.QueryString["DoctorId"].ToString() : DoctorId, sbTemp, sbTemplateStyle, drTemplateStyle, Page,
                                           common.myStr(dtTemplate.Rows[i]["PageId"]), common.myStr(Session["UserID"]),
                                            0, common.myInt(ViewState["EncounterId"]), common.myStr(dtGroupDateWiseTemplate.Rows[iGp]["TemplateDate"]), true);
                                #endregion
                                if (sbTemp.ToString() != "")
                                    TemplateString.Append(sbTemp + "<br/>");
                                sbTemp = null;
                                sbTemplateStyle = null;
                            }
                            drTemplateStyle = null;
                            Templinespace = "";
                        }
                        #endregion
                        #region Template Plan of Care
                        if (common.myStr(dtTemplate.Rows[i]["TemplateId"]).Trim() != ""
                             && common.myStr(dtTemplate.Rows[i]["TemplateType"]).Trim() == "D"
                            && common.myStr(dtTemplate.Rows[i]["TemplateCode"]).Trim() == "POC"
                            && common.myStr(dtTemplate.Rows[i]["TemplateDate"]) == common.myStr(dtGroupDateWiseTemplate.Rows[iGp]["TemplateDate"]))
                        {
                            DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                            dv.RowFilter = "PageId =" + common.myInt(dtTemplate.Rows[i]["TemplateId"]).ToString();
                            if (dv.Count > 0)
                            {
                                drTemplateStyle = dv[0].Row;
                                string sBegin = "", sEnd = "";
                                Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                                MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                            }
                            dv.Dispose();
                            sbTemp = new StringBuilder();
                            if ((common.myInt(templateid) == 0)
                                || (common.myInt(templateid) == common.myInt(dtTemplate.Rows[i]["TemplateId"])))
                            {
                                #region Assign Data and call Dynamic Template Plan of Care
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
                                if (common.myInt(templateid) == 0)
                                {
                                    dvDyTable1.RowFilter = "TemplateId=" + common.myStr(dtTemplate.Rows[i]["TemplateId"]);
                                    dvDyTable4.RowFilter = "TemplateId=" + common.myStr(dtTemplate.Rows[i]["TemplateId"]);
                                    dvDyTable6.RowFilter = "TemplateId=" + common.myStr(dtTemplate.Rows[i]["TemplateId"]);
                                    dtDyTempTable = dvDyTable4.ToTable();
                                    dvDyTable4.Sort = "RecordId ASC";
                                }
                                else
                                {
                                    dvDyTable1.RowFilter = "TemplateId=" + common.myStr(templateid);
                                    dvDyTable4.RowFilter = "TemplateId=" + common.myStr(templateid);
                                    dvDyTable6.RowFilter = "TemplateId=" + common.myStr(templateid);
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
                                dvDyTable3.RowFilter = "SectionId IN (" + sSectionId + ") AND GroupDate='" + common.myStr(dtGroupDateWiseTemplate.Rows[iGp]["TemplateDate"]) + "'";
                                dvDyTable4.RowFilter = "GroupDate='" + common.myStr(dtGroupDateWiseTemplate.Rows[iGp]["TemplateDate"]) + "' AND SectionId IN (" + sSectionId + ")";

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
                                    bindData(dsDymanicTemplateData, common.myStr(dtTemplate.Rows[i]["TemplateId"]), sbTemp, common.myStr(dtGroupDateWiseTemplate.Rows[iGp]["TemplateDate"]));
                                    if (sbTemp.Length > 20)
                                        TemplateString.Append(sbTemp + "<br/>");
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
                            }

                            Templinespace = "";
                        }
                        #endregion
                        #region Orders And Procedures
                        if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim() == "Orders And Procedures"
                             && common.myStr(dtTemplate.Rows[i]["TemplateType"]).Trim() == "S"
                            && common.myStr(dtTemplate.Rows[i]["TemplateDate"]) == common.myStr(dtGroupDateWiseTemplate.Rows[iGp]["TemplateDate"]))
                        {
                            sbTemplateStyle = new StringBuilder();
                            DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                            dv.RowFilter = "PageId =" + common.myInt(dtTemplate.Rows[i]["PageId"]).ToString();
                            if (dv.Count > 0)
                            {
                                drTemplateStyle = dv[0].Row;
                                string sBegin = "", sEnd = "";
                                Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                                MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                            }
                            dv.Dispose();
                            sbTemp = new StringBuilder();

                            if ((common.myInt(templateid) == 0)
                                 || (common.myInt(templateid) == 17))
                            {
                                #region Call Bind Order data
                                BindCaseSheet.BindOrders(dsTemplateData.Tables[8], DoctorId, sbTemp, sbTemplateStyle, drTemplateStyle,
                                               Page, common.myStr(dtTemplate.Rows[i]["PageId"]), common.myInt(ViewState["EncounterId"]),
                                               common.myStr(dtGroupDateWiseTemplate.Rows[iGp]["TemplateDate"]), string.Empty, true, true);
                                #endregion


                                if (sbTemp.ToString() != "")
                                    TemplateString.Append(sbTemp + "<br/>");
                                sbTemp = null;
                                sbTemplateStyle = null;
                            }

                            drTemplateStyle = null;
                            Templinespace = "";
                        }
                        #endregion
                        #region Prescription
                        if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim() == "Prescription"
                            && common.myStr(dtTemplate.Rows[i]["TemplateType"]).Trim() == "S"
                            && common.myStr(dtTemplate.Rows[i]["TemplateDate"]) == common.myStr(dtGroupDateWiseTemplate.Rows[iGp]["TemplateDate"]))
                        {
                            sbTemplateStyle = new StringBuilder();
                            DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                            dv.RowFilter = "PageId =" + common.myInt(dtTemplate.Rows[i]["PageId"]).ToString();

                            if (dv.Count > 0)
                            {
                                drTemplateStyle = dv[0].Row;
                                string sBegin = "", sEnd = "";
                                Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                                MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                            }

                            dv.Dispose();
                            sbTemp = new StringBuilder();

                            if ((common.myInt(templateid) == 0)
                                 || (common.myInt(templateid) == 153))
                            {
                                DataSet dsMedication = new DataSet();
                                DataView dvTable1 = new DataView(dsTemplateData.Tables[10]);
                                DataView dvTable2 = new DataView(dsTemplateData.Tables[11]);
                                dvTable1.ToTable().TableName = "Item";
                                dvTable2.ToTable().TableName = "ItemDetail";

                                dsMedication.Tables.Add(dvTable1.ToTable());
                                dsMedication.Tables.Add(dvTable2.ToTable());

                                dvTable1.Dispose();
                                dvTable2.Dispose();

                                #region Call Medication Template data
                                BindCaseSheet.BindMedication(dsMedication, sbTemp, sbTemplateStyle, drTemplateStyle, Page, common.myStr(dtTemplate.Rows[i]["PageId"]),
                                               common.myInt(Session["UserID"]).ToString(), common.myStr(dtGroupDateWiseTemplate.Rows[iGp]["TemplateDate"]), 0, string.Empty, string.Empty, true);
                                #endregion

                                dsMedication.Dispose();
                                if (sbTemp.ToString() != "")
                                    TemplateString.Append(sbTemp + "<br/>");
                                sbTemp = null;
                                sbTemplateStyle = null;
                            }

                            drTemplateStyle = null;
                            Templinespace = "";
                        }
                        #endregion
                        #region Non Drug Order
                        if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim() == "Non Drug Order"
                             && common.myStr(dtTemplate.Rows[i]["TemplateType"]).Trim() == "S"
                            && common.myStr(dtTemplate.Rows[i]["TemplateDate"]) == common.myStr(dtGroupDateWiseTemplate.Rows[iGp]["TemplateDate"]))
                        {
                            sbTemplateStyle = new StringBuilder();
                            DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                            dv.RowFilter = "PageId =" + common.myInt(dtTemplate.Rows[i]["PageId"]).ToString();
                            if (dv.Count > 0)
                            {
                                drTemplateStyle = dv[0].Row;
                                string sBegin = "", sEnd = "";
                                Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                                MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                            }
                            dv.Dispose();
                            sbTemp = new StringBuilder();
                            if ((common.myInt(templateid) == 0)
                                 || (common.myInt(templateid) == 1166))
                            {
                                #region Call Non Drug Order template data
                                BindCaseSheet.BindNonDrugOrder(dsTemplateData.Tables[7], sbTemp, sbTemplateStyle, drTemplateStyle, Page, "0",
                                                  common.myInt(ViewState["EncounterId"]), common.myStr(dtGroupDateWiseTemplate.Rows[iGp]["TemplateDate"]), true);
                                #endregion
                                if (sbTemp.ToString() != "")
                                    TemplateString.Append(sbTemp + "<br/>");
                                sbTemp = null;
                                sbTemplateStyle = null;
                            }
                            drTemplateStyle = null;
                            Templinespace = "";
                        }
                        #endregion
                        #region Diet Order
                        if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim() == "Diet Order"
                             && common.myStr(dtTemplate.Rows[i]["TemplateType"]).Trim() == "S"
                            && common.myStr(dtTemplate.Rows[i]["TemplateDate"]) == common.myStr(dtGroupDateWiseTemplate.Rows[iGp]["TemplateDate"]))
                        {
                            sbTemplateStyle = new StringBuilder();
                            DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                            dv.RowFilter = "PageId =" + common.myInt(dtTemplate.Rows[i]["PageId"]).ToString();

                            if (dv.Count > 0)
                            {
                                drTemplateStyle = dv[0].Row;
                                string sBegin = "", sEnd = "";
                                Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                                MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                            }

                            dv.Dispose();
                            sbTemp = new StringBuilder();

                            if ((common.myInt(templateid) == 0)
                                 || (common.myInt(templateid) == 1172))
                            {
                                #region Call Diet Order data
                                BindCaseSheet.BindDietOrderInNote(dsTemplateData.Tables[9], sbTemp, sbTemplateStyle, drTemplateStyle, Page, "0", common.myStr(dtGroupDateWiseTemplate.Rows[iGp]["TemplateDate"]), common.myInt(ViewState["EncounterId"]));
                                #endregion

                                if (sbTemp.ToString() != "")
                                    TemplateString.Append(sbTemp + "<br/>");
                                sbTemp = null;
                                sbTemplateStyle = null;
                            }

                            drTemplateStyle = null;
                            Templinespace = "";
                        }
                        #endregion
                        #region Doctor Progress Note
                        if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim() == "Doctor Progress Note"
                             && common.myStr(dtTemplate.Rows[i]["TemplateType"]).Trim() == "S"
                            && common.myStr(dtTemplate.Rows[i]["TemplateDate"]) == common.myStr(dtGroupDateWiseTemplate.Rows[iGp]["TemplateDate"]))
                        {
                            sbTemplateStyle = new StringBuilder();
                            DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                            dv.RowFilter = "PageId =" + common.myInt(dtTemplate.Rows[i]["PageId"]).ToString();
                            if (dv.Count > 0)
                            {
                                drTemplateStyle = dv[0].Row;
                                string sBegin = "", sEnd = "";
                                Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                                MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                            }
                            dv.Dispose();
                            sbTemp = new StringBuilder();
                            if ((common.myInt(templateid) == 0)
                                 || (common.myInt(templateid) == 1013))
                            {
                                #region Call Doctor Progress Note template data
                                BindCaseSheet.BindDoctorProgressNote(dsTemplateData.Tables[3], sbTemp, sbTemplateStyle, drTemplateStyle, Page,
                                           common.myStr(dtTemplate.Rows[i]["PageId"]), common.myStr(Session["UserID"]),
                                            common.myStr(dtGroupDateWiseTemplate.Rows[iGp]["TemplateDate"]), common.myInt(ViewState["EncounterId"]), true);
                                #endregion
                                if (sbTemp.ToString() != "")
                                    TemplateString.Append(sbTemp + "<br/>");
                                sbTemp = null;
                                sbTemplateStyle = null;
                            }
                            Templinespace = "";
                        }
                        #endregion
                        #region Referal History
                        if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim() == "Referral History"
                             && common.myStr(dtTemplate.Rows[i]["TemplateType"]).Trim() == "S"
                            && common.myStr(dtTemplate.Rows[i]["TemplateDate"]) == common.myStr(dtGroupDateWiseTemplate.Rows[iGp]["TemplateDate"]))
                        {
                            sbTemplateStyle = new StringBuilder();
                            DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                            dv.RowFilter = "PageId =" + common.myInt(dtTemplate.Rows[i]["PageId"]).ToString();
                            if (dv.Count > 0)
                            {
                                drTemplateStyle = dv[0].Row;
                                string sBegin = "", sEnd = "";
                                Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                                MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                            }
                            dv.Dispose();
                            sbTemp = new StringBuilder();

                            if ((common.myInt(templateid) == 0)
                                 || (common.myInt(templateid) == 1081))
                            {
                                StringBuilder temp1 = new StringBuilder();
                                #region Call Referral History Template Data
                                BindCaseSheet.BindReferalHistory(dsTemplateData.Tables[5], sbTemp, sbTemplateStyle, drTemplateStyle, Page, "0",
                                    common.myInt(ViewState["EncounterId"]), common.myStr(dtGroupDateWiseTemplate.Rows[iGp]["TemplateDate"]));
                                #endregion
                                if (sbTemp.ToString() != "")
                                    TemplateString.Append(sbTemp + "<br/>");
                                sbTemp = null;
                                sbTemplateStyle = null;
                            }
                            Templinespace = "";
                        }
                        #endregion
                        #region Current Medication
                        if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim() == "Current Medication"
                            && common.myStr(dtTemplate.Rows[i]["DataStatus"]).Trim() == "AVAILABLE")
                        {
                            sbTemplateStyle = new StringBuilder();
                            DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                            dv.RowFilter = "PageId =" + common.myInt(dtTemplate.Rows[i]["PageId"]).ToString();
                            if (dv.Count > 0)
                            {
                                drTemplateStyle = dv[0].Row;
                                string sBegin = "", sEnd = "";
                                Templinespace = common.myStr(common.myInt(drTemplateStyle["TemplateSpaceNumber"]) - 1);
                                MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                            }
                            dv.Dispose();
                            sbTemp = new StringBuilder();

                            if ((common.myInt(templateid) == 0)
                                 || (common.myInt(templateid) == 10005))
                            {
                                bnotes.BindMedication(common.myInt(ViewState["EncounterId"]), common.myInt(Session["HospitalLocationID"]), common.myInt(ViewState["RegistrationId"]), sbTemp, sbTemplateStyle, "C", drTemplateStyle,
                                                Page, common.myStr(dtTemplate.Rows[i]["PageId"]), common.myInt(Session["UserID"]).ToString(),
                                                common.myDate("1900/01/01").ToString(),
                                                common.myDate("2059/01/01").ToString(), common.myStr(ViewState["OPIP"]), "");
                                if (sbTemp.ToString() != "")
                                    TemplateString.Append(sbTemp + "<br/>");
                                sbTemp = null;
                                sbTemplateStyle = null;
                            }

                            drTemplateStyle = null;
                            Templinespace = "";
                        }
                        #endregion
                        #region Immunization
                        if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim() == "Immunization"
                          && common.myStr(dtTemplate.Rows[i]["TemplateType"]).Trim() == "S"
                            && common.myStr(dtTemplate.Rows[i]["TemplateDate"]) == common.myStr(dtGroupDateWiseTemplate.Rows[iGp]["TemplateDate"]))
                        {
                            sbTemplateStyle = new StringBuilder();
                            DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                            dv.RowFilter = "PageId =" + common.myInt(dtTemplate.Rows[i]["PageId"]).ToString();
                            if (dv.Count > 0)
                            {
                                drTemplateStyle = dv[0].Row;
                                string sBegin = "", sEnd = "";
                                Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                                MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                            }
                            dv.Dispose();
                            sbTemp = new StringBuilder();

                            if ((common.myInt(templateid) == 0)
                                 || (common.myInt(templateid) == 113))
                            {
                                BindCaseSheet.BindImmunization(dsTemplateData.Tables[13], sbTemp, sbTemplateStyle, drTemplateStyle, Page,
                                            common.myStr(dtTemplate.Rows[i]["PageId"]), common.myStr(dtGroupDateWiseTemplate.Rows[iGp]["TemplateDate"]),
                                            common.myInt(ViewState["EncounterId"]));
                                if (sbTemp.ToString() != "")
                                    TemplateString.Append(sbTemp + "<br/>");
                                sbTemp = null;
                                sbTemplateStyle = null;
                            }

                            Templinespace = "";
                        }
                        #endregion
                        #region Daily Injection
                        if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim() == "Daily Injections"
                            && common.myStr(dtTemplate.Rows[i]["TemplateType"]).Trim() == "S"
                            && common.myStr(dtTemplate.Rows[i]["TemplateDate"]) == common.myStr(dtGroupDateWiseTemplate.Rows[iGp]["TemplateDate"]))
                        {
                            sbTemplateStyle = new StringBuilder();
                            DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                            dv.RowFilter = "PageId =" + common.myInt(dtTemplate.Rows[i]["PageId"]).ToString();
                            if (dv.Count > 0)
                            {
                                drTemplateStyle = dv[0].Row;
                                string sBegin = "", sEnd = "";
                                Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                                MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                            }

                            dv.Dispose();
                            sbTemp = new StringBuilder();

                            if ((common.myInt(templateid) == 0)
                                 || (common.myInt(templateid) == 805))
                            {
                                BindCaseSheet.BindInjection(dsTemplateData.Tables[12], sbTemp, sbTemplateStyle, drTemplateStyle, Page,
                                             common.myStr(dtTemplate.Rows[i]["PageId"]), common.myStr(dtGroupDateWiseTemplate.Rows[iGp]["TemplateDate"]),
                                             common.myInt(ViewState["EncounterId"]));

                                TemplateString.Append(sbTemp + "<br/>");
                                sbTemp = null;
                                sbTemplateStyle = null;
                            }
                            Templinespace = "";
                        }
                        #endregion
                        #region Follow-up Appointment
                        if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim().ToUpper().Equals("FOLLOW UP APPOINTMENT")
                            && common.myStr(dtTemplate.Rows[i]["TemplateType"]).Trim() == "S"
                            && common.myStr(dtTemplate.Rows[i]["TemplateDate"]) == common.myStr(dtGroupDateWiseTemplate.Rows[iGp]["TemplateDate"]))
                        {
                            sbTemplateStyle = new StringBuilder();
                            DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                            dv.RowFilter = "PageId =" + common.myInt(dtTemplate.Rows[i]["PageId"]).ToString();
                            if (dv.Count > 0)
                            {
                                drTemplateStyle = dv[0].Row;
                                string sBegin = "", sEnd = "";
                                Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                                MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                            }
                            dv.Dispose();
                            if ((common.myInt(templateid) == 0)
                                 || (common.myInt(templateid) == 919))
                            {
                                StringBuilder temp = new StringBuilder();
                                #region FollowUp Appointment
                                BindCaseSheet.GetEncounterFollowUpAppointment(dsTemplateData.Tables[6],
                                       temp, sbTemplateStyle, drTemplateStyle, Page, common.myStr(dtGroupDateWiseTemplate.Rows[iGp]["TemplateDate"]),
                                       common.myInt(ViewState["EncounterId"]));
                                #endregion

                                TemplateString.Append(temp);
                                temp = null;
                                sbTemplateStyle = null;
                            }
                            Templinespace = "";
                        }
                        #endregion

                    }
                    if (TemplateString.Length > 30)
                    {
                        sb.Append("<span style='" + String.Empty + "'>");
                        sb.Append("<br/><b><u> Date : " + common.myDate(dtGroupDateWiseTemplate.Rows[iGp]["TemplateDate"]).ToString("dd/MM/yyyy") + "</u></b><br/>");
                        sb.Append(TemplateString);
                        sb.Append("</span>");
                        TemplateString = null;
                    }
                }
            }
            Session["NoAllergyDisplay"] = null;
            if (sign == true)
            {
                sb.Append(hdnDoctorImage.Value);
            }
            else if (sign == false)
            {
                if (RTF1.Content != null)
                {
                    if (RTF1.Content.Contains("dvDoctorImage") == true)
                    {
                        string signData = RTF1.Content.Replace('"', '$');
                        string st = "<div id=$dvDoctorImage$>";
                        int start = signData.IndexOf(@st);
                        if (start > 0)
                        {
                            int End = signData.IndexOf("</div>", start);
                            StringBuilder sbte = new StringBuilder();
                            sbte.Append(signData.Substring(start, (End + 6) - start));
                            StringBuilder ne = new StringBuilder();
                            ne.Append(signData.Replace(sbte.ToString(), ""));
                            sb.Append(ne.Replace('$', '"').ToString());
                            sbte = null;
                            ne = null;
                            signData = "";
                            st = "";
                            start = 0;
                            End = 0;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + ex.Message;
            objException.HandleException(ex);
        }
        finally
        {
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
            dtGroupDateWiseTemplate.Dispose();
            sbTemp = null;
            BindCaseSheet = null;
            dsTemplateData.Dispose();
            sTemplateName = String.Empty;
        }
        return sb.ToString();
    }

    private string BindStaticTemplates(int StaticTemplateId, int TemplateFieldId)
    {
        int RegId = 0;
        int EncounterId = 0;

        StringBuilder sb = new StringBuilder();
        StringBuilder sbStatic = new StringBuilder();
        StringBuilder sbTemplateStyle = new StringBuilder();
        DataSet dsTemplate = new DataSet();
        DataSet dsTemplateStyle = new DataSet();
        DataRow drTemplateStyle = null;
        DataTable dtTemplate = new DataTable();
        Hashtable hst = new Hashtable();
        string Templinespace = "";
        BaseC.DiagnosisDA fun;

        BindNotes bnotes = new BindNotes(sConString);
        fun = new BaseC.DiagnosisDA(sConString);

        string DoctorId = fun.GetDoctorId(common.myInt(Session["HospitalLocationID"]), Convert.ToInt16(common.myInt(Session["UserID"])));

        dsTemplateStyle = bnotes.GetTemplateStyle(common.myInt(Session["HospitalLocationId"]));

        dsTemplate = bnotes.GetEMRTemplates(common.myInt(ViewState["EncounterId"]), common.myInt(ViewState["RegistrationId"]), common.myInt(ViewState["EREncounterId"]).ToString());
        DataView dvFilterStaticTemplate = new DataView(dsTemplate.Tables[0]);
        dvFilterStaticTemplate.RowFilter = "PageId=" + StaticTemplateId;
        dtTemplate = dvFilterStaticTemplate.ToTable();

        sb.Append("<span style='" + string.Empty + "'>");

        if (dtTemplate.Rows.Count > 0)
        {
            if (common.myStr(dtTemplate.Rows[0]["TemplateName"]).Trim() == "Allergies"
                && common.myStr(dtTemplate.Rows[0]["DataStatus"]).Trim() == "AVAILABLE")
            {
                string strTemplateType = common.myStr(dtTemplate.Rows[0]["PageIdentification"]);
                strTemplateType = strTemplateType.Substring(0, 1);
                sbTemplateStyle = new StringBuilder();
                DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                dv.RowFilter = "PageId =" + common.myStr(dtTemplate.Rows[0]["PageId"]);
                drTemplateStyle = null;// = dv[0].Row;
                if (dv.Count > 0)
                {
                    drTemplateStyle = dv[0].Row;
                    string sBegin = "", sEnd = "";
                    Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                    MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                }
                StringBuilder sbTemp = new StringBuilder();


                bnotes.BindAllergies(common.myInt(ViewState["RegistrationId"]), sbStatic, sbTemplateStyle, drTemplateStyle, Page, common.myInt(Session["HospitalLocationId"]).ToString(),
                            common.myInt(Session["UserID"]).ToString(), common.myStr(dtTemplate.Rows[0]["PageID"]),
                            common.myDate("1900/01/01").ToString(),
                            common.myDate("2100/01/01").ToString(), TemplateFieldId, "");

                // sb.Append(sbTemp + "<br/>");


                drTemplateStyle = null;
                Templinespace = "";
            }
            else if (common.myStr(dtTemplate.Rows[0]["TemplateName"]).Trim() == "Vitals"
                && common.myStr(dtTemplate.Rows[0]["DataStatus"]).Trim() == "AVAILABLE")
            {
                string strTemplateType = common.myStr(dtTemplate.Rows[0]["PageIdentification"]);
                strTemplateType = strTemplateType.Substring(0, 1);
                sbTemplateStyle = new StringBuilder();
                DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                dv.RowFilter = "PageId =" + common.myStr(dtTemplate.Rows[0]["PageId"]);
                if (dv.Count > 0)
                {
                    drTemplateStyle = dv[0].Row;
                    string sBegin = "", sEnd = "";
                    Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                    MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                }
                StringBuilder sbTemp = new StringBuilder();


                bnotes.BindVitals(common.myInt(Session["HospitalLocationID"]).ToString(), EncounterId, sbStatic, sbTemplateStyle, drTemplateStyle,
                                    Page, common.myStr(dtTemplate.Rows[0]["PageId"]), common.myInt(Session["UserID"]).ToString(),
                                    common.myDate("1900/01/01").ToString(),
                                    common.myDate("2100/01/01").ToString(), TemplateFieldId, common.myInt(ViewState["EREncounterId"]).ToString(), "");

                //sb.Append(sbTemp + "<br/>" + "<br/>");


                drTemplateStyle = null;
                Templinespace = "";

            }

            else if (common.myStr(dtTemplate.Rows[0]["TemplateName"]).Trim() == "Diagnosis"
                && common.myStr(dtTemplate.Rows[0]["DataStatus"]).Trim() == "AVAILABLE")
            {
                string strTemplateType = common.myStr(dtTemplate.Rows[0]["PageIdentification"]);
                strTemplateType = strTemplateType.Substring(0, 1);
                sbTemplateStyle = new StringBuilder();
                DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                dv.RowFilter = "PageId =" + common.myStr(dtTemplate.Rows[0]["PageId"]);
                if (dv.Count > 0)
                {
                    drTemplateStyle = dv[0].Row;
                    string sBegin = "", sEnd = "";
                    Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                    MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                }
                StringBuilder sbTemp = new StringBuilder();


                bnotes.BindAssessments(common.myInt(ViewState["RegistrationId"]), common.myInt(Session["HospitalLocationID"]), common.myInt(ViewState["EncounterId"]), Convert.ToInt16(common.myInt(Session["UserID"])),
                            DoctorId, sbStatic, sbTemplateStyle, drTemplateStyle, Page,
                            common.myStr(dtTemplate.Rows[0]["PageId"]), common.myInt(Session["UserID"]).ToString(),
                            common.myDate("1900/01/01").ToString(),
                            common.myDate("2100/01/01").ToString(), TemplateFieldId, common.myInt(ViewState["EREncounterId"]).ToString(), "");

                //sb.Append(sbTemp + "<br/>");

                drTemplateStyle = null;
                Templinespace = "";
            }
            //sb.Append("</span>");
        }
        return "<br/>" + sbStatic.ToString();
    }

    private string BindNonTabularImageTypeFieldValueTemplates(DataTable dtIMTypeTemplate)
    {
        StringBuilder sb = new StringBuilder();
        if (dtIMTypeTemplate.Rows.Count > 0)
        {
            if (common.myStr(dtIMTypeTemplate.Rows[0]["ImagePath"]) != "")
            {
                sb.Append("<table id='dvImageType' runat='server' ><tr align='left'><td align='center'><img src='" + dtIMTypeTemplate.Rows[0]["ImagePath"].ToString() + "' width='80px' height='80px' border='0' align='left' alt='Image' /></td></tr></table>");
            }
        }
        return sb.ToString();
    }

    //bool GetStatus()
    //{
    //    DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
    //    DataSet dsStatusId = new DataSet();
    //    int StatusId = 0;
    //    Hashtable hstInput = new Hashtable();

    //    hstInput.Add("@intencounterid", common.myInt(ViewState["EncounterId"]));
    //    hstInput.Add("@intFormId", common.myInt(Session["formId"]));
    //    string sql = "Select StatusId, FormName from EMRPatientForms epf inner join EMRForms ef on  epf.FormId = ef.Id"
    //        + " where EncounterId = @intencounterid"
    //        + " And FormId = @intFormId AND epf.Active=1"
    //        + " Select StatusId, Status, Code From GetStatus(1,'Notes') where Code='P'"
    //        + " Select StatusId, Status, Code From GetStatus(1,'Notes') where Code='S'"
    //        + " select s.code from encounter e inner join statusmaster s on e.statusid = s.statusid where e.id = @intencounterid and e.active = 1 ";

    //    dsStatusId = dl.FillDataSet(CommandType.Text, sql, hstInput);
    //    if (dsStatusId.Tables[1].Rows.Count > 0)
    //    {
    //        hdnUnSignedId.Value = common.myStr(dsStatusId.Tables[1].Rows[0]["StatusId"]);
    //    }
    //    if (dsStatusId.Tables[2].Rows.Count > 0)
    //    {
    //        hdnSignedId.Value = common.myStr(dsStatusId.Tables[2].Rows[0]["StatusId"]);
    //    }
    //    if (dsStatusId.Tables[0].Rows.Count > 0)
    //    {
    //        //lblFormName.Text = common.myStr(dsStatusId.Tables[0].Rows[0]["FormName"]);
    //        StatusId = common.myInt(dsStatusId.Tables[0].Rows[0]["StatusId"]);
    //        ViewState["StatusID"] = StatusId;
    //    }
    //    if (dsStatusId.Tables[3].Rows.Count > 0)
    //    {
    //        if (common.myStr(dsStatusId.Tables[3].Rows[0]["Code"]).Trim() == "S")
    //        {
    //            btnSeenByDoctor.Text = "Patient UnSeen";
    //        }
    //        else if (common.myStr(dsStatusId.Tables[3].Rows[0]["Code"]).Trim() == "O"
    //            || common.myStr(dsStatusId.Tables[3].Rows[0]["Code"]).Trim() == "VT")
    //        {
    //            btnSeenByDoctor.Text = "Patient Seen";
    //        }
    //        else
    //        {
    //            btnSeenByDoctor.Visible = false;
    //        }
    //    }

    //    if (StatusId == common.myInt(hdnSignedId.Value))
    //    {
    //        btnSentenceGallery.Enabled = false;
    //        btnSave.Enabled = false;
    //        btnSigned.Text = "UnSign";
    //        btnSeenByDoctor.Enabled = false;
    //        btnAddendum.Enabled = true;
    //        //chkUpdateTempData.Enabled = false;

    //        RTF1.EditModes = Telerik.Web.UI.EditModes.Preview;

    //        return true;
    //    }
    //    else if (StatusId == common.myInt(hdnUnSignedId.Value))
    //    {
    //        btnSigned.Text = "Sign";
    //        btnSentenceGallery.Enabled = true;
    //        btnSave.Enabled = true;
    //        btnAddendum.Enabled = false;
    //        btnSeenByDoctor.Enabled = true;
    //        //chkUpdateTempData.Enabled = true;
    //        // RTF1.EditModes = EditModes.Design;
    //        return false;
    //    }
    //    else
    //    {
    //        btnSigned.Text = "Sign";
    //        btnSentenceGallery.Enabled = true;
    //        btnSave.Enabled = true;
    //        btnAddendum.Enabled = false;
    //        btnSeenByDoctor.Enabled = true;
    //        //chkUpdateTempData.Enabled = true;
    //        //RTF1.EditModes = EditModes.Design;
    //        return false;
    //    }


    //}

    protected void btnPullForward_Click(object sender, EventArgs e)
    {

    }

    protected void btnDefaultTemplate_Click(object sender, EventArgs e)
    {

    }

    protected void btnOldNotes_Click(object sender, EventArgs e)
    {
        SaveNoFinilizedNote();

        RadWindow3.NavigateUrl = "/Emr/Letters/OldForm.aspx";
        RadWindow3.Height = 600;
        RadWindow3.Width = 900;
        RadWindow3.Top = 20;
        RadWindow3.Left = 20;
        // RadWindowForNew.Title = "Time Slot";
        RadWindow3.OnClientClose = "OnClientClose";
        RadWindow3.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindow3.Modal = true;
        RadWindow3.VisibleStatusbar = false;
    }

    //protected void btnSigned_OnClick(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        ViewState["SignStatus"] = true;
    //        String StrEncode = "";
    //        if (common.myStr(btnSigned.Text).Trim() == "Sign")
    //        {
    //            StrEncode = RTF1.Content + common.myStr(hdnDoctorImage.Value);
    //            StrEncode = StrEncode.Replace("'", "\"");
    //        }
    //        else
    //        {
    //            string t = RTF1.Content;
    //            string Ttype = "dvDoctorImage";
    //            t = t.Replace('"', '$');
    //            string st = "<div id=$" + Ttype + "$>";
    //            int RosSt = t.IndexOf(st);
    //            if (RosSt > 0 || RosSt == 0)
    //            {
    //                int RosEnd = t.IndexOf("</div>", RosSt);
    //                string str = t.Substring(RosSt, (RosEnd) - RosSt);
    //                string ne = t.Replace(str, "");
    //                t = ne.Replace('$', '"');
    //            }
    //            StrEncode = t;
    //            StrEncode = StrEncode.Replace('$', '"');
    //        }
    //        BindNotes bnotes = new BindNotes(sConString);
    //        BaseC.EMR emr = new BaseC.EMR(sConString);
    //        Hashtable hshOut = emr.SavePatientSignData(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]),
    //                            common.myInt(ViewState["RegistrationId"]), btnSigned.Text.Trim() == "Sign" ? true : false,
    //                            common.myInt(ViewState["DoctorId"]), StrEncode, common.myInt(ViewState["EncounterId"]), common.myInt(Session["UserId"]));

    //        DataSet ds = bnotes.GetPatientEMRData(common.myInt(ViewState["EncounterId"]));
    //        if (ds.Tables[0].Rows.Count > 0)
    //        {
    //            RTF1.Content = common.myStr(ds.Tables[0].Rows[0]["PatientSummary"]);
    //        }
    //        lblMessage.Text = common.myStr(hshOut["@chvErrorStatus"]);
    //        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);

    //      //  GetStatus();

    //        bindDetails();

    //        if (Convert.ToBoolean(hshOut["@bitSignedNote"]) == true)
    //        {
    //            RTF1.EditModes = EditModes.Preview;
    //            Session["EncounterStatus"] = "CLOSE";
    //            btnSigned.Text = "Unsign";
    //        }
    //        else
    //        {
    //            //RTF1.EditModes = EditModes.Design;
    //            Session["EncounterStatus"] = "OPEN";
    //            btnSigned.Text = "Sign";
    //        }

    //    }
    //    catch (Exception ex)
    //    {
    //        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
    //        lblMessage.Text = "Error: " + ex.Message;
    //        objException.HandleException(ex);
    //    }
    //}

    protected void AddStr1(string type, string Saved_RTF_Content, StringBuilder sbTemp, string Lock)
    {
        sbTemp.Append("<div id='" + type + "'><span style='color: #000000;'>");
    }

    protected void AddStr2(string type, ref string Saved_RTF_Content, StringBuilder sbTemp, string Lock, string Linespace, string ShowNote)
    {
        StringBuilder sb = new StringBuilder();
        sbTemp.Append("</span></div>");
        if (common.myStr(sbTemp).Length > 49)
        {
            if (Linespace != "")
            {
                int ls = common.myInt(Linespace);
                for (int i = 1; i <= ls; i++)
                {
                    sbTemp.Append("<br/>");
                }
            }
            else
            {
                sbTemp.Append("<br />");
            }
        }
        if (Saved_RTF_Content == "" || Saved_RTF_Content == null)
        {
            if (common.myStr(sbTemp).Length > 62)  //if (sbTemp.ToString().Length > 68)
            {
                sb.Append(common.myStr(sbTemp));
            }
        }
        else
        {
            //change
            Saved_RTF_Content += common.myStr(sbTemp);

            if (type != "LAB")
            {
                if (ShowNote == "True"
                && (ViewState["PullForward"] == null || ViewState["DefaultTemplate"] == null))
                {
                    Replace(type, ref Saved_RTF_Content, sbTemp.ToString(), Lock);
                }
                else if (ViewState["PullForward"] == null || ViewState["DefaultTemplate"] == null)
                {
                    Replace(type, ref Saved_RTF_Content, sbTemp.ToString(), Lock);
                }
                else if (common.myStr(ViewState["DefaultTemplate"]) != "")
                {
                    if (common.myStr(ViewState["DefaultTemplate"]).ToUpper() == "TRUE")
                    {
                        Replace(type, ref Saved_RTF_Content, sbTemp.ToString(), Lock);
                    }
                }
                else if (common.myStr(ViewState["PullForward"]) != "")
                {
                    if (common.myStr(ViewState["PullForward"]).ToUpper() == "TRUE")
                    {
                        Replace(type, ref Saved_RTF_Content, sbTemp.ToString(), Lock);
                    }
                }
            }
        }

    }

    protected string GetTemplateId(string TemplateName, int HospitalLocationId)
    {
        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

        string sqlQ = "Select Id from EMRTemplate where HospitalLocationId=@HospitalLocationId and templateName like @TemplateName";
        Hashtable hs = new Hashtable();
        hs.Add("@TemplateName", TemplateName);
        hs.Add("@HospitalLocationId", HospitalLocationId);
        Object templateId = dl.ExecuteScalar(CommandType.Text, sqlQ, hs);
        return templateId.ToString();
    }

    protected void Replace(string Ttype, ref string t, string strNew, string Lock)
    {
        if (t != null)
        {
            t = t.Replace('"', '$');
            string st = "<div id=$" + Ttype + "$>";
            int RosSt = t.IndexOf(st);
            if (RosSt > 0 || RosSt == 0)
            {
                int RosEnd = t.IndexOf("</div>", RosSt);
                string str = t.Substring(RosSt, (RosEnd) - RosSt);
                string ne = t.Replace(str, strNew);
                t = ne.Replace('$', '"');
            }
            else
            {
            }
        }
        t = t.Replace('$', '"');
    }

    private void SaveNoFinilizedNote()
    {
        Hashtable hstInput = new Hashtable();
        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

        if (ViewState["StatusID"] != null)
        {
            if (RTF1.Content != ""
                && RTF1.Content != null
                && common.myInt(ViewState["StatusID"]) == 20)
            {
                string strCheckValue = "select RegistrationId, EncounterId from EMRPatientForms where RegistrationId=" + common.myInt(ViewState["RegistrationId"]).ToString() +
                                       " and EncounterId=" + common.myInt(ViewState["EncounterId"]) +
                                       " and FormId=" + common.myInt(Session["formId"]);

                DataSet ds = new DataSet();

                hstInput.Add("@RegistrationId", common.myInt(ViewState["RegistrationId"]));

                hstInput.Add("@PatientSummary", RTF1.Content);
                hstInput.Add("@EncounterId", common.myInt(ViewState["EncounterId"]));
                hstInput.Add("@FormId", common.myInt(Session["formId"]));
                hstInput.Add("@EncodedBy", common.myInt(Session["UserID"]));

                ds = dl.FillDataSet(CommandType.Text, strCheckValue);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string SqlUpdate = " Update EMRPatientForms Set PatientSummary=@PatientSummary "
                                  + " Where RegistrationId=@RegistrationId And EncounterId = @EncounterId And FormId= @FormId";
                    dl.ExecuteNonQuery(CommandType.Text, SqlUpdate, hstInput);
                }
                else
                {
                    string SqlInsert = "INSERT into EMRPatientForms(RegistrationId,EncounterId,FormId,Priority,StatusId,Active,PatientSummary,EncodedBy,EncodedDate) VALUES(@RegistrationId,@EncounterId,@FormId,0,20,'True',@PatientSummary,@EncodedBy,GETUTCDATE())";
                    dl.ExecuteNonQuery(CommandType.Text, SqlInsert, hstInput);
                }
            }
        }
    }

    //protected void btnSave_Click(object sender, EventArgs e)
    //{
    //    DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

    //    Hashtable hstInput = new Hashtable();
    //    String StrEncode = "";

    //    if (common.myBool(ViewState["SignStatus"]))
    //    {
    //        if (btnSigned.Text.ToString().Trim() == "Sign")
    //        {
    //            StrEncode = RTF1.Content + hdnDoctorImage.Value.ToString();
    //            StrEncode = StrEncode.Replace('$', '"');
    //        }
    //        else
    //        {
    //            string t = RTF1.Content;
    //            string Ttype = "dvDoctorImage";
    //            t = t.Replace('"', '$');
    //            //if (Lock == "0")
    //            //{
    //            string st = "<div id=$" + Ttype + "$>";
    //            int RosSt = t.IndexOf(st);
    //            if (RosSt > 0 || RosSt == 0)
    //            {
    //                int RosEnd = t.IndexOf("</div>", RosSt);
    //                string str = t.Substring(RosSt, (RosEnd) - RosSt);
    //                string ne = t.Replace(str, "");
    //                t = ne.Replace('$', '"');
    //            }
    //            StrEncode = t;
    //            StrEncode = StrEncode.Replace('$', '"');
    //        }
    //    }
    //    else
    //    {
    //        StrEncode = RTF1.Content;
    //        StrEncode = StrEncode.Replace('$', '"');
    //    }


    //    RTF1.Content = StrEncode.Replace("'", @"""");
    //    hstInput.Add("@PatientSummary", RTF1.Content);
    //    hstInput.Add("@RegistrationId", common.myInt(ViewState["RegistrationId"]));
    //    hstInput.Add("@EncounterId", common.myInt(ViewState["EncounterId"]));

    //    string SqlQry = " Update EMRPatientForms Set PatientSummary=@PatientSummary , PdfNote=CONVERT(varbinary(max),@PatientSummary)"
    //                  + " Where RegistrationId=@RegistrationId And EncounterId = @EncounterId";
    //    int temp = dl.ExecuteNonQuery(CommandType.Text, SqlQry, hstInput);
    //    if (temp == 0)
    //    {
    //        lblMessage.Text = "&nbsp;Saved&nbsp;";
    //    }
    //}

    public StringBuilder BindProblemsROS(int HospitalId, int EncounterId, StringBuilder sb, String sDisplayName, String sTemplateName, string pageID)
    {
        DataSet ds;
        DataSet dsGender;
        string strGender = "He";
        Hashtable hstInput = new Hashtable();
        Hashtable hsGender = new Hashtable();
        Hashtable hsProblems = new Hashtable();
        DataTable dtPositiveRos = new DataTable();
        DataTable dtNegativeRos = new DataTable();
        StringBuilder objStrTmp = new StringBuilder();
        DAL.DAL DlObj = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

        //rafat
        hstInput.Add("@inyHospitalLocationID", common.myInt(Session["HospitalLocationID"]));
        //hstInput.Add("@intTemplateId", GetTemplateId("ROS", common.myInt(Session["HospitalLocationID"])));
        hstInput.Add("@intTemplateId", GetTemplateId(sTemplateName, common.myInt(Session["HospitalLocationID"])));

        hstInput.Add("chrGenderType", getGenderValue(common.myInt(Session["Gender"])));

        hstInput.Add("@intFormId", common.myInt(Session["formId"]));
        DataSet dsFont = DlObj.FillDataSet(CommandType.StoredProcedure, "UspEMRGetSectionTreeForPatientsForms", hstInput);
        //string BeginList = "", EndList = "", BeginList2 = "", EndList2 = "", BeginList3 = "", EndList3 = "";
        DataRow drFont = dsFont.Tables[0].Rows[0] as DataRow;


        hsGender.Add("@intRegistrationId", common.myInt(ViewState["RegistrationId"]));
        string SqlQry = " Select dbo.GetGender(GENDER) as 'Gender' from registration where Id = @intRegistrationId";
        dsGender = DlObj.FillDataSet(CommandType.Text, SqlQry, hsGender);
        if (dsGender.Tables[0].Rows.Count > 0)
        {
            if (common.myStr(dsGender.Tables[0].Rows[0]["Gender"]) == "Male")
            {
                strGender = "He";
            }
            else
            {
                strGender = "She";
            }
        }

        //Review Of Systems

        hsProblems.Add("@intEncounterId", EncounterId);
        //hsProblems.Add("@intTemplateId", GetTemplateId("ROS", common.myInt(Session["HospitalLocationID"])));
        hsProblems.Add("@intTemplateId", GetTemplateId(sTemplateName, common.myInt(Session["HospitalLocationID"])));
        ds = new DataSet();
        ds = DlObj.FillDataSet(CommandType.StoredProcedure, "UspEMRGetPatientROS", hsProblems);

        if (ds.Tables[0].Rows.Count > 0)
        {
            DataView dv1 = new DataView(ds.Tables[0]);
            dv1.RowFilter = "PositiveValue <> ''";
            dtPositiveRos = dv1.ToTable();

            DataView dv2 = new DataView(ds.Tables[0]);
            dv2.RowFilter = "NegativeValue <> ''";
            dtNegativeRos = dv2.ToTable();
            //Make font start

            if (common.myStr(drFont["TemplateBold"]) != ""
                || common.myStr(drFont["TemplateItalic"]) != ""
                || common.myStr(drFont["TemplateUnderline"]) != ""
                || common.myStr(drFont["TemplateFontSize"]) != ""
                || common.myStr(drFont["TemplateForecolor"]) != ""
                || common.myStr(drFont["TemplateListStyle"]) != "")
            {
                string sBegin = "", sEnd = "";
                MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drFont);
                if (Convert.ToBoolean(drFont["TemplateDisplayTitle"]))
                {
                    //objStrTmp.Append(sBegin + drFont["TemplateName"].ToString() + sEnd);
                    objStrTmp.Append(sBegin + sDisplayName.ToString() + sEnd);
                }
                //objStrTmp.Append("<br /><Strong>Positive Symptoms:</Strong>");
                //objStrTmp.Append("<br />" + sBegin + "Positive Symptoms:" + sEnd);
            }
            else
            {
                if (Convert.ToBoolean(drFont["TemplateDisplayTitle"]))
                {
                    //objStrTmp.Append(drFont["TemplateName"].ToString());//Default Setting
                    objStrTmp.Append(sDisplayName.ToString());//Default Setting
                }
                //objStrTmp.Append("<br /><Strong>Positive Symptoms:</Strong>");
            }

            // Make Font End

            //sb.Append("<u><Strong>Review of systems</Strong></u>");

        }
        // For Positive Symptoms
        if (dtPositiveRos.Rows.Count > 0)
        {
            string strSectionId = ""; // dtPositiveRos.Rows[0]["SectionId"].ToString();
            DataTable dt = new DataTable();
            for (int i = 0; i < dtPositiveRos.Rows.Count; i++)
            {

                DataRow dr = dtPositiveRos.Rows[i] as DataRow;
                if (common.myStr(dr["SectionId"]) != strSectionId)
                {
                    string sBegin = "", sEnd = "";
                    if (common.myStr(drFont["SectionsBold"]) != ""
                        || common.myStr(drFont["SectionsItalic"]) != ""
                        || common.myStr(drFont["SectionsUnderline"]) != ""
                        || common.myStr(drFont["SectionsFontSize"]) != ""
                        || common.myStr(drFont["SectionsForecolor"]) != ""
                        || common.myStr(drFont["SectionsListStyle"]) != "")
                    {
                        MakeFontWithoutListStyle("Sections", ref sBegin, ref sEnd, drFont);
                        if (Convert.ToBoolean(drFont["SectionDisplayTitle"]))   //19June2010
                        {
                            if (i == 0)
                            {
                                objStrTmp.Append("<br />" + sBegin + "Positive Symptoms:" + sEnd);
                            }
                            objStrTmp.Append("<br />" + sBegin + common.myStr(dr["SectionName"]).Trim() + ": " + sEnd);
                        }
                    }
                    else
                    {
                        if (i == 0)
                        {
                            objStrTmp.Append("<br />" + "Positive Symptoms:");
                        }
                        objStrTmp.Append("<br />" + common.myStr(dr["SectionName"]).Trim() + ": ");
                    }

                    if (common.myStr(dr["FieldsBold"]) != ""
                        || common.myStr(dr["FieldsItalic"]) != ""
                        || common.myStr(dr["FieldsUnderline"]) != ""
                        || common.myStr(dr["FieldsFontSize"]) != ""
                        || common.myStr(dr["FieldsForecolor"]) != ""
                        || common.myStr(dr["FieldsListStyle"]) != "")
                    {
                        sBegin = ""; sEnd = "";
                        MakeFontWithoutListStyle("Fields", ref sBegin, ref sEnd, dr);
                        objStrTmp.Append(sBegin + strGender + " has ");
                    }
                    else
                    {
                        objStrTmp.Append(strGender + " has ");
                    }

                    //sb.Append("<br />" + dr["SectionName"].ToString().ToUpper() + ": " + strGender + " has ");
                    strSectionId = common.myStr(dr["SectionId"]);
                    DataView dv = new DataView(dtPositiveRos);
                    dv.RowFilter = "SectionId =" + common.myStr(dr["SectionId"]);
                    dt = dv.ToTable();

                    for (int j = 0; j < dt.Rows.Count; j++)
                    {
                        if (j == (dt.Rows.Count - 1))
                        {
                            if (dt.Rows.Count == 1)
                            {
                                objStrTmp.Append("" + common.myStr(dt.Rows[j]["PositiveValue"]).ToLower().Trim() + ". ");
                            }
                            else
                            {
                                objStrTmp.Remove(objStrTmp.Length - 2, 2);
                                objStrTmp.Append(" and " + common.myStr(dt.Rows[j]["PositiveValue"]).ToLower().Trim() + ". ");
                            }
                        }
                        else
                        {
                            objStrTmp.Append(common.myStr(dt.Rows[j]["PositiveValue"]).ToLower().Trim() + ", ");
                        }
                    }
                    objStrTmp.Append(sEnd);
                }
            }
        }

        // For Negative Symptoms
        if (dtNegativeRos.Rows.Count > 0)
        {
            //if (drFont["TemplateBold"].ToString() != "" || drFont["TemplateItalic"].ToString() != "" || drFont["TemplateUnderline"].ToString() != "" || drFont["TemplateFontSize"].ToString() != "" || drFont["TemplateForecolor"].ToString() != "" || drFont["TemplateListStyle"].ToString() != "")
            //{
            //    string sBegin = "", sEnd = "";
            //    MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drFont);
            //    //objStrTmp.Append("<br /><br />" + sBegin + "Negative Symptoms:" + sEnd);
            //    //objStrTmp.Append("<br /><br /><Strong>Negative Symptoms:</Strong>");
            //}
            //else
            //{
            //    objStrTmp.Append("<br /><br /><Strong>Negative Symptoms:</Strong>");
            //}          
            string strSectionId = ""; // 
            DataTable dt = new DataTable();
            for (int i = 0; i < dtNegativeRos.Rows.Count; i++)
            {

                DataRow dr = dtNegativeRos.Rows[i] as DataRow;
                if (common.myStr(dr["SectionId"]) != strSectionId)
                {
                    string sBegin = "", sEnd = "";
                    if (common.myStr(drFont["SectionsBold"]) != ""
                        || common.myStr(drFont["SectionsItalic"]) != ""
                        || common.myStr(drFont["SectionsUnderline"]) != ""
                        || common.myStr(drFont["SectionsFontSize"]) != ""
                        || common.myStr(drFont["SectionsForecolor"]) != ""
                        || common.myStr(drFont["SectionsListStyle"]) != "")
                    {

                        MakeFontWithoutListStyle("Sections", ref sBegin, ref sEnd, drFont);
                        if (Convert.ToBoolean(drFont["SectionDisplayTitle"]))   //19June2010
                        {
                            if (i == 0)
                            {
                                objStrTmp.Append("<br /><br />" + sBegin + "Negative Symptoms:" + sEnd);
                            }
                            objStrTmp.Append("<br />" + sBegin + common.myStr(dr["SectionName"]).Trim() + ": " + sEnd);
                        }
                    }
                    else
                    {
                        if (i == 0)
                        {
                            objStrTmp.Append("<br />" + "Negative Symptoms:");
                        }
                        objStrTmp.Append("<br />" + common.myStr(dr["SectionName"]).Trim() + ": ");
                    }


                    if (common.myStr(dr["FieldsBold"]) != ""
                        || common.myStr(dr["FieldsItalic"]) != ""
                        || common.myStr(dr["FieldsUnderline"]) != ""
                        || common.myStr(dr["FieldsFontSize"]) != ""
                        || common.myStr(dr["FieldsForecolor"]) != ""
                        || common.myStr(dr["FieldsListStyle"]) != "")
                    {
                        sBegin = ""; sEnd = "";
                        MakeFontWithoutListStyle("Fields", ref sBegin, ref sEnd, dr);
                        objStrTmp.Append(sBegin + strGender + " does not have ");
                    }
                    else
                    {
                        objStrTmp.Append(strGender + " does not have ");
                    }

                    //sb.Append("<br />" + dr["SectionName"].ToString().ToUpper() + ": " + strGender + " does not have ");
                    strSectionId = common.myStr(dr["SectionId"]);
                    DataView dv = new DataView(dtNegativeRos);
                    dv.RowFilter = "SectionId =" + common.myStr(dr["SectionId"]);
                    dt = dv.ToTable();
                    for (int j = 0; j < dt.Rows.Count; j++)
                    {

                        if (j == dt.Rows.Count - 1)
                        {
                            if (dt.Rows.Count == 1)
                            {
                                objStrTmp.Append("" + common.myStr(dt.Rows[j]["NegativeValue"]).ToLower().Trim() + ". ");
                            }
                            else
                            {
                                objStrTmp.Remove(objStrTmp.Length - 2, 2);
                                objStrTmp.Append(" or " + common.myStr(dt.Rows[j]["NegativeValue"]).ToLower().Trim() + ". ");
                            }
                        }
                        else
                        {
                            objStrTmp.Append(common.myStr(dt.Rows[j]["NegativeValue"]).ToLower().Trim() + ", ");
                        }
                    }
                    objStrTmp.Append(sEnd);
                }
            }
        }
        sb.Append(objStrTmp);
        //sb.Append("<br/>");
        return sb;
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

    protected string CreateString(DataSet objDs, int iRootId, string iRootName, string TabularType, string sectionId, string EntryType, int RecordId, string GroupingDate)
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
                #region Tabular
                if (bool.Parse(TabularType) == true)
                {
                    DataView dvFilter = new DataView(objDs.Tables[0]);
                    if (objDs.Tables[0].Rows.Count > 0)
                    {
                        string sBegin = string.Empty;
                        string sEnd = string.Empty;
                        dvFilter.Sort = "RowNum ASC";
                        dvFilter.RowFilter = "ISNULL(RowCaptionName,'')='' AND RowNum > 2 AND RecordId<>0 AND GroupDate='" + GroupingDate + "' AND RecordId= " + RecordId;
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
                                    dvData.RowFilter = "RecordId=" + RecordId + " AND GroupDate='" + GroupingDate + "'";

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
                            dvRowCaption.RowFilter = "GroupDate='" + GroupingDate + "' AND RecordId= " + RecordId;
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
                                    dvData.RowFilter = "RecordId=" + RecordId + " AND RowCaptionId>0 AND GroupDate='" + GroupingDate + "'";

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
                                sEnd = "";
                                MakeFont("Fields", ref sBegin, ref sEnd, item);
                                if (common.myBool(item["DisplayTitle"]))
                                {
                                    // if (EntryType != "M")
                                    // {
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
                                        objStr.Append(sEnd + "</li>");
                                    }
                                }
                                BeginList = "";
                                sBegin = "";
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
                            for (int i = 0; i < objDv.ToTable().Rows.Count; i++)
                            {
                                if (objDt.Rows.Count > 0)
                                {
                                    string FType = common.myStr(dtFieldType.Rows[0]["FieldType"]);
                                    if (FType == "C")
                                    {
                                        FType = "C";
                                    }
                                    if (FType == "C" || FType == "D" || FType == "B" || FType == "R")
                                    {
                                        if (FType == "B")
                                        {
                                            objStr.Append(" " + objDt.Rows[i]["TextValue"]);
                                        }
                                        else
                                        {
                                            BindDataValue(objDs, objDt, objStr, i, FType);
                                        }
                                    }
                                    else if (FType == "T" || FType == "M" || FType == "S" || FType == "W")
                                    {
                                        if (common.myStr(ViewState["iTemplateId"]) != "163")
                                        {
                                            if (i == 0)
                                            {
                                                objStr.Append(" " + common.myStr(objDt.Rows[i]["TextValue"]));
                                            }
                                            else
                                            {
                                                objStr.Append(", " + common.myStr(objDt.Rows[i]["TextValue"]));
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
                                        objStr.Append(BindStaticTemplates(common.myInt(objDt.Rows[0]["StaticTemplateId"]), common.myInt(objDt.Rows[0]["FieldId"])));
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
                                                objStr.Append("<br />");
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
                                //    objStr.Append("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style='font-size:10px'>(Entered By: " + sEnterBy + " Date/Time: " + sVisitDate + ")</span>");
                                //}
                            }
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

                string sDisplayEnteredBy = common.myStr(Session["DisplayEnteredByInCaseSheet"]);
                if ((sDisplayEnteredBy == "Y") || (sDisplayEnteredBy == "N" && common.myStr(HttpContext.Current.Session["OPIP"]) == "I"))
                {
                    if (objStr.ToString() != "" && bool.Parse(TabularType) == false)
                    {
                        DataView dvValues = new DataView(objDs.Tables[1]);
                        dvValues.RowFilter = "SectionId=" + common.myStr(sectionId);
                        if (dvValues.ToTable().Rows.Count > 0)
                        {
                            objStr.Append("<br/>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style='font-size:10px'>(Entered By: " + common.myStr(dvValues.ToTable().Rows[0]["EnterBy"]) + " Date/Time: " + common.myStr(dvValues.ToTable().Rows[0]["VisitDateTime"]) + ")</span>");
                        }
                        dvValues.Dispose();
                    }
                    else
                    {
                        if (objStr.ToString() != "" && bool.Parse(TabularType) == true)
                        {
                            DataView dvValues = new DataView(objDs.Tables[0]);
                            dvValues.RowFilter = "SectionId=" + common.myStr(sectionId) + " AND RecordId=" + RecordId + " AND IsData='D'";
                            if (dvValues.ToTable().Rows.Count > 0)
                            {
                                objStr.Append("<br/>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style='font-size:10px'>(Entered By: " + common.myStr(dvValues.ToTable().Rows[0]["EnterBy"]) + " Date/Time: " + common.myStr(dvValues.ToTable().Rows[0]["EntryDate"]) + ")</span>");
                            }
                            dvValues.Dispose();
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
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

    protected void BindDataValue(DataSet objDs, DataTable objDt, StringBuilder objStr, int i, string FType)
    {
        if (i == 0)
        {
            objStr.Append(" " + common.myStr(objDt.Rows[i]["TextValue"]));
        }
        else
        {
            if (FType != "C")
            {
                objStr.Append(", " + common.myStr(objDt.Rows[i]["TextValue"]));
            }
            else
            {
                if (i == 0)
                {
                    objStr.Append(" " + common.myStr(objDt.Rows[i]["TextValue"]));
                }
                else if (i + 1 == objDs.Tables[1].Rows.Count)
                {
                    objStr.Append(" and " + common.myStr(objDt.Rows[i]["TextValue"]) + ".");
                }
                else
                {
                    objStr.Append(", " + common.myStr(objDt.Rows[i]["TextValue"]));
                }
            }
        }
        //}
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
                dvNonTabular.RowFilter = "Tabular <> 1";
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
                        if (common.myInt(dtEntry.Rows[it]["RecordId"]) > 0)
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
                                                        common.myInt(dtEntry.Rows[it]["RecordId"]), GroupingDate));

                                            str.Append(" ");

                                            dr3 = null;
                                            dsAllNonTabularSectionDetails.Dispose();
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
                                                        common.myInt(dtEntry.Rows[it]["RecordId"]), GroupingDate));

                                            str.Append(" ");

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
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + ex.Message;
            objException.HandleException(ex);
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

    protected void btnExporttoWord_Click(object sender, EventArgs e)
    {
        RTF1.ExportToRtf();
    }

    protected void btnImage_Click(object sender, EventArgs e)
    {
        hdnImagePath.Value = "";
        RadWindow2.NavigateUrl = "../ImageEditor/Annotator.aspx?Page=I";
        RadWindow2.Height = 600;
        RadWindow2.Width = 1200;
        RadWindow2.Top = 20;
        RadWindow2.Left = 20;
        // RadWindowForNew.Title = "Time Slot";
        RadWindow2.OnClientClose = "GetImageOnClientClose";
        RadWindow2.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindow2.Modal = true;
        RadWindow2.VisibleStatusbar = false;
        RadWindow2.InitialBehaviors = WindowBehaviors.Maximize;
    }

    protected void btnRefresh_Click(object sender, EventArgs e)
    {

        BindPatientIllustrationImages(SignStatus);
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("/EMR/Templates/Notes.aspx?ifId=" + common.myStr(Request.QueryString["ifId"]), false);
    }

    private string BindPatientIllustrationImages(bool bSign)
    {
        hdnImagePath.Value = "";
        RTF1.Content = "";
        if (hdnImagePath.Value != "")
        {
            RTF1.Content = "<table><tbody><tr><td><img src='" + hdnImagePath.Value + "' width='250px' height='250px' border='0' align='middle' alt='Image' /></td></tr></tbody></table>" + RTF1.Content;
        }
        else
        {
            BaseC.EMR emr = new BaseC.EMR(sConString);
            DataSet ds = emr.GetPatientIllustrationImages(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), common.myInt(ViewState["RegistrationId"]), common.myInt(ViewState["EncounterId"]));
            if (ds.Tables[0].Rows.Count > 0)
            {
                DirectoryInfo objDir = new DirectoryInfo(Server.MapPath("~/PatientDocuments/" + common.myInt(Session["HospitalLocationId"]) + "/" + common.myInt(ViewState["RegistrationId"]) + "/" + common.myInt(ViewState["EncounterId"]) + "/"));
                if (objDir.Exists)
                {
                    //     string path = "/PatientDocuments/" + Session["HospitalLocationID"].ToString() + "/" + Convert.ToString(Session["RegistrationId"]) + "/" + Request.QueryString["EncId"] != null ? Request.QueryString["EncId"].ToString() : common.myStr(Session["EncounterId"]) + "/";
                    string path = "/PatientDocuments/" + common.myInt(Session["HospitalLocationID"]).ToString() + "/" + common.myInt(ViewState["RegistrationId"]).ToString() + "/" + common.myInt(ViewState["EncounterId"]) + "/";
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        hdnImagePath.Value = hdnImagePath.Value + " <table><tbody><tr><td><img src='" + path + ds.Tables[0].Rows[i]["ImageName"].ToString() + "' width='650px' height='450px' border='0' align='middle' alt='Image' /></td></tr></tbody></table><br/>";
                    }
                    RTF1.Content = hdnImagePath.Value + BindEditor(bSign);
                }
            }
            else
            {
                RTF1.Content = BindEditor(bSign);
            }
        }
        hdnImagePath.Value = "";
        return RTF1.Content;
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

    protected string GetFontFamily(string typ, DataRow item)
    {
        string FieldValue = "";
        string FontName = "";
        string sBegin = "";
        ClinicDefaults cd = new ClinicDefaults(Page);
        BaseC.EMRMasters.Fonts fonts = new BaseC.EMRMasters.Fonts();
        FontName = fonts.GetFont("Name", common.myStr(item[typ + "FontStyle"]));
        if (FontName != "")
        {
            sBegin += " font-family: " + FontName + ";";
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

    protected void btnSentenceGallery_OnClick(object sender, EventArgs e)
    {
        RadWindowForNew.NavigateUrl = "~/EMR/Templates/SentanceGallery.aspx?ctrlId=" + RTF1.ClientID + "&typ=2";
        RadWindowForNew.Height = 580;
        RadWindowForNew.Width = 650;
        RadWindowForNew.Top = 40;
        RadWindowForNew.Left = 100;
        RadWindowForNew.Title = "Sentance Gallery";
        RadWindowForNew.OnClientClose = "OnCloseSentenceGalleryRadWindow";
        RadWindowForNew.Modal = true;
        RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
        RadWindowForNew.VisibleStatusbar = false;
    }

    protected void btnAddendum_OnClick(object sender, EventArgs e)
    {
        RadWindowForNew.NavigateUrl = "~/Addendum.aspx";
        RadWindowForNew.Height = 445;
        RadWindowForNew.Width = 650;
        RadWindowForNew.Top = 40;
        RadWindowForNew.Left = 100;
        RadWindowForNew.Title = "Word Processor - Addendum";
        RadWindowForNew.OnClientClose = "OnClientClose";
        RadWindowForNew.Modal = true;
        RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
        RadWindowForNew.VisibleStatusbar = false;
    }

    protected void btnBackToWordProcessor_Click(object sender, EventArgs e)
    {
        pnlPrint.Visible = false;
        pnlCaseSheet.Visible = true;
    }

    protected void btnLetter_Click(object sender, EventArgs e)
    {
        Session["RTF"] = RTF1.Content.ToString();
        RadWindow2.NavigateUrl = "/Emr/Letters/Default.aspx";
        RadWindow2.Height = 600;
        RadWindow2.Width = 900;
        RadWindow2.Top = 40;
        RadWindow2.Left = 100;
        RadWindow2.Behaviors = WindowBehaviors.Maximize | WindowBehaviors.Minimize | WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Pin;
        RadWindow2.Modal = true;
        //asdf
        RadWindow2.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
        RadWindow2.VisibleStatusbar = false;

    }

    protected void btnPrintPDF_Click(object sender, EventArgs e)
    {
        Session["RTF"] = RTF1.Content.ToString();

        RadWindow2.NavigateUrl = "PrintPdf.aspx";
        RadWindow2.Height = 600;
        RadWindow2.Width = 1000;
        RadWindow2.Top = 40;
        RadWindow2.Left = 100;
        RadWindow2.Behaviors = WindowBehaviors.Maximize | WindowBehaviors.Minimize | WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Pin;
        RadWindow2.Modal = true;
        //asdf
        RadWindow2.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
        RadWindow2.VisibleStatusbar = false;



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

    protected void btnDictionary_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/MPages/Dictionary.aspx", false);
    }

    protected void lnkAddAllergy_OnClick(object sender, EventArgs e)
    {
        openPopup("Allergy");
    }

    protected void lnkAddDiagnosis_OnClick(object sender, EventArgs e)
    {
        openPopup("Diagnosis");
    }

    protected void lnkAddMedication_OnClick(object sender, EventArgs e)
    {
        openPopup("Medication");
    }

    protected void lnkAddVital_OnClick(object sender, EventArgs e)
    {
        openPopup("Vital");
    }

    protected void lnkAddNote_OnClick(object sender, EventArgs e)
    {
        openPopup("Note");
    }

    protected void lnkAddProblem_OnClick(object sender, EventArgs e)
    {
        openPopup("Problem");
    }

    protected void lnkAddOrder_OnClick(object sender, EventArgs e)
    {
        openPopup("Order");
    }

    protected void lnkSpecilityTemplate_OnClick(object sender, EventArgs e)
    {
        if (common.myInt(ddlSpecilityTemplate.SelectedValue) == 0)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Please select specility template from list !";
            Alert.ShowAjaxMsg(lblMessage.Text, this);
            return;
        }
        openPopup("SpecilityTemplate");
    }

    protected void lnkGeneralTemplate_OnClick(object sender, EventArgs e)
    {
        if (common.myInt(ddlGeneralTemplate.SelectedValue) == 0)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Please select general template from list !";
            Alert.ShowAjaxMsg(lblMessage.Text, this);
            return;
        }
        openPopup("GeneralTemplate");
    }

    public void openPopup(string strOpen)
    {
        switch (strOpen)
        {
            case "Allergy"://Add Allergy
                RadWindow1.NavigateUrl = "~/EMR/Allergy/Allergy.aspx?From=POPUP";
                break;
            case "Diagnosis"://Add Diagnosis
                RadWindow1.NavigateUrl = "~/EMR/Assessment/Diagnosis.aspx?From=POPUP";
                break;
            case "Medication": //Add Prescription
                RadWindow1.NavigateUrl = "/EMR/Medication/MedicineOrder.aspx?Regid=" + common.myInt(ViewState["RegistrationId"]) +
                                        "&RegNo=" + common.myInt(ViewState["RegistrationNo"]) +
                                        "&EncId=" + common.myInt(ViewState["EncounterId"]) +
                                        "&EncNo=" + common.myStr(ViewState["EncounterNo"]);
                break;
            case "Vital"://Add Vitals
                RadWindow1.NavigateUrl = "~/EMR/Vitals/Vitals.aspx?From=POPUP";
                break;
            case "Note"://Add Notes
                RadWindow1.NavigateUrl = "~/EMR/Templates/NoteSelect.aspx?From=POPUP";
                break;
            case "Problem"://Add Problems
                RadWindow1.NavigateUrl = "~/EMR/Problems/Default.aspx?From=POPUP";
                break;
            case "Order"://Add Orders
                RadWindow1.NavigateUrl = "/EMRBILLING/Popup/AddServices.aspx?Regid=" + common.myInt(ViewState["RegistrationId"]) +
                    "&RegNo=" + common.myInt(ViewState["RegistrationNo"]) +
                    "&EncId=" + common.myInt(ViewState["EncounterId"]) +
                    "&EncNo=" + common.myInt(ViewState["EncounterNo"]) +
                    "&OP_IP=I&FromWard=Y&CompanyId=0&InsuranceId=0&CardId=0&PayerType=&BType=";
                break;
            case "SpecilityTemplate"://Add SpecilityTemplate
                RadWindow1.NavigateUrl = "/EMR/Templates/Default.aspx?Regno=" + common.myInt(ViewState["RegistrationNo"]) + "&MASTER=No&Mpg=P" + common.myStr(ddlSpecilityTemplate.SelectedValue);
                break;
            case "GeneralTemplate"://Add GeneralTemplate
                RadWindow1.NavigateUrl = "/EMR/Templates/Default.aspx?Regno=" + common.myInt(ViewState["RegistrationNo"]) + "&MASTER=No&Mpg=p" + common.myStr(ddlGeneralTemplate.SelectedValue);
                break;
        }

        RadWindow1.Height = 600;
        RadWindow1.Width = 1050;
        RadWindow1.Top = 10;
        RadWindow1.Left = 10;
        RadWindow1.OnClientClose = "FillOnClientClose";
        RadWindow1.VisibleOnPageLoad = true;
        RadWindow1.Modal = true;
        RadWindow1.VisibleStatusbar = false;
    }

    protected void btnSeenByDoctor_OnClick(object sender, EventArgs e)
    {
        if (common.myStr(Session["EncounterStatus"]).ToUpper().Contains("CLOSE"))
        {
            return;
        }
        else
        {
            BaseC.EMR objE = new BaseC.EMR(sConString);
            int iCurrentStatus = 0;
            //if (btnSeenByDoctor.Text == "Patient Seen")
            //{
            //    iCurrentStatus = 0;
            //}
            //else
            //{
            //    iCurrentStatus = 1;
            //}

            string strError = objE.EMRSaveSeenByDoctor(common.myInt(ViewState["EncounterId"]), common.myInt(Session["EmployeeId"]),
                                iCurrentStatus, common.myInt(Session["UserId"]));

            lblMessage.Text = strError;
            //if (btnSeenByDoctor.Text == "Patient Seen")
            //{
            //    btnSeenByDoctor.Text = "Patient UnSeen";
            //}
            //else
            //{
            //    btnSeenByDoctor.Text = "Patient Seen";
            //}
        }
    }

    //private void bindPatientTemplateList()
    //{
    //    DataSet ds = new DataSet();
    //    try
    //    {
    //        BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);

    //        ds = objEMR.getTemplateEnteredList(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), common.myInt(ViewState["RegistrationId"]), common.myInt(ViewState["EncounterId"]), "", "");
    //        foreach (DataRow dsTitle in ds.Tables[0].Rows)
    //        {
    //            RadComboBoxItem item = new RadComboBoxItem();
    //            item.Text = (string)dsTitle["TemplateName"];
    //            item.Value = dsTitle["TemplateId"].ToString();
    //            item.Attributes.Add("TemplateType", common.myStr(dsTitle["TemplateType"]));

    //            ddlTemplatePatient.Items.Add(item);
    //            item.DataBind();
    //        }
    //        ddlTemplatePatient.Items.Insert(0, new RadComboBoxItem("ALL", ""));
    //        ddlTemplatePatient.SelectedIndex = 0;
    //    }
    //    catch (Exception Ex)
    //    {
    //        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
    //        lblMessage.Text = "Error: " + Ex.Message;

    //        objException.HandleException(Ex);
    //    }
    //    finally
    //    {
    //        ds.Dispose();
    //    }
    //}

    //protected void ddlTemplatePatient_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        bindDetails();
    //    }
    //    catch (Exception Ex)
    //    {
    //        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
    //        lblMessage.Text = "Error: " + Ex.Message;
    //        objException.HandleException(Ex);
    //    }
    //}

    protected void btnRefreshData_OnClick(object sender, EventArgs e)
    {
        //try
        //{

        bindDetails();
        //}
        //catch (Exception Ex)
        //{
        //    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
        //    lblMessage.Text = "Error: " + Ex.Message;
        //    objException.HandleException(Ex);
        //}
    }

    private string getGenderValue(int Gender)
    {
        if (Gender == 1)
        {
            return "F";
        }
        else if (Gender == 2)
        {
            return "M";
        }
        else
        {
            return "M";
        }
    }

}