using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using Telerik.Web.UI;
using System.Configuration;
using System.Drawing;
using System.IO;

public partial class Include_Components_MasterComponent_ucCaseSheetNew : System.Web.UI.UserControl
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    int RegId;
    int HospitalId;
    int EncounterId;
    int UserId;

    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["UserID"] == null)
            {
                Response.Redirect("/Login.aspx?Logout=1", false);
            }

            if (common.myStr(Request.QueryString["Type"]) == "OT")
            {
                lnkBtnViewCaseSheet.Visible = false;
            }
            lblMessage.Text = string.Empty;
            if (Request.QueryString["Mpg"] != null)
            {
                Session["CurrentNode"] = common.myStr(Request.QueryString["Mpg"]);
            }
            if (common.myInt(Session["RegistrationId"]) == 0)
            {
                return;
            }

            Session["DisplayEnteredByInCaseSheet"] = null;

            Session["DisplayEnteredByInCaseSheet"] = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]),
                                                    common.myInt(Session["FacilityId"]), "DisplayEnteredByInCaseSheet", sConString);

            Session["ControlOnlyByDisplayEnteredByInCaseSheet"] = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]),
                                                    common.myInt(Session["FacilityId"]), "ControlOnlyByDisplayEnteredByInCaseSheet", sConString);

            if (common.myInt(Request.QueryString["EncId"]) > 0)
            {
                EncounterId = common.myInt(Request.QueryString["EncId"]);
                ViewState["EncounterId"] = EncounterId;
            }
            else if (common.myStr(Session["EncounterId"]) != "")
            {
                EncounterId = common.myInt(Session["EncounterId"]);
                ViewState["EncounterId"] = EncounterId;
            }

            ViewState["DoctorId"] = common.myInt(Request.QueryString["DoctorId"]);
            if (common.myInt(ViewState["DoctorId"]).Equals(0))
            {
                ViewState["DoctorId"] = common.myInt(Session["DoctorId"]);
            }

            getDoctorImage();
            setOnPageLoad();
            if (Request.QueryString["Category"] != null && !common.myStr(Request.QueryString["Category"]).Equals(string.Empty))
            {
                if (common.myStr(Request.QueryString["Category"]).Equals("PopUp"))
                {
                    ibtnClose.Visible = true;
                }

            }
        }
        IsCopyCaseSheetAuthorized();
    }
    public void setOnPageLoad()
    {
        try
        {
            //if (common.myStr(Session["SelectedCaseSheet"]) != "PN")
            //{
            //    return;
            //}

            dtpFromDate.DateInput.DateFormat = common.myStr(Session["OutputDateFormat"]);
            dtpToDate.DateInput.DateFormat = common.myStr(Session["OutputDateFormat"]);

            //dtpFromDate.SelectedDate = Convert.ToDateTime(common.myDate(Session["EncounterDate"]).ToString("yyyy/MM/dd"));
            //dtpToDate.SelectedDate = DateTime.Now;

            //dtpFromDate.MinDate = Convert.ToDateTime(common.myDate(Session["EncounterDate"]).ToString("yyyy/MM/dd"));
            //dtpFromDate.MaxDate = DateTime.Now;
            //dtpToDate.MinDate = Convert.ToDateTime(common.myDate(Session["EncounterDate"]).ToString("yyyy/MM/dd"));
            //dtpToDate.MaxDate = DateTime.Now;


            dtpFromDate.SelectedDate = common.myDate((Session["EncounterDate"]).ToString());
            dtpToDate.SelectedDate = DateTime.Now;

            dtpFromDate.MinDate = common.myDate((Session["EncounterDate"]).ToString());
            dtpFromDate.MaxDate = common.myDate(DateTime.Now.ToString("yyyy/MM/dd"));
            dtpToDate.MinDate = common.myDate((Session["EncounterDate"]).ToString());
            dtpToDate.MaxDate = DateTime.Now;


            //dtpFromDate.SelectedDate = common.myDate(Convert.ToDateTime(Session["EncounterDate"]).ToString("yyyy/MM/dd"));
            //dtpToDate.SelectedDate = DateTime.Now;

            //dtpFromDate.MinDate = common.myDate(Convert.ToDateTime(Session["EncounterDate"]).ToString("yyyy/MM/dd"));
            //dtpFromDate.MaxDate = DateTime.Now;
            //dtpToDate.MinDate = common.myDate(Convert.ToDateTime(Session["EncounterDate"]).ToString("yyyy/MM/dd"));
            //dtpToDate.MaxDate = DateTime.Now;


            //dtpFromDate.SelectedDate = Convert.ToDateTime(Convert.ToDateTime(Session["EncounterDate"]).ToString("yyyy/MM/dd"));
            //dtpToDate.SelectedDate = DateTime.Now;

            //dtpFromDate.MinDate = Convert.ToDateTime(Convert.ToDateTime(Session["EncounterDate"]).ToString("yyyy/MM/dd"));
            //dtpFromDate.MaxDate = DateTime.Now;
            //dtpToDate.MinDate = Convert.ToDateTime(Convert.ToDateTime(Session["EncounterDate"]).ToString("yyyy/MM/dd"));
            //dtpToDate.MaxDate = DateTime.Now;

            if (common.myStr(Session["RegistrationId"]) == "")
            {
                return;
            }

            EncounterId = common.myInt(Session["EncounterId"]);

            bindTemplates();
            bindAttachments();

            ViewState["RequestTemplateId"] = common.myInt(Request.QueryString["RequestTemplateId"]);
            BindPatientHiddenDetails();

            Session["SelectFindPatient"] = null;
            GetFonts();

            RTF1.EditModes = EditModes.Preview;

            Session["SelectedCaseSheet"] = null;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    void BindPatientHiddenDetails()
    {
        //if (Session["PatientDetailString"] != null)
        //{
        //    lblPatientDetail.Text = Session["PatientDetailString"].ToString();
        //}
    }
    private void bindDetails()
    {
        gvDetails.Enabled = false;
        btnRefresh.Enabled = false;
        try
        {
            //RTF1.Content = BindEditor(true, false);
            if (common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]),
                                                common.myInt(Session["FacilityId"]), "ShowSignatureFooterLine", sConString).Equals("Y"))
            {
                RTF1.Content = BindEditor(true, false);
            }
            else
            {
                RTF1.Content = BindEditor(false, false);
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
            gvDetails.Enabled = true;
            btnRefresh.Enabled = true;
        }
    }
    private string[] getFromAndToDate(string ddlDateRange)
    {
        string[] sDate = new string[2];

        //if (ddlDateRange == "DR")
        //{
        //    if (dtpFromDate.SelectedDate != null && dtpToDate.SelectedDate != null)
        //    {
        //        sDate[0] = common.myDate(dtpFromDate.SelectedDate).ToString("yyyy/MM/dd");
        //        sDate[1] = common.myDate(dtpToDate.SelectedDate).ToString("yyyy/MM/dd");
        //    }
        //}
        //else if (ddlDateRange == "LTM")
        //{
        //    sDate[0] = DateTime.Now.AddMonths(-3).ToString("yyyy/MM/dd");
        //    sDate[1] = DateTime.Now.ToString("yyyy/MM/dd");
        //}
        //else if (ddlDateRange == "LSM")
        //{
        //    sDate[0] = DateTime.Now.AddDays(-6).ToString("yyyy/MM/dd");
        //    sDate[1] = DateTime.Now.ToString("yyyy/MM/dd");
        //}
        //else if (ddlDateRange == "LOY")
        //{
        //    sDate[0] = DateTime.Now.AddYears(-1).ToString("yyyy/MM/dd");
        //    sDate[1] = DateTime.Now.ToString("yyyy/MM/dd");
        //}
        //else
        //{
        //    sDate[0] = DateTime.Now.AddYears(-3).ToString("yyyy/MM/dd");
        //    sDate[1] = DateTime.Now.ToString("yyyy/MM/dd");
        //}

        sDate[0] = common.myDate(dtpFromDate.SelectedDate).ToString("yyyy/MM/dd");
        sDate[1] = common.myDate(dtpToDate.SelectedDate).ToString("yyyy/MM/dd");

        return sDate;
    }

    private void bindTemplates()
    {
        DataSet ds = new DataSet();
        BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
        try
        {
            //ViewState["GroupTable"] = null;

            //string[] sDateRange = getFromAndToDate(string.Empty); //getFromAndToDate(common.myStr(ddlDateRange.SelectedValue));

            //ds = objEMR.getEMRPatientVisitsBased(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), common.myInt(Session["RegistrationId"]),
            //                                common.myInt(Session["EncounterId"]), "T", 0,
            //                                0, sDateRange[0], sDateRange[1]);
            //if (ds.Tables.Count > 1)
            //{
            //    ViewState["GroupTable"] = ds.Tables[1];
            //}

            //gvDetails.DataSource = ds.Tables[0];
            //gvDetails.DataBind();

            ds = objEMR.getTemplateEnteredList(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), common.myInt(Session["RegistrationId"]),
                                                common.myInt(Session["EncounterId"]), common.myDate(dtpFromDate.SelectedDate.Value).ToString("yyyy/MM/dd"),
                                                common.myDate(dtpToDate.SelectedDate.Value).ToString("yyyy/MM/dd"));

            gvDetails.DataSource = ds.Tables[0];
            gvDetails.DataBind();
            ViewState["TemplateDetail"] = ds.Tables[0];
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
        finally
        {
            ds.Dispose();
            objEMR = null;
        }
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

    private string BindEditor(bool sign, bool IsViewSelectedTemplate)
    {
        StringBuilder sb = new StringBuilder();
        StringBuilder sbTemplateStyle = new StringBuilder();
        StringBuilder TemplateString;
        DataSet ds = new DataSet();
        DataSet dsTemplateStyle = new DataSet();
        DataRow drTemplateStyle = null;
        DataTable dtTemplate = new DataTable();
        DataView dvDataFilter = new DataView();
        DataTable dtEncounter = new DataTable();
        DataSet dsTemplateData = new DataSet();
        string AckString = string.Empty;
        bool ShowAck = false;

        string Templinespace = "";
        BindCaseSheet BindCaseSheet = new BindCaseSheet(sConString);
        BindNotes bnotes = new BindNotes(sConString);
        BaseC.DiagnosisDA fun = new BaseC.DiagnosisDA(sConString);
        BaseC.clsEMR emr = new BaseC.clsEMR(sConString);
        DataTable dtGroupDateWiseTemplate = new DataTable();
        StringBuilder sbTemp = new StringBuilder();
        bool bAllergyDisplay = false;
        bool bPatientBookingDisplay = false;

        string sTemplateName = common.myStr(ViewState["TemplateName"]);

        StringBuilder sbDynamicTemplateIds = new StringBuilder();
        StringBuilder sbStaticTemplateIds = new StringBuilder();
        try
        {
            string DoctorId = fun.GetDoctorId(common.myInt(Session["HospitalLocationID"]), Convert.ToInt16(common.myInt(Session["UserID"])));
            dsTemplateStyle = bnotes.GetTemplateStyle(common.myInt(Session["HospitalLocationId"]));

            if (IsViewSelectedTemplate)
            {
                ViewState["templateid_new"] = string.Empty;
                ViewState["TemplateName"] = string.Empty;
                ViewState["TemplateType"] = string.Empty;

                foreach (GridViewRow dataItem in gvDetails.Rows)
                {
                    CheckBox chkRow = (CheckBox)dataItem.FindControl("chkRow");

                    if (common.myBool(chkRow.Checked))
                    {
                        HiddenField hdnTemplateId = (HiddenField)dataItem.FindControl("hdnTemplateId");
                        HiddenField hdnTemplateType = (HiddenField)dataItem.FindControl("hdnTemplateType");

                        if (common.myInt(hdnTemplateId.Value) > 0)
                        {
                            if (common.myStr(hdnTemplateType.Value).Equals("D"))
                            {
                                if (sbDynamicTemplateIds.ToString() != string.Empty)
                                {
                                    sbDynamicTemplateIds.Append(",");
                                }
                                sbDynamicTemplateIds.Append(hdnTemplateId.Value);
                            }
                            else
                            {
                                if (sbStaticTemplateIds.ToString() != string.Empty)
                                {
                                    sbStaticTemplateIds.Append(",");
                                }
                                sbStaticTemplateIds.Append(hdnTemplateId.Value);
                            }
                        }
                    }
                }

                if (sbDynamicTemplateIds.ToString() == string.Empty && sbStaticTemplateIds.ToString() == string.Empty)
                {
                    return string.Empty;
                }

                dsTemplateData = emr.getEMRPrintCaseSheetDate(common.myInt(Session["HospitalLocationId"]),
                                                    common.myInt(Session["FacilityId"]), common.myInt(Session["RegistrationId"]),
                                                    common.myInt(ViewState["EncounterId"]), common.myDate(dtpFromDate.SelectedDate.Value).ToString("yyyy/MM/dd"),
                                                    common.myDate(dtpToDate.SelectedDate.Value).ToString("yyyy/MM/dd"), string.Empty,
                                                    0, string.Empty, false, 0);
            }
            else
            {
                dsTemplateData = emr.getEMRPrintCaseSheetDate(common.myInt(Session["HospitalLocationId"]),
                                                    common.myInt(Session["FacilityId"]), common.myInt(Session["RegistrationId"]),
                                                    common.myInt(ViewState["EncounterId"]), common.myDate(dtpFromDate.SelectedDate.Value).ToString("yyyy/MM/dd"),
                                                    common.myDate(dtpToDate.SelectedDate.Value).ToString("yyyy/MM/dd"), sTemplateName,
                                                    common.myInt(ViewState["templateid_new"]), common.myStr(ViewState["TemplateType"]), false, 0);
            }

            dtGroupDateWiseTemplate = emr.GetDateWiseGroupingTemplate(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
                                                    common.myInt(Session["RegistrationId"]), common.myInt(ViewState["EncounterId"]),
                                                    common.myDate(dtpFromDate.SelectedDate.Value).ToString("yyyy/MM/dd"),
                                                    common.myDate(dtpToDate.SelectedDate.Value).ToString("yyyy/MM/dd"));

            dvDataFilter = new DataView(dsTemplateData.Tables[21]);

            if (IsViewSelectedTemplate)
            {
                if (sbDynamicTemplateIds.ToString() != string.Empty || sbStaticTemplateIds.ToString() != string.Empty)
                {
                    if (sbDynamicTemplateIds.ToString() != string.Empty && sbStaticTemplateIds.ToString() != string.Empty)
                    {
                        dvDataFilter.RowFilter = "(TemplateType='D' AND TemplateId IN (" + sbDynamicTemplateIds.ToString() + ")) OR (TemplateType='S' AND PageId IN (" + sbStaticTemplateIds.ToString() + "))";
                    }
                    else if (sbDynamicTemplateIds.ToString() != string.Empty && sbStaticTemplateIds.ToString() == string.Empty)
                    {
                        dvDataFilter.RowFilter = "(TemplateType='D' AND TemplateId IN (" + sbDynamicTemplateIds.ToString() + "))";
                    }
                    else if (sbDynamicTemplateIds.ToString() == string.Empty && sbStaticTemplateIds.ToString() != string.Empty)
                    {
                        dvDataFilter.RowFilter = "(TemplateType='S' AND PageId IN (" + sbStaticTemplateIds.ToString() + "))";
                    }
                }

                dvDataFilter.Sort = "TemplateDate DESC";
                //Change By Himanshu On Date 28/03/2022 for Sanar Hospital ASC to DESC
            }

            dtEncounter = dsTemplateData.Tables[22];
          

            if (dsTemplateData.Tables.Count > 24)
            {
                if (dsTemplateData.Tables[25].Rows.Count > 0)
                {
                    if (common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]),
                           common.myInt(Session["FacilityId"]), "IsAcknowledgedByAndDate", sConString).Equals("Y"))
                    {
                        AckString = "<table><tr><td><b>Patient Acknowledged By: </b>" + common.myStr(dsTemplateData.Tables[25].Rows[0]["AcknowledgedByName"]).Trim() + "</td><td>&nbsp;<b>Patient Acknowledged Date: </b>" + common.myStr(dsTemplateData.Tables[25].Rows[0]["AcknowledgedDate"]) + " </td></tr></table>";
                        ShowAck = true;
                    }
                }
            }
           

            for (int iEn = 0; iEn < dtEncounter.Rows.Count; iEn++)
            {
                if (dvDataFilter.ToTable().Rows.Count > 0)
                {
                    #region Template Wise

                    dtTemplate = dvDataFilter.ToTable();
                    TemplateString = new StringBuilder();
                    if (ShowAck)
                    {
                        TemplateString = TemplateString.Append(AckString);
                        ShowAck = false;
                    }
                    //if(dsTemplateData.Tables.Count>23)
                    //{ 
                    // AckString = "<table><tr><td> AcknowledgedBy :" + common.myStr(dsTemplateData.Tables[24].Rows[0]["AcknowledgedByName"]) + "</td><td> AcknowledgedDate :" + common.myStr(dsTemplateData.Tables[24].Rows[0]["AcknowledgedDate"]) + " </td> </tr></table>";
                    //}
                    //TemplateString = TemplateString.Append(AckString);
                    for (int i = 0; i < dtTemplate.Rows.Count; i++)
                    {
                        #region Admission Request
                        if (common.myInt(ViewState["templateid_new"]).Equals(1121))
                        {
                            if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim() == "AdmissionRequest"
                                    && common.myStr(dtTemplate.Rows[i]["TemplateType"]).Trim() == "S" && bPatientBookingDisplay == false)
                            {
                                sbTemplateStyle = new StringBuilder();
                                DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                                dv.RowFilter = "PageId=" + common.myInt(dtTemplate.Rows[i]["PageId"]).ToString();
                                if (dv.Count > 0)
                                {
                                    drTemplateStyle = dv[0].Row;
                                    string sBegin = "", sEnd = "";
                                    Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                                    MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                                }
                                dv.Dispose();
                                #region Call Bind Patient Booking
                                sbTemp = new StringBuilder();
                                BindCaseSheet.BindPatientBooking(dsTemplateData.Tables[20], sbTemp, sbTemplateStyle, drTemplateStyle,
                                                   Page, common.myStr(dtTemplate.Rows[i]["PageId"]), "");
                                if (sbTemp.ToString() != "")
                                    TemplateString.Append(sbTemp + "<br/>" + "<br/>");
                                bPatientBookingDisplay = true;
                                #endregion
                            }
                        }
                        #endregion
                        #region Chief Complaints
                        if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim() == "Chief Complaints"
                             && common.myStr(dtTemplate.Rows[i]["TemplateType"]).Trim() == "S")
                        {
                            sbTemplateStyle = new StringBuilder();
                            DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                            dv.RowFilter = "PageId=" + common.myInt(dtTemplate.Rows[i]["PageId"]).ToString();
                            if (dv.Count > 0)
                            {
                                drTemplateStyle = dv[0].Row;
                                string sBegin = "", sEnd = "";
                                Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                                MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                            }
                            dv.Dispose();
                            sbTemp = new StringBuilder();
                            if ((common.myInt(ViewState["templateid_new"]) == 0)
                                 || (common.myInt(ViewState["templateid_new"]) == 98))
                            {
                                #region Call Bind Problem data
                                BindCaseSheet.BindProblemsHPI(dsTemplateData.Tables[0], common.myInt(Session["RegistrationId"]), common.myInt(dtEncounter.Rows[iEn]["EncounterId"]), sbTemp, sbTemplateStyle, drTemplateStyle,
                                               Page, common.myStr(dtTemplate.Rows[i]["PageId"]), "", true);
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
                           && common.myStr(dtTemplate.Rows[i]["TemplateCode"]).Trim() == "HIS")
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
                            if ((common.myInt(ViewState["templateid_new"]) == 0)
                                || (common.myInt(ViewState["templateid_new"]) == common.myInt(dtTemplate.Rows[i]["TemplateId"])))
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
                                if (common.myInt(ViewState["templateid_new"]) == 0)
                                {
                                    dvDyTable1.RowFilter = "TemplateId=" + common.myStr(dtTemplate.Rows[i]["TemplateId"]);
                                    dvDyTable4.RowFilter = "TemplateId=" + common.myStr(dtTemplate.Rows[i]["TemplateId"]);
                                    dvDyTable6.RowFilter = "TemplateId=" + common.myStr(dtTemplate.Rows[i]["TemplateId"]);
                                    dtDyTempTable = dvDyTable4.ToTable();
                                    //Change By Himanshu On Date 28/03/2022 for Sanar Hospital ASC to DESC

                                    dvDyTable4.Sort = "RecordId DESC";
                                }
                                else
                                {
                                    dvDyTable1.RowFilter = "TemplateId=" + common.myStr(common.myInt(ViewState["templateid_new"]));
                                    dvDyTable4.RowFilter = "TemplateId=" + common.myStr(common.myInt(ViewState["templateid_new"]));
                                    dvDyTable6.RowFilter = "TemplateId=" + common.myStr(common.myInt(ViewState["templateid_new"]));
                                    dtDyTempTable = dvDyTable4.ToTable();
                                    //Change By Himanshu On Date 28/03/2022 for Sanar Hospital ASC to DESC

                                    dvDyTable4.Sort = "RecordId DESC";
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
                            dv.RowFilter = "PageId=" + common.myInt(dtTemplate.Rows[i]["PageId"]).ToString();
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

                            if ((common.myInt(ViewState["templateid_new"]) == 0)
                                 || (common.myInt(ViewState["templateid_new"]) == 8))
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
                             && common.myStr(dtTemplate.Rows[i]["TemplateType"]).Trim() == "S")
                        {
                            sbTemplateStyle = new StringBuilder();
                            DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                            dv.RowFilter = "PageId=" + common.myInt(dtTemplate.Rows[i]["PageId"]).ToString();
                            if (dv.Count > 0)
                            {
                                drTemplateStyle = dv[0].Row;
                                string sBegin = "", sEnd = "";
                                Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                                MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                            }
                            dv.Dispose();
                            sbTemp = new StringBuilder();

                            if ((common.myInt(ViewState["templateid_new"]) == 0)
                                 || (common.myInt(ViewState["templateid_new"]) == 14))
                            {
                                #region Call Vital Template data
                                BindCaseSheet.BindVitals(dsTemplateData.Tables[11], sbTemp, sbTemplateStyle, drTemplateStyle,
                                                    Page, common.myStr(dtTemplate.Rows[i]["PageId"]), 0, "", common.myInt(dtEncounter.Rows[iEn]["EncounterId"]), true);

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
                            if ((common.myInt(ViewState["templateid_new"]) == 0)
                                || (common.myInt(ViewState["templateid_new"]) == common.myInt(dtTemplate.Rows[i]["TemplateId"])))
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
                                if (common.myInt(ViewState["templateid_new"]) == 0)
                                {
                                    dvDyTable1.RowFilter = "TemplateId=" + common.myStr(dtTemplate.Rows[i]["TemplateId"]);
                                    dvDyTable4.RowFilter = "TemplateId=" + common.myStr(dtTemplate.Rows[i]["TemplateId"]);
                                    dvDyTable6.RowFilter = "TemplateId=" + common.myStr(dtTemplate.Rows[i]["TemplateId"]);
                                    dtDyTempTable = dvDyTable4.ToTable();
                                    //dvDyTable4.Sort = "RecordId ASC, EntryDate ASC"; change By Himanshu 28/03/2022 for sanar hospital
                                    dvDyTable4.Sort = "RecordId DESC, EntryDate DESC";

                                }
                                else
                                {
                                    dvDyTable1.RowFilter = "TemplateId=" + common.myStr(common.myInt(ViewState["templateid_new"]));
                                    dvDyTable4.RowFilter = "TemplateId=" + common.myStr(common.myInt(ViewState["templateid_new"]));
                                    dvDyTable6.RowFilter = "TemplateId=" + common.myStr(common.myInt(ViewState["templateid_new"]));
                                    dtDyTempTable = dvDyTable4.ToTable();
                                    //dvDyTable4.Sort = "RecordId ASC, EntryDate ASC"; change By Himanshu 28/03/2022 for sanar hospital
                                    dvDyTable4.Sort = "RecordId DESC, EntryDate DESC";
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
                            dv.RowFilter = "PageId=" + common.myInt(dtTemplate.Rows[i]["PageId"]).ToString();
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
                           && common.myStr(dtTemplate.Rows[i]["TemplateType"]).Trim() == "S")
                        {
                            sbTemplateStyle = new StringBuilder();
                            DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                            dv.RowFilter = "PageId=" + common.myInt(dtTemplate.Rows[i]["PageId"]).ToString();
                            if (dv.Count > 0)
                            {
                                drTemplateStyle = dv[0].Row;
                                string sBegin = "", sEnd = "";
                                Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                                MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                            }
                            dv.Dispose();
                            sbTemp = new StringBuilder();

                            if ((common.myInt(ViewState["templateid_new"]) == 0)
                                 || (common.myInt(ViewState["templateid_new"]) == 133))
                            {
                                #region Call Diagnosis Template Data
                                if (common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]),
                                                        common.myInt(Session["FacilityId"]), "IsShowEMRDiagnosisInTabularFormat", sConString).Equals("Y"))
                                {
                                    BindCaseSheet.BindTabularAssessments(dsTemplateData.Tables[4], sbTemp, sbTemplateStyle, drTemplateStyle, Page,
                                        common.myStr(dtTemplate.Rows[i]["PageId"]), common.myStr(Session["UserID"]),
                                        0, common.myInt(dtEncounter.Rows[iEn]["EncounterId"]), "", string.Empty, true);
                                }
                                else
                                {
                                    BindCaseSheet.BindAssessments(dsTemplateData.Tables[4], sbTemp, sbTemplateStyle, drTemplateStyle, Page,
                                               common.myStr(dtTemplate.Rows[i]["PageId"]), common.myStr(Session["UserID"]),
                                               0, common.myInt(dtEncounter.Rows[iEn]["EncounterId"]), string.Empty, "Y", true);
                                }
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
                             && common.myStr(dtTemplate.Rows[i]["TemplateType"]).Trim() == "S")
                        {
                            sbTemplateStyle = new StringBuilder();
                            DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                            dv.RowFilter = "PageId=" + common.myInt(dtTemplate.Rows[i]["PageId"]).ToString();
                            if (dv.Count > 0)
                            {
                                drTemplateStyle = dv[0].Row;
                                string sBegin = "", sEnd = "";
                                Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                                MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                            }
                            dv.Dispose();
                            sbTemp = new StringBuilder();
                            if ((common.myInt(ViewState["templateid_new"]) == 0)
                                 || (common.myInt(ViewState["templateid_new"]) == 1085))
                            {
                                #region Call Provisional Diagnosis template data
                                BindCaseSheet.BindPatientProvisionalDiagnosis(dsTemplateData.Tables[2],
                                           Request.QueryString["DoctorId"] != null ? Request.QueryString["DoctorId"].ToString() : DoctorId, sbTemp, sbTemplateStyle, drTemplateStyle, Page,
                                           common.myStr(dtTemplate.Rows[i]["PageId"]), common.myStr(Session["UserID"]),
                                            0, common.myInt(dtEncounter.Rows[iEn]["EncounterId"]), "", true);
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
                            && common.myStr(dtTemplate.Rows[i]["TemplateCode"]).Trim() == "POC")
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
                            if ((common.myInt(ViewState["templateid_new"]) == 0)
                                || (common.myInt(ViewState["templateid_new"]) == common.myInt(dtTemplate.Rows[i]["TemplateId"])))
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
                                if (common.myInt(ViewState["templateid_new"]) == 0)
                                {
                                    dvDyTable1.RowFilter = "TemplateId=" + common.myStr(dtTemplate.Rows[i]["TemplateId"]);
                                    dvDyTable4.RowFilter = "TemplateId=" + common.myStr(dtTemplate.Rows[i]["TemplateId"]);
                                    dvDyTable6.RowFilter = "TemplateId=" + common.myStr(dtTemplate.Rows[i]["TemplateId"]);
                                    dtDyTempTable = dvDyTable4.ToTable();
                                    //change By Himanshu 28 / 03 / 2022 for sanar hospital ASC to DESC
                                    dvDyTable4.Sort = "RecordId DESC";
                                }
                                else
                                {
                                    dvDyTable1.RowFilter = "TemplateId=" + common.myStr(common.myInt(ViewState["templateid_new"]));
                                    dvDyTable4.RowFilter = "TemplateId=" + common.myStr(common.myInt(ViewState["templateid_new"]));
                                    dvDyTable6.RowFilter = "TemplateId=" + common.myStr(common.myInt(ViewState["templateid_new"]));
                                    dtDyTempTable = dvDyTable4.ToTable();
                                    //change By Himanshu 28 / 03 / 2022 for sanar hospital ASC to DESC
                                    dvDyTable4.Sort = "RecordId DESC";
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
                            }

                            Templinespace = "";
                        }
                        #endregion
                        #region Orders And Procedures
                        if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim() == "Orders And Procedures"
                             && common.myStr(dtTemplate.Rows[i]["TemplateType"]).Trim() == "S")
                        {
                            sbTemplateStyle = new StringBuilder();
                            DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                            dv.RowFilter = "PageId=" + common.myInt(dtTemplate.Rows[i]["PageId"]).ToString();
                            if (dv.Count > 0)
                            {
                                drTemplateStyle = dv[0].Row;
                                string sBegin = "", sEnd = "";
                                Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                                MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                            }
                            dv.Dispose();
                            sbTemp = new StringBuilder();

                            if ((common.myInt(ViewState["templateid_new"]) == 0)
                                 || (common.myInt(ViewState["templateid_new"]) == 17))
                            {
                                #region Call Bind Order data
                                BindCaseSheet.BindOrders(dsTemplateData.Tables[8], DoctorId, sbTemp, sbTemplateStyle, drTemplateStyle,
                                               Page, common.myStr(dtTemplate.Rows[i]["PageId"]), common.myInt(dtEncounter.Rows[iEn]["EncounterId"]), string.Empty, string.Empty, true, true);
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
                            //&& common.myStr(dtTemplate.Rows[i]["TemplateDate"]) == common.myStr(dtGroupDateWiseTemplate.Rows[iGp]["TemplateDate"])
                            )
                        {
                            sbTemplateStyle = new StringBuilder();
                            DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                            dv.RowFilter = "PageId=" + common.myInt(dtTemplate.Rows[i]["PageId"]).ToString();

                            if (dv.Count > 0)
                            {
                                drTemplateStyle = dv[0].Row;
                                string sBegin = "", sEnd = "";
                                Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                                MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                            }

                            dv.Dispose();
                            sbTemp = new StringBuilder();

                            if ((common.myInt(ViewState["templateid_new"]) == 0)
                                 || (common.myInt(ViewState["templateid_new"]) == 153))
                            {
                                DataSet dsMedication = new DataSet();
                                DataView dvTable1 = new DataView(dsTemplateData.Tables[10]);

                                dvTable1.ToTable().TableName = "Item";

                                dvTable1.RowFilter = "EncounterId=" + common.myInt(dtEncounter.Rows[iEn]["EncounterId"]);

                                dsMedication.Tables.Add(dvTable1.ToTable());

                                dvTable1.Dispose();

                                #region Call Medication Template data
                                //BindCaseSheet.BindMedication(dsMedication, sbTemp, sbTemplateStyle, drTemplateStyle, Page, common.myStr(dtTemplate.Rows[i]["PageId"]),
                                //                       common.myInt(Session["UserID"]).ToString(), string.Empty, 0);

                                DataTable dtDistinctValues = dsMedication.Tables[0].DefaultView.ToTable(true, "IndentId").Copy();
                                try
                                {
                                    foreach (DataRow drDistinctValues in dtDistinctValues.Rows)
                                    {
                                        BindCaseSheet.BindMedication(dsMedication, sbTemp, sbTemplateStyle, drTemplateStyle, Page, common.myStr(dtTemplate.Rows[i]["PageId"]),
                                                       common.myInt(Session["UserID"]).ToString(), string.Empty, common.myInt(drDistinctValues["IndentId"]), string.Empty, string.Empty, true);
                                    }
                                }
                                catch
                                {
                                }
                                finally
                                {
                                    dtDistinctValues.Dispose();
                                }
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
                             && common.myStr(dtTemplate.Rows[i]["TemplateType"]).Trim() == "S")
                        {
                            sbTemplateStyle = new StringBuilder();
                            DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                            dv.RowFilter = "PageId=" + common.myInt(dtTemplate.Rows[i]["PageId"]).ToString();
                            if (dv.Count > 0)
                            {
                                drTemplateStyle = dv[0].Row;
                                string sBegin = "", sEnd = "";
                                Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                                MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                            }
                            dv.Dispose();
                            sbTemp = new StringBuilder();
                            if ((common.myInt(ViewState["templateid_new"]) == 0)
                                 || (common.myInt(ViewState["templateid_new"]) == 1166))
                            {
                                #region Call Non Drug Order template data
                                BindCaseSheet.BindNonDrugOrder(dsTemplateData.Tables[7], sbTemp, sbTemplateStyle, drTemplateStyle, Page, "0",
                                                  common.myInt(dtEncounter.Rows[iEn]["EncounterId"]), "", true);
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
                             && common.myStr(dtTemplate.Rows[i]["TemplateType"]).Trim() == "S")
                        {
                            sbTemplateStyle = new StringBuilder();
                            DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                            dv.RowFilter = "PageId=" + common.myInt(dtTemplate.Rows[i]["PageId"]).ToString();

                            if (dv.Count > 0)
                            {
                                drTemplateStyle = dv[0].Row;
                                string sBegin = "", sEnd = "";
                                Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                                MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                            }

                            dv.Dispose();
                            sbTemp = new StringBuilder();

                            if ((common.myInt(ViewState["templateid_new"]) == 0)
                                 || (common.myInt(ViewState["templateid_new"]) == 1172))
                            {
                                #region Call Diet Order data
                                BindCaseSheet.BindDietOrderInNote(dsTemplateData.Tables[9], sbTemp, sbTemplateStyle, drTemplateStyle, Page, "0", "",
                                    common.myInt(dtEncounter.Rows[iEn]["EncounterId"]));
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
                             && common.myStr(dtTemplate.Rows[i]["TemplateType"]).Trim() == "S")
                        {
                            sbTemplateStyle = new StringBuilder();
                            DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                            dv.RowFilter = "PageId=" + common.myInt(dtTemplate.Rows[i]["PageId"]).ToString();
                            if (dv.Count > 0)
                            {
                                drTemplateStyle = dv[0].Row;
                                string sBegin = "", sEnd = "";
                                Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                                MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                            }
                            dv.Dispose();
                            sbTemp = new StringBuilder();
                            if ((common.myInt(ViewState["templateid_new"]) == 0)
                                 || (common.myInt(ViewState["templateid_new"]) == 1013))
                            {
                                #region Call Doctor Progress Note template data
                                BindCaseSheet.BindDoctorProgressNote(dsTemplateData.Tables[3], sbTemp, sbTemplateStyle, drTemplateStyle, Page,
                                           common.myStr(dtTemplate.Rows[i]["PageId"]), common.myStr(Session["UserID"]), "",
                                           common.myInt(dtEncounter.Rows[iEn]["EncounterId"]), true);
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
                             && common.myStr(dtTemplate.Rows[i]["TemplateType"]).Trim() == "S")
                        {
                            sbTemplateStyle = new StringBuilder();
                            DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                            dv.RowFilter = "PageId=" + common.myInt(dtTemplate.Rows[i]["PageId"]).ToString();
                            if (dv.Count > 0)
                            {
                                drTemplateStyle = dv[0].Row;
                                string sBegin = "", sEnd = "";
                                Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                                MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                            }
                            dv.Dispose();
                            sbTemp = new StringBuilder();

                            if ((common.myInt(ViewState["templateid_new"]) == 0)
                                 || (common.myInt(ViewState["templateid_new"]) == 1081))
                            {
                                StringBuilder temp1 = new StringBuilder();
                                #region Call Referral History Template Data
                                BindCaseSheet.BindReferalHistory(dsTemplateData.Tables[5], sbTemp, sbTemplateStyle, drTemplateStyle, Page, "0",
                                    common.myInt(dtEncounter.Rows[iEn]["EncounterId"]), "");
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
                            dv.RowFilter = "PageId=" + common.myInt(dtTemplate.Rows[i]["PageId"]).ToString();
                            if (dv.Count > 0)
                            {
                                drTemplateStyle = dv[0].Row;
                                string sBegin = "", sEnd = "";
                                Templinespace = common.myStr(common.myInt(drTemplateStyle["TemplateSpaceNumber"]) - 1);
                                MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                            }
                            dv.Dispose();
                            sbTemp = new StringBuilder();

                            if ((common.myInt(ViewState["templateid_new"]) == 0)
                                 || (common.myInt(ViewState["templateid_new"]) == 10005))
                            {
                                bnotes.BindMedication(common.myInt(ViewState["EncounterId"]), common.myInt(Session["HospitalLocationID"]), common.myInt(Session["RegistrationId"]), sbTemp, sbTemplateStyle, "C", drTemplateStyle,
                                                Page, common.myStr(dtTemplate.Rows[i]["PageId"]), common.myInt(Session["UserID"]).ToString(),
                                                common.myDate(dtpFromDate.SelectedDate.Value).ToString(),
                                                common.myDate(dtpToDate.SelectedDate.Value).ToString(), common.myStr(ViewState["OPIP"]), "");
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
                          && common.myStr(dtTemplate.Rows[i]["TemplateType"]).Trim() == "S")
                        {
                            sbTemplateStyle = new StringBuilder();
                            DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                            dv.RowFilter = "PageId=" + common.myInt(dtTemplate.Rows[i]["PageId"]).ToString();
                            if (dv.Count > 0)
                            {
                                drTemplateStyle = dv[0].Row;
                                string sBegin = "", sEnd = "";
                                Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                                MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                            }
                            dv.Dispose();
                            sbTemp = new StringBuilder();

                            if ((common.myInt(ViewState["templateid_new"]) == 0)
                                 || (common.myInt(ViewState["templateid_new"]) == 113))
                            {
                                BindCaseSheet.BindImmunization(dsTemplateData.Tables[13], sbTemp, sbTemplateStyle, drTemplateStyle, Page,
                                            common.myStr(dtTemplate.Rows[i]["PageId"]), "", common.myInt(dtEncounter.Rows[iEn]["EncounterId"]));
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
                            && common.myStr(dtTemplate.Rows[i]["TemplateType"]).Trim() == "S")
                        {
                            sbTemplateStyle = new StringBuilder();
                            DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                            dv.RowFilter = "PageId=" + common.myInt(dtTemplate.Rows[i]["PageId"]).ToString();
                            if (dv.Count > 0)
                            {
                                drTemplateStyle = dv[0].Row;
                                string sBegin = "", sEnd = "";
                                Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                                MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                            }

                            dv.Dispose();
                            sbTemp = new StringBuilder();

                            if ((common.myInt(ViewState["templateid_new"]) == 0)
                                 || (common.myInt(ViewState["templateid_new"]) == 805))
                            {
                                BindCaseSheet.BindInjection(dsTemplateData.Tables[12], sbTemp, sbTemplateStyle, drTemplateStyle, Page,
                                             common.myStr(dtTemplate.Rows[i]["PageId"]), "", common.myInt(dtEncounter.Rows[iEn]["EncounterId"]));

                                TemplateString.Append(sbTemp + "<br/>");
                                sbTemp = null;
                                sbTemplateStyle = null;
                            }
                            Templinespace = "";
                        }
                        #endregion
                        #region Follow-up Appointment
                        if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim().ToUpper().Equals("FOLLOW UP APPOINTMENT")
                            && common.myStr(dtTemplate.Rows[i]["TemplateType"]).Trim() == "S")
                        {
                            sbTemplateStyle = new StringBuilder();
                            DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                            dv.RowFilter = "PageId=" + common.myInt(dtTemplate.Rows[i]["PageId"]).ToString();
                            if (dv.Count > 0)
                            {
                                drTemplateStyle = dv[0].Row;
                                string sBegin = "", sEnd = "";
                                Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                                MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                            }
                            dv.Dispose();
                            if ((common.myInt(ViewState["templateid_new"]) == 0)
                                 || (common.myInt(ViewState["templateid_new"]) == 919))
                            {
                                StringBuilder temp = new StringBuilder();
                                #region FollowUp Appointment
                                BindCaseSheet.GetEncounterFollowUpAppointment(dsTemplateData.Tables[6],
                                       temp, sbTemplateStyle, drTemplateStyle, Page, "", common.myInt(dtEncounter.Rows[iEn]["EncounterId"]));
                                #endregion

                                TemplateString.Append(temp);
                                temp = null;
                                sbTemplateStyle = null;
                            }
                            Templinespace = "";
                        }
                        #endregion
                        #region Drug Administration
                        if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim().ToUpper().Equals("DRUG ADMINISTRATION")
                             && common.myStr(dtTemplate.Rows[i]["TemplateType"]).Trim().Equals("S"))
                        {
                            sbTemplateStyle = new StringBuilder();
                            DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                            dv.RowFilter = "PageId=" + common.myInt(dtTemplate.Rows[i]["PageId"]).ToString();
                            if (dv.Count > 0)
                            {
                                drTemplateStyle = dv[0].Row;
                                string sBegin = string.Empty;
                                string sEnd = string.Empty;
                                Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                                MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                            }
                            dv = new DataView();
                            sbTemp = new StringBuilder();
                            if ((common.myInt(ViewState["templateid_new"]).Equals(0))
                                 || (common.myInt(ViewState["templateid_new"]).Equals(22618)))
                            {
                                #region Call Drug Administration data
                                if (dsTemplateData.Tables.Count > 22)
                                {
                                    BindCaseSheet.bindDrugAdministration(dsTemplateData.Tables[23], sbTemp, sbTemplateStyle, drTemplateStyle,
                                                        Page, common.myStr(dtTemplate.Rows[i]["PageId"]), 0,
                                                        string.Empty, common.myInt(dtEncounter.Rows[iEn]["EncounterId"]));
                                }
                                #endregion
                                if (!sbTemp.ToString().Equals(string.Empty))
                                {
                                    TemplateString.Append(sbTemp + "<br/>" + "<br/>");
                                }
                                sbTemp = null;
                                sbTemplateStyle = null;
                            }
                            drTemplateStyle = null;
                            Templinespace = string.Empty;
                        }
                        #endregion
                    }
                    if (TemplateString.Length > 30)
                    {
                        //if (iEn == 0)
                        //{
                        //    sb.Append("<span style='font-size:20px; font-family:Tohama;'>");
                        //    sb.Append("<b><u>Initial Assessment</u></b><br/><br/>");
                        //    sb.Append("</span>");
                        //}
                        sb.Append("<span style='" + String.Empty + "'>");
                        sb.Append(TemplateString);
                        sb.Append("</span><br/>");
                        TemplateString = null;
                    }

                    #endregion
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
            sbDynamicTemplateIds = null;
            sbStaticTemplateIds = null;
        }
        return sb.ToString();
    }

    private string BindStaticTemplates(int StaticTemplateId, int TemplateFieldId)
    {
        string sEREncounterId = "0";
        if (Request.QueryString["EREncounterId"] != null)
        {
            sEREncounterId = Request.QueryString["EREncounterId"].ToString();
        }
        string[] sDate = getFromAndToDate(string.Empty);//getFromAndToDate(ddlDateRange.SelectedValue);
        StringBuilder sb = new StringBuilder();
        StringBuilder sbStatic = new StringBuilder();
        StringBuilder sbTemplateStyle = new StringBuilder();
        DataSet dsTemplate = new DataSet();
        DataSet dsTemplateStyle = new DataSet();
        DataRow drTemplateStyle = null;
        DataTable dtTemplate = new DataTable();
        Hashtable hst = new Hashtable();
        string Templinespace = "";
        string Fonts = string.Empty;
        DataView dvFilterStaticTemplate = new DataView();
        RegId = common.myInt(Session["RegistrationID"]);
        HospitalId = common.myInt(Session["HospitalLocationID"]);
        EncounterId = common.myInt(ViewState["EncounterId"]) == 0 ? common.myInt(EncounterId) : common.myInt(ViewState["EncounterId"]);
        UserId = common.myInt(Session["UserID"]);
        BindNotes bnotes = new BindNotes(sConString);
        BaseC.DiagnosisDA fun = new BaseC.DiagnosisDA(sConString);
        try
        {
            string DoctorId = fun.GetDoctorId(HospitalId, Convert.ToInt16(UserId));
            dsTemplateStyle = bnotes.GetTemplateStyle(common.myInt(Session["HospitalLocationId"]));
            dsTemplate = bnotes.GetEMRTemplates(EncounterId, RegId, sEREncounterId);
            dvFilterStaticTemplate = new DataView(dsTemplate.Tables[0]);
            dvFilterStaticTemplate.RowFilter = "PageId=" + common.myInt(StaticTemplateId);
            dtTemplate = dvFilterStaticTemplate.ToTable();
            sb.Append("<span style='" + Fonts + "'>");
            if (dtTemplate.Rows.Count > 0)
            {
                if (common.myStr(dtTemplate.Rows[0]["TemplateName"]).Trim() == "Allergies"
                    && common.myStr(dtTemplate.Rows[0]["DataStatus"]).Trim() == "AVAILABLE")
                {
                    string strTemplateType = common.myStr(dtTemplate.Rows[0]["PageIdentification"]);
                    strTemplateType = strTemplateType.Substring(0, 1);
                    sbTemplateStyle = new StringBuilder();
                    DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                    dv.RowFilter = "PageId=" + common.myInt(dtTemplate.Rows[0]["PageId"]);
                    drTemplateStyle = null;// = dv[0].Row;
                    if (dv.Count > 0)
                    {
                        drTemplateStyle = dv[0].Row;
                        string sBegin = "", sEnd = "";
                        Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                        MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                    }
                    StringBuilder sbTemp = new StringBuilder();
                    bnotes.BindAllergies(RegId, sbStatic, sbTemplateStyle, drTemplateStyle, Page, common.myStr(Session["HospitalLocationId"]),
                                common.myStr(Session["UserID"]), common.myStr(dtTemplate.Rows[0]["PageID"]),
                                common.myStr(sDate[0]), common.myStr(sDate[1]), TemplateFieldId, "");
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
                    dv.RowFilter = "PageId=" + common.myInt(dtTemplate.Rows[0]["PageId"]);
                    if (dv.Count > 0)
                    {
                        drTemplateStyle = dv[0].Row;
                        string sBegin = "", sEnd = "";
                        Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                        MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                    }
                    StringBuilder sbTemp = new StringBuilder();
                    bnotes.BindVitals(HospitalId.ToString(), common.myInt(ViewState["EncounterId"]), sbStatic, sbTemplateStyle, drTemplateStyle,
                                        Page, common.myStr(dtTemplate.Rows[0]["PageId"]), common.myStr(Session["UserID"]),
                                         common.myStr(sDate[0]), common.myStr(sDate[1]), TemplateFieldId, sEREncounterId, "");
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
                    dv.RowFilter = "PageId=" + common.myInt(dtTemplate.Rows[0]["PageId"]);
                    if (dv.Count > 0)
                    {
                        drTemplateStyle = dv[0].Row;
                        string sBegin = "", sEnd = "";
                        Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                        MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                    }
                    StringBuilder sbTemp = new StringBuilder();
                    if (common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]),
                                                       common.myInt(Session["FacilityId"]), "IsShowEMRDiagnosisInTabularFormat", sConString).Equals("Y"))
                    {
                        bnotes.BindTabularAssessments(RegId, HospitalId, EncounterId, Convert.ToInt16(UserId),
               DoctorId, sbStatic, sbTemplateStyle, drTemplateStyle, Page,
               common.myStr(dtTemplate.Rows[0]["PageId"]), common.myStr(Session["UserID"]),
               common.myStr(sDate[0]), common.myStr(sDate[1]), TemplateFieldId, sEREncounterId, "");
                    }

                    else
                    {
                        bnotes.BindAssessments(RegId, HospitalId, EncounterId, Convert.ToInt16(UserId),
                                    DoctorId, sbStatic, sbTemplateStyle, drTemplateStyle, Page,
                                    common.myStr(dtTemplate.Rows[0]["PageId"]), common.myStr(Session["UserID"]),
                                    common.myStr(sDate[0]), common.myStr(sDate[1]), TemplateFieldId, sEREncounterId, "");
                    }
                    drTemplateStyle = null;
                    Templinespace = "";
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            sEREncounterId = string.Empty;
            sDate = null;
            sbTemplateStyle = null;
            dsTemplate.Dispose();
            dsTemplateStyle.Dispose();
            drTemplateStyle = null;
            dtTemplate.Dispose();
            hst = null;
            Templinespace = string.Empty;
            dvFilterStaticTemplate.Dispose();
            RegId = 0;
            HospitalId = 0;
            EncounterId = 0;
            UserId = 0;
            bnotes = null;
            fun = null;
        }
        return "<br/>" + common.myStr(sbStatic);
    }
    protected string CreateString(DataSet objDs, int iRootId, string iRootName, string TabularType, string sectionId, string EntryType, int RecordId)
    {
        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        StringBuilder objStr = new StringBuilder();
        DataView objDv = null;
        DataTable objDt = null;
        DataSet dsMain = new DataSet();
        StringBuilder objStrTmp = new StringBuilder();
        int MaxLength = 0, FieldsLength = 0;
        if (objDs != null)
        {
            if (bool.Parse(TabularType) == true)
            {
                if (objDs.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = objDs.Tables[0].Rows[0];
                    DataView dvValues = new DataView(objDs.Tables[1]);
                    dvValues.RowFilter = "FieldId=" + common.myInt(dr["FieldId"]);
                    MaxLength = dvValues.ToTable().Rows.Count;
                    DataTable dtFilter = dvValues.ToTable();

                    DataView dvFilter = new DataView(dtFilter);
                    dvFilter.RowFilter = "RowCaption='0'";
                    DataTable dtNewTable = dvFilter.ToTable();

                    if (dtNewTable.Rows.Count > 0)
                    {
                        if (MaxLength != 0)
                        {
                            if (EntryType != "M")
                            {
                                objStr.Append("<br /><table border='1' style='border-color:#000000; border:solid;  border-collapse:collapse; " + sFontSize + "'  cellpadding='2' cellspacing='3' ><tr align='center'>");
                                FieldsLength = objDs.Tables[0].Rows.Count;
                                //if (common.myStr(objDs.Tables[0].Rows[0]["TRows"]).Trim() != ""
                                //    && common.myStr(objDs.Tables[0].Rows[0]["TRows"]).Trim() != "0")
                                //{
                                //    objStr.Append("<th align='center' style='border-bottom-color:Black; border-bottom-style:solid; border-bottom-width:1px; " + sFontSize + " ' >&nbsp;</th>");
                                //}
                                for (int i = 0; i < FieldsLength; i++)   // it makes table
                                {
                                    objStr.Append("<th align='center' style='border-bottom-color:Black; border-bottom-style:solid; border-bottom-width:1px; " + sFontSize + " ' >" + common.myStr(objDs.Tables[0].Rows[i]["FieldName"]) + "</th>");

                                    dr = objDs.Tables[0].Rows[i];
                                    dvValues = new DataView(objDs.Tables[1]);
                                    dvValues.RowFilter = "FieldId=" + common.myInt(dr["FieldId"]);
                                    dsMain.Tables.Add(dvValues.ToTable("table" + i.ToString()));

                                    if (dvValues.ToTable().Rows.Count > MaxLength)
                                    {
                                        MaxLength = dvValues.ToTable().Rows.Count;
                                    }
                                }
                            }
                            objStr.Append("</tr>");
                            if (MaxLength == 0)
                            {
                            }
                            else
                            {
                                if (EntryType != "M")
                                {
                                    if (dsMain.Tables[0].Rows.Count > 0)
                                    {
                                        for (int i = 0; i < MaxLength; i++)
                                        {
                                            objStr.Append("<tr>");
                                            for (int j = 0; j < dsMain.Tables.Count; j++)
                                            {
                                                if (dsMain.Tables[j].Rows.Count > i
                                                    && dsMain.Tables[j].Rows.Count > 0)
                                                {
                                                    objStr.Append("<td style=' " + sFontSize + "' align='right'>" + common.myStr(dsMain.Tables[j].Rows[i]["TextValue"]) + "</td>");
                                                }
                                                else
                                                {
                                                    objStr.Append("<td style=' " + sFontSize + "' align='right'>&nbsp;</td>");
                                                }
                                            }
                                            objStr.Append("</tr>");
                                        }
                                        objStr.Append("</table>");
                                    }
                                }
                                else
                                {
                                    Hashtable hstInput = new Hashtable();
                                    hstInput.Add("@intTemplateId", iRootId);
                                    if (common.myInt(Session["Gender"]) == 1)
                                    {
                                        hstInput.Add("chrGenderType", "F");
                                    }
                                    else if (common.myInt(Session["Gender"]) == 2)
                                    {
                                        hstInput.Add("chrGenderType", "M");
                                    }
                                    else
                                    {
                                        hstInput.Add("chrGenderType", "M");
                                    }
                                    hstInput.Add("@intEncounterId", common.myInt(ViewState["EncounterId"]) == 0 ? common.myInt(EncounterId) : common.myInt(ViewState["EncounterId"]));
                                    hstInput.Add("@intSecId", Convert.ToInt32(sectionId));
                                    hstInput.Add("@intRecordId", RecordId);
                                    DataSet dsTabulerTemplate = dl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetTemplateDetailsTabular", hstInput);
                                    DataView dvRowCaption = new DataView(dsTabulerTemplate.Tables[0]);
                                    StringBuilder sbCation = new StringBuilder();

                                    if (dvRowCaption.ToTable().Rows.Count > 0)
                                    {
                                        dvRowCaption.RowFilter = "RowNum>0";
                                        DataTable dt = dvRowCaption.ToTable();
                                        if (dt.Rows.Count > 0)
                                        {
                                            sbCation.Append("<br /><table border='1' style='border-color:#000000; border:solid;  border-collapse:collapse; " + sFontSize + "'  cellpadding='2' cellspacing='3' ><tr align='center'>");
                                            int column = dt.Columns.Count;
                                            int ColumnCount = 0;
                                            int count = 1;

                                            //Added by rakesh because caption tabular template showing last column missiong start
                                            for (int k = 1; k < (column - 5); k++)
                                            //Added by rakesh because caption tabular template showing last column missiong start
                                            {
                                                sbCation.Append("<td>");
                                                sbCation.Append(common.myStr(dt.Rows[0]["Col" + count]));
                                                sbCation.Append("</td>");
                                                count++;
                                                ColumnCount++;
                                            }
                                            sbCation.Append("</tr>");

                                            DataView dvRow = new DataView(dt);
                                            DataTable dtRow = dvRow.ToTable();
                                            for (int l = 1; l <= dtRow.Rows.Count - 3; l++)
                                            {
                                                sbCation.Append("<tr>");
                                                for (int i = 1; i < ColumnCount + 1; i++)
                                                {
                                                    if (dt.Rows[1]["Col" + i].ToString() == "D" || dt.Rows[1]["Col" + i].ToString() == "IM")
                                                    {
                                                        DataView dvDrop = new DataView(dsTabulerTemplate.Tables[1]);
                                                        if (dt.Rows[l + 1]["Col" + i].ToString() != "")
                                                        {
                                                            dvDrop.RowFilter = "ValueId=" + common.myInt(dt.Rows[l + 1]["Col" + i]);
                                                            if (dvDrop.ToTable().Rows.Count > 0)
                                                            {
                                                                if (dt.Rows[1]["Col" + i].ToString() == "D")
                                                                {
                                                                    sbCation.Append("<td style=' " + sFontSize + "' align='center'>" + dvDrop.ToTable().Rows[0]["ValueName"].ToString() + "</td>");
                                                                }
                                                                else if (dt.Rows[1]["Col" + i].ToString() == "IM")
                                                                {
                                                                    sbCation.Append("<td align='center' ><img  id='dvImageType' runat='server'  alt='Image' width='30px' heigth='30px' src='" + dvDrop.ToTable().Rows[0]["ImagePath"].ToString() + "' /></td>");
                                                                }
                                                                else
                                                                {
                                                                    sbCation.Append("<td style=' " + sFontSize + "' align='center'>&nbsp;</td>");
                                                                }
                                                            }
                                                            else
                                                            {
                                                                sbCation.Append("<td style=' " + sFontSize + "' align='center'>&nbsp;</td>");
                                                            }
                                                        }
                                                        else
                                                        {
                                                            sbCation.Append("<td style=' " + sFontSize + "' align='center'>&nbsp;</td>");
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (dt.Rows[l + 1]["Col" + i].ToString() != "")
                                                        {
                                                            sbCation.Append("<td style=' " + sFontSize + "' align='center'>" + dt.Rows[l + 1]["Col" + i].ToString() + "</td>");
                                                        }
                                                        else
                                                        {
                                                            sbCation.Append("<td style=' " + sFontSize + "' align='center'>&nbsp;</td>");
                                                        }
                                                    }
                                                }
                                                sbCation.Append("</tr>");
                                            }
                                        }
                                        sbCation.Append("</table>");
                                    }
                                    objStr.Append(sbCation);
                                }

                            }
                        }
                    }
                    else
                    {
                        Hashtable hstInput = new Hashtable();
                        hstInput.Add("@intTemplateId", iRootId);

                        if (common.myInt(Session["Gender"]) == 1)
                        {
                            hstInput.Add("chrGenderType", "F");
                        }
                        else if (common.myInt(Session["Gender"]) == 2)
                        {
                            hstInput.Add("chrGenderType", "M");
                        }
                        else
                        {
                            hstInput.Add("chrGenderType", "M");
                        }

                        hstInput.Add("@intEncounterId", common.myInt(ViewState["EncounterId"]) == 0 ? common.myInt(EncounterId) : common.myInt(ViewState["EncounterId"]));
                        hstInput.Add("@intSecId", Convert.ToInt32(sectionId));
                        hstInput.Add("@intRecordId", RecordId);
                        DataSet dsTabulerTemplate = dl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetTemplateDetailsTabular", hstInput);

                        DataView dvRowCaption = new DataView(dsTabulerTemplate.Tables[0]);

                        dvRowCaption.RowFilter = "RowCaptionId>0";
                        if (dvRowCaption.ToTable().Rows.Count > 0)
                        {
                            StringBuilder sbCation = new StringBuilder();
                            dvRowCaption.RowFilter = "RowNum>0";
                            DataTable dt = dvRowCaption.ToTable();
                            if (dt.Rows.Count > 0)
                            {
                                sbCation.Append("<br /><table border='1' style='border-color:#000000; border:solid;  border-collapse:collapse; " + sFontSize + "'  cellpadding='2' cellspacing='3' ><tr align='center'>");
                                int column = dt.Columns.Count;
                                int ColumnCount = 0;
                                int count = 1;
                                for (int k = 1; k < (column - 4); k++)
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

                                DataView dvRow = new DataView(dt);
                                dvRow.RowFilter = "RowCaptionId>0";
                                DataTable dtRow = dvRow.ToTable();
                                for (int l = 1; l <= dtRow.Rows.Count; l++)
                                {
                                    sbCation.Append("<tr>");
                                    for (int i = 0; i < ColumnCount; i++)
                                    {
                                        if (i == 0)
                                        {
                                            sbCation.Append("<td style=' " + sFontSize + "' align='center'>" + common.myStr(dt.Rows[l + 1]["RowCaptionName"]) + "</td>");
                                        }
                                        else
                                        {
                                            if (dt.Rows[1]["Col" + i].ToString() == "D" || dt.Rows[1]["Col" + i].ToString() == "IM")
                                            {
                                                DataView dvDrop = new DataView(dsTabulerTemplate.Tables[1]);
                                                if (dt.Rows[l + 1]["Col" + i].ToString() != "")
                                                {
                                                    dvDrop.RowFilter = "ValueId=" + common.myInt(dt.Rows[l + 1]["Col" + i]);
                                                    if (dvDrop.ToTable().Rows.Count > 0)
                                                    {
                                                        if (dt.Rows[1]["Col" + i].ToString() == "D")
                                                        {
                                                            sbCation.Append("<td style=' " + sFontSize + "' align='center'>" + dvDrop.ToTable().Rows[0]["ValueName"].ToString() + "</td>");
                                                        }
                                                        else if (dt.Rows[1]["Col" + i].ToString() == "IM")
                                                        {
                                                            sbCation.Append("<td align='center' ><img  id='dvImageType' runat='server'  alt='Image' width='30px' heigth='30px' src='" + dvDrop.ToTable().Rows[0]["ImagePath"].ToString() + "' /></td>");
                                                        }
                                                        else
                                                        {
                                                            sbCation.Append("<td style=' " + sFontSize + "' align='center'>&nbsp;</td>");
                                                        }
                                                    }
                                                    else
                                                    {
                                                        sbCation.Append("<td style=' " + sFontSize + "' align='center'>&nbsp;</td>");
                                                    }
                                                }
                                                else
                                                {
                                                    sbCation.Append("<td style=' " + sFontSize + "' align='center'>&nbsp;</td>");
                                                }
                                            }
                                            else
                                            {
                                                if (dt.Rows[l + 1]["Col" + i].ToString() != "")
                                                {
                                                    sbCation.Append("<td style=' " + sFontSize + "' align='center'>" + dt.Rows[l + 1]["Col" + i].ToString() + "</td>");
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
                            }
                            objStr.Append(sbCation);
                        }

                    }
                }
            }
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
                    objDv.RowFilter = "FieldId=" + common.myInt(item["FieldId"]);
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
                        if (objDt.Rows.Count > 0)
                        {
                            sEnd = "";
                            MakeFont("Fields", ref sBegin, ref sEnd, item);
                            if (Convert.ToBoolean(item["DisplayTitle"]))
                            {
                                objStr.Append(BeginList + sBegin + common.myStr(item["FieldName"]));
                                if (objStr.ToString() != "")
                                {
                                    objStr.Append(sEnd + "</li>");
                                }
                            }
                            BeginList = "";
                            sBegin = "";
                        }
                        else
                        {
                            if (common.myStr(item["FieldType"]).Equals("H"))
                            {
                                sEnd = "";
                                MakeFont("Fields", ref sBegin, ref sEnd, item);
                                if (common.myBool(item["DisplayTitle"]))
                                {
                                    objStr.Append(BeginList + sBegin + "<U>" + common.myStr(item["FieldName"]) + "</U>");

                                    if (objStr.ToString() != "")
                                    {
                                        objStr.Append(sEnd + "</li>");
                                    }
                                }
                                BeginList = "";
                                sBegin = "";
                            }
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
                        objDv.RowFilter = "FieldId=" + common.myInt(item["FieldId"]);
                        objDt = objDv.ToTable();
                        DataView dvFieldType = new DataView(objDs.Tables[0]);
                        dvFieldType.RowFilter = "FieldId=" + common.myInt(item["FieldId"]);
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
                                else if (FType == "O")
                                {
                                    objStr.Append(" " + common.myStr(objDt.Rows[i]["TextValue"]));
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
            if (objStr.ToString() != "")
            {
                DataView dvValues = new DataView(objDs.Tables[1]);
                dvValues.RowFilter = "SectionId=" + common.myInt(sectionId);
                if (dvValues.ToTable().Rows.Count > 0)
                {
                    if (ViewState["CurrentTemplateFontName"] != null && !common.myStr(ViewState["CurrentTemplateFontName"]).Equals(string.Empty))
                    {
                        objStr.Append("<b><br/>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style='font-family: " + common.myStr(ViewState["CurrentTemplateFontName"]) + "; '>Entered By: " + common.myStr(dvValues.ToTable().Rows[0]["EnterBy"]) + " Date/Time: " + common.myStr(dvValues.ToTable().Rows[0]["VisitDateTime"]) + "</span></b>");
                    }
                    else
                    {
                        objStr.Append("<b><br/>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style='font-family: Tahoma;'>Entered By: " + common.myStr(dvValues.ToTable().Rows[0]["EnterBy"]) + " Date/Time: " + common.myStr(dvValues.ToTable().Rows[0]["VisitDateTime"]) + "</span></b>");
                    }
                }
            }
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

    protected void bindData(string iFormId, string TemplateId, StringBuilder sb)
    {
        string str = "";
        string sEntryType = "V";
        StringBuilder objStrTmp = new StringBuilder();
        StringBuilder objStrSettings = new StringBuilder();
        int iPrevId = 0;

        Hashtable hstInput = new Hashtable();
        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        hstInput.Add("@inyHospitalLocationID", common.myInt(Session["HospitalLocationID"]));
        hstInput.Add("@intTemplateId", TemplateId);

        if (common.myInt(Session["Gender"]) == 1)
        {
            hstInput.Add("chrGenderType", "F");
        }
        else if (common.myInt(Session["Gender"]) == 2)
        {
            hstInput.Add("chrGenderType", "M");
        }

        hstInput.Add("@intFormId", "1");
        hstInput.Add("@bitDischargeSummary", 0);
        DataSet ds = dl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetSectionTreeForPatientsForms", hstInput);

        if (ds.Tables[0].Rows.Count > 0)
        {
            sEntryType = common.myStr(ds.Tables[0].Rows[0]["EntryType"]);
        }
        hstInput = new Hashtable();
        hstInput.Add("@intTemplateId", TemplateId);

        if (common.myInt(Session["Gender"]) == 1)
        {
            hstInput.Add("chrGenderType", "F");
        }
        else if (common.myInt(Session["Gender"]) == 2)
        {
            hstInput.Add("chrGenderType", "M");
        }
        else
        {
            hstInput.Add("chrGenderType", "M");
        }
        ////if (Request.QueryString["ER"] != null && Request.QueryString["ER"].ToString() == "ER")
        ////{
        //   hstInput.Add("@intRegistrationId", common.myInt(Session["RegistrationId"]));
        //    hstInput.Add("@intEncounterId", common.myInt(ViewState["EncounterId"]) == 0 ? common.myInt(EncounterId) : common.myInt(ViewState["EncounterId"]));
        //}
        //else
        //{
        if (sEntryType == "S")
        {
            hstInput.Add("@intRegistrationId", common.myInt(Session["RegistrationId"]));
        }
        else
        {
            hstInput.Add("@intEncounterId", common.myInt(ViewState["EncounterId"]) == 0 ? common.myInt(EncounterId) : common.myInt(ViewState["EncounterId"]));
            hstInput.Add("@intRegistrationId", common.myInt(Session["RegistrationId"]));
        }
        //}
        hstInput.Add("@intFacilityId", common.myInt(Session["FacilityId"]));
        //char[] chr = { '|' };
        //string[] FromDate = tvCategory.SelectedNode.Value.Split(chr);
        //if (FromDate.LongLength == 6)
        //{
        //    hstInput.Add("@chrFromDate", Convert.ToDateTime(FromDate[4]).ToString("yyyy/MM/dd"));//sDate[0].SelectedDate.Value.ToString("yyyy/MM/dd"));
        //}

        hstInput.Add("@chrFromDate", Convert.ToDateTime(dtpFromDate.SelectedDate).ToString("yyyy/MM/dd"));

        hstInput.Add("@chrToDate", DateTime.Now.ToString("yyyy/MM/dd"));
        hstInput.Add("@intEREncounterId", Request.QueryString["EREncounterId"] == null ? "0" : Request.QueryString["EREncounterId"].ToString());
        DataSet dsAllSectionDetails = dl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetTemplateDetails", hstInput);

        string BeginList = "", EndList = "", BeginList2 = "", BeginList3 = "", EndList3 = "";
        int t = 0, t2 = 0, t3 = 0;

        DataView dv = new DataView(dsAllSectionDetails.Tables[2]);
        DataTable dtEntry = dv.ToTable(true, "RecordId");
        int iRecordId = 0;
        for (int it = 0; it < dtEntry.Rows.Count; it++)
        {
            foreach (DataRow item in ds.Tables[0].Rows)
            {
                DataTable dtFieldValue = new DataTable();
                DataView dv1 = new DataView(dsAllSectionDetails.Tables[0]);
                dv1.RowFilter = "SectionId=" + common.myInt(item["SectionId"]);
                DataTable dtFieldName = dv1.ToTable();
                if (dsAllSectionDetails.Tables.Count > 2)
                {
                    DataView dv2 = new DataView(dsAllSectionDetails.Tables[2]);
                    dv2.RowFilter = "RecordId=" + common.myInt(dtEntry.Rows[it]["RecordId"]);
                    dtFieldValue = dv2.ToTable();
                }

                DataSet dsAllFieldsDetails = new DataSet();
                dsAllFieldsDetails.Tables.Add(dtFieldName);
                dsAllFieldsDetails.Tables.Add(dtFieldValue);
                if (dsAllSectionDetails.Tables[0].Rows.Count > 0)
                {
                    if (dsAllSectionDetails.Tables.Count > 2)
                    {
                        if (dsAllSectionDetails.Tables[2].Rows.Count > 0) // add for education details
                        {
                            string sBegin = "", sEnd = "";
                            DataRow dr3;
                            dr3 = dsAllSectionDetails.Tables[0].Rows[0];
                            getabulerFontSize("Fields", ref sBegin, ref sEnd, dr3);
                            ViewState["iTemplateId"] = common.myInt(item["TemplateId"]);
                            str = CreateString(dsAllFieldsDetails, common.myInt(item["TemplateId"]), common.myStr(item["TemplateName"]), common.myStr(item["Tabular"]),
                                item["SectionId"].ToString(), common.myStr(item["EntryType"]), common.myInt(dtEntry.Rows[it]["RecordId"]));
                            str += " ";
                            if (iPrevId == common.myInt(item["TemplateId"]))
                            {
                                if (iRecordId != Convert.ToInt16(dtEntry.Rows[it]["RecordId"]))
                                {
                                    if (sEntryType == "M")
                                    {
                                        objStrTmp.Append("<br/><br/><b>" + common.myStr(dtFieldValue.Rows[0]["VisitDateTime"]) + "</b>");
                                    }
                                }
                                if (t2 == 0)
                                {
                                    if (t3 == 0)//Template
                                    {
                                        t3 = 1;
                                        if (common.myStr(item["SectionsListStyle"]) == "1")
                                        {
                                            BeginList3 = "<ul>"; EndList3 = "</ul>";
                                        }
                                        else if (common.myStr(item["SectionsListStyle"]) == "2")
                                        {
                                            BeginList3 = "<ol>"; EndList3 = "</ol>";
                                        }
                                    }
                                }
                                if (common.myStr(item["SectionsBold"]) != ""
                                    || common.myStr(item["SectionsItalic"]) != ""
                                    || common.myStr(item["SectionsUnderline"]) != ""
                                    || common.myStr(item["SectionsFontSize"]) != ""
                                    || common.myStr(item["SectionsForecolor"]) != ""
                                    || common.myStr(item["SectionsListStyle"]) != "")
                                {
                                    sBegin = ""; sEnd = "";
                                    MakeFont("Sections", ref sBegin, ref sEnd, item);
                                    if (Convert.ToBoolean(item["SectionDisplayTitle"]))   //19June2010
                                    {
                                        if (str.Trim() != "")
                                        {
                                            objStrTmp.Append(BeginList3 + sBegin + common.myStr(item["SectionName"]) + sEnd); //    objStrTmp.Append("<br />" + BeginList3 + sBegin + item["SectionName"].ToString() + sEnd);
                                        }
                                    }
                                    BeginList3 = "";
                                }
                                else
                                {
                                    if (Convert.ToBoolean(item["SectionDisplayTitle"]))    //19June
                                    {
                                        if (str.Trim() != "")
                                        {
                                            objStrTmp.Append(common.myStr(item["SectionName"])); //objStrTmp.Append("<br />" + item["SectionName"].ToString());
                                        }
                                    }
                                }

                                if (common.myStr(item["SectionsListStyle"]) == "3"
                                    || common.myStr(item["TemplateListStyle"]) == "0")
                                {
                                    objStrTmp.Append("<br />");
                                }
                                else
                                {
                                    if (str.Trim() != "")
                                    {
                                        objStrTmp.Append(str);
                                    }
                                }
                            }
                            else
                            {

                                if (t == 0)
                                {
                                    t = 1;
                                    if (common.myStr(item["TemplateListStyle"]) == "1")
                                    {
                                        BeginList = "<ul>"; EndList = "</ul>";
                                    }
                                    else if (common.myStr(item["TemplateListStyle"]) == "2")
                                    {
                                        BeginList = "<ol>"; EndList = "</ol>";
                                    }
                                }
                                if (common.myStr(item["TemplateBold"]) != ""
                                    || common.myStr(item["TemplateItalic"]) != ""
                                    || common.myStr(item["TemplateUnderline"]) != ""
                                    || common.myStr(item["TemplateFontSize"]) != ""
                                    || common.myStr(item["TemplateForecolor"]) != ""
                                    || common.myStr(item["TemplateListStyle"]) != "")
                                {
                                    sBegin = ""; sEnd = "";
                                    MakeFont("Template", ref sBegin, ref sEnd, item);
                                    if (Convert.ToBoolean(item["TemplateDisplayTitle"]))
                                    {
                                        if (sBegin.Contains("<br/>") == true)
                                        {
                                            sBegin = sBegin.Remove(0, 5);
                                            objStrTmp.Append(BeginList + sBegin + common.myStr(item["TemplateName"]) + sEnd + "<br/>");
                                        }
                                        else
                                        {
                                            objStrTmp.Append(BeginList + sBegin + common.myStr(item["TemplateName"]) + sEnd + "<br/>");
                                        }
                                    }
                                    if (sEntryType == "M" && str.Trim() != "")
                                    {
                                        objStrTmp.Append("<br/><br/><b> " + common.myStr(dtFieldValue.Rows[0]["VisitDateTime"]) + "</b>");
                                    }
                                    BeginList = "";
                                }
                                else
                                {
                                    if (Convert.ToBoolean(item["TemplateDisplayTitle"]))
                                    {
                                        objStrTmp.Append(common.myStr(item["TemplateName"]));//Default Setting
                                    }
                                    if (sEntryType == "M" && str.Trim() != "")
                                    {
                                        objStrTmp.Append("<br/><br/><b> " + common.myStr(dtFieldValue.Rows[0]["VisitDateTime"]) + "</b>");
                                    }
                                }
                                if (common.myStr(item["TemplateListStyle"]) == "3"
                                    || common.myStr(item["TemplateListStyle"]) == "0")
                                {
                                    objStrTmp.Append("<br />");
                                }
                                objStrTmp.Append(EndList);
                                if (t2 == 0)
                                {
                                    t2 = 1;
                                    if (common.myStr(item["SectionsListStyle"]) == "1")
                                    {
                                        BeginList2 = "<ul>"; EndList3 = "</ul>";
                                    }
                                    else if (common.myStr(item["SectionsListStyle"]) == "2")
                                    {
                                        BeginList2 = "<ol>"; EndList3 = "</ol>";
                                    }
                                }
                                if (common.myStr(item["SectionsBold"]) != ""
                                    || common.myStr(item["SectionsItalic"]) != ""
                                    || common.myStr(item["SectionsUnderline"]) != ""
                                    || common.myStr(item["SectionsFontSize"]) != ""
                                    || common.myStr(item["SectionsForecolor"]) != ""
                                    || common.myStr(item["SectionsListStyle"]) != "")
                                {
                                    sBegin = ""; sEnd = "";
                                    MakeFont("Sections", ref sBegin, ref sEnd, item);
                                    if (Convert.ToBoolean(item["SectionDisplayTitle"])) // Comment On 19June2010 hit1
                                    {
                                        if (str.Trim() != "") //add 19June2010
                                        {
                                            objStrTmp.Append(BeginList2 + sBegin + common.myStr(item["SectionName"]) + sEnd);
                                        }
                                    }
                                    BeginList2 = "";
                                }
                                else
                                {
                                    if (Convert.ToBoolean(item["SectionDisplayTitle"]))// Comment ON 19June2010
                                    {
                                        if (str.Trim() != "") //add 19June2010
                                        {
                                            objStrTmp.Append(common.myStr(item["SectionName"])); //Comment On 19June2010
                                        }
                                    }
                                }
                                if (common.myStr(item["SectionsListStyle"]) == "3"
                                    || common.myStr(item["SectionsListStyle"]) == "0")
                                {
                                    objStrTmp.Append("<br />");
                                }

                                objStrTmp.Append(str);
                            }
                            iRecordId = Convert.ToInt16(dtEntry.Rows[it]["RecordId"]);
                            iPrevId = common.myInt(item["TemplateId"]);
                        }
                    }
                }
            }
        }

        if (t2 == 1 && t3 == 1)
        {
            objStrTmp.Append(EndList3);
        }
        else
        {
            objStrTmp.Append(EndList);
        }
        sb.Append(objStrTmp.ToString());
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
                                            str.Replace("&lt", "<");
                                            str.Replace("&gt", ">");
                                            str.Append(" ");


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
                                                        common.myInt(dtEntry.Rows[it]["RecordId"]), GroupingDate, common.myBool(item["IsConfidential"])));

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
                            //Change By Himanshu On Date 28/03/2022 for Sanar Hospital ASC to DESC

                            dvFilter.Sort = "RowNum DESC";
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
                                    sEnd = "";
                                    MakeFont("Fields", ref sBegin, ref sEnd, item);
                                    if (Convert.ToBoolean(item["DisplayTitle"]))
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
                                else
                                {
                                    if (common.myStr(item["FieldType"]).Equals("H"))
                                    {
                                        sEnd = "";
                                        MakeFont("Fields", ref sBegin, ref sEnd, item);
                                        if (common.myBool(item["DisplayTitle"]))
                                        {
                                            objStr.Append(BeginList + sBegin + "<U>" + common.myStr(item["FieldName"]) + "</U>");

                                            if (objStr.ToString() != "")
                                            {
                                                objStr.Append(sEnd + "</li>");
                                            }
                                        }
                                        BeginList = "";
                                        sBegin = "";
                                    }
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
                                                    if (FType == "M")
                                                    {
                                                        objStr.Append(" " + common.myStr(objDt.Rows[i]["TextValue"]).Replace("\n", "<br/>"));
                                                    }
                                                    else
                                                    {
                                                        objStr.Append(" " + common.myStr(objDt.Rows[i]["TextValue"]));
                                                    }
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
                                        else if (FType == "O")
                                        {
                                            objStr.Append(sBegin + " : " + common.myStr(objDt.Rows[i]["TextValue"]) + sEnd);
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
                                    //    objStr.Append("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=' font-size:8pt;'>(Entered By: " + sEnterBy + " Date/Time: " + sVisitDate + ")</span>");
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
                }
                string sDisplayEnteredBy = common.myStr(Session["DisplayEnteredByInCaseSheet"]);
                string EnterByDate = string.Empty;

                if (sDisplayEnteredBy.Equals("Y")
                    || (sDisplayEnteredBy.Equals("N") && common.myStr(Session["OPIP"]).Equals("I") && !common.myStr(Session["ControlOnlyByDisplayEnteredByInCaseSheet"]).ToUpper().Equals("Y")))
                {
                    int EnterById = 0;
                    if ((objStr.ToString() != "" || IsConfidential == true) && bool.Parse(TabularType) == false)
                    {
                        DataView dvValues = new DataView(objDs.Tables[1]);
                        dvValues.RowFilter = "SectionId=" + common.myStr(sectionId);
                        if (dvValues.ToTable().Rows.Count > 0)
                        {
                            EnterById = common.myInt(dvValues.ToTable().Rows[0]["EnteredById"]);
                            if (ViewState["CurrentTemplateFontName"] != null && !common.myStr(ViewState["CurrentTemplateFontName"]).Equals(string.Empty))
                            {

                                objStr.Append("<b><br/>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style='font-family: " + common.myStr(ViewState["CurrentTemplateFontName"]) + "; font-size:8pt;'>Entered By: " + common.myStr(dvValues.ToTable().Rows[0]["EnterBy"]) + "</span></b>");
                                EnterByDate = " on " + common.myStr(dvValues.ToTable().Rows[0]["VisitDateTime"]);
                            }
                            else
                            {
                                objStr.Append("<b><br/>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style='font-family: Tahoma; font-size:8pt;'>Entered By: " + common.myStr(dvValues.ToTable().Rows[0]["EnterBy"]) + "</span></b>");
                                EnterByDate = " on " + common.myStr(dvValues.ToTable().Rows[0]["VisitDateTime"]);
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
                                EnterById = common.myInt(dvValues.ToTable().Rows[0]["EnteredById"]);
                                if (ViewState["CurrentTemplateFontName"] != null && !common.myStr(ViewState["CurrentTemplateFontName"]).Equals(string.Empty))
                                {
                                    //objStr.Append("<br/>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style='font-family: " + common.myStr(ViewState["CurrentTemplateFontName"]) + "; '>Entered By: " + common.myStr(dvValues.ToTable().Rows[0]["EnterBy"]) + " Date/Time: " + common.myStr(dvValues.ToTable().Rows[0]["VisitDateTime"]) + "</span><br/>");
                                    objStr.Append("<b><br/>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style='font-family: " + common.myStr(ViewState["CurrentTemplateFontName"]) + "; font-size:8pt;'>Entered By: " + common.myStr(dvValues.ToTable().Rows[0]["EnterBy"]) + "</span></b>");
                                    EnterByDate = " on " + common.myStr(dvValues.ToTable().Rows[0]["EntryDate"]);
                                }
                                else
                                {
                                    objStr.Append("<b><br/>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style='font-family: Tahoma; font-size:8pt;'>Entered By: " + common.myStr(dvValues.ToTable().Rows[0]["EnterBy"]) + "</span></b>");
                                    EnterByDate = " on " + common.myStr(dvValues.ToTable().Rows[0]["EntryDate"]);
                                }
                            }
                            dvValues.Dispose();
                        }
                    }

                    #region Footer Signature In Entered by
                    System.Collections.Generic.Dictionary<string, string> collHospitalSetupValues = new System.Collections.Generic.Dictionary<string, string>();

                    collHospitalSetupValues = common.GetFlagValueHospitalSetupMultiple(common.myInt(System.Web.HttpContext.Current.Session["HospitalLocationId"]), common.myInt(System.Web.HttpContext.Current.Session["FacilityId"]),
                            "IsShowSignatureWithEnteredByInCasesheet", sConString);

                    if (collHospitalSetupValues.ContainsKey("IsShowSignatureWithEnteredByInCasesheet"))
                    {
                        if (common.myStr(collHospitalSetupValues["IsShowSignatureWithEnteredByInCasesheet"]).Equals("Y"))
                        {
                            DataTable dt = new DataTable();
                            clsIVF objivf = new clsIVF(sConString);
                            clsExceptionLog objException = new clsExceptionLog();

                            try
                            {
                                //dt = objivf.getDoctorSignatureDetails(common.myInt(System.Web.HttpContext.Current.Session["DoctorId"]), common.myInt(System.Web.HttpContext.Current.Session["FacilityId"]), common.myInt(System.Web.HttpContext.Current.Session["HospitalLocationID"])).Tables[0];
                                dt = objivf.getDoctorSignatureDetails(EnterById, common.myInt(System.Web.HttpContext.Current.Session["FacilityId"]), common.myInt(System.Web.HttpContext.Current.Session["HospitalLocationID"])).Tables[0];

                                if (common.myStr(dt.Rows[0]["SignatureLine1"]).Trim().Length > 0)
                                {
                                    objStr.Append(" <b><span style='font-family: " + common.myStr(System.Web.HttpContext.Current.Session["CurrentTemplateFontName"]) + "; font-size:8pt;'> " + common.myStr(dt.Rows[0]["SignatureLine1"]).Trim() + "</span></b>");
                                }
                                if (common.myStr(dt.Rows[0]["SignatureLine2"]).Trim().Length > 0)
                                {
                                    objStr.Append(" <b><span style='font-family: " + common.myStr(System.Web.HttpContext.Current.Session["CurrentTemplateFontName"]) + "; font-size:8pt;'> ," + common.myStr(dt.Rows[0]["SignatureLine2"]).Trim() + "</span></b>");
                                }
                                if (common.myStr(dt.Rows[0]["SignatureLine3"]).Trim().Length > 0)
                                {
                                    objStr.Append(" <b><span style='font-family: " + common.myStr(System.Web.HttpContext.Current.Session["CurrentTemplateFontName"]) + "; font-size:8pt;'> ," + common.myStr(dt.Rows[0]["SignatureLine3"]).Trim() + "</span></b>");
                                }
                                if (common.myStr(dt.Rows[0]["SignatureLine4"]).Trim().Length > 0)
                                {
                                    objStr.Append(" <b><span style='font-family: " + common.myStr(System.Web.HttpContext.Current.Session["CurrentTemplateFontName"]) + "; font-size:8pt;'> ," + common.myStr(dt.Rows[0]["SignatureLine4"]).Trim() + "</span></b>");
                                }
                            }
                            catch (Exception Ex)
                            {
                                objException.HandleException(Ex);
                            }
                            finally
                            {
                                dt.Dispose();
                                objivf = null;
                                objException = null;
                            }
                        }
                    }
                    #endregion
                    objStr.Append(" <b><span style='font-family: " + common.myStr(System.Web.HttpContext.Current.Session["CurrentTemplateFontName"]) + "; font-size:8pt;'> " + EnterByDate + "</span></b>");
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
    public string getDefaultFontSize()
    {
        string sFontSize = "";
        string FieldValue = "";
        ClinicDefaults cd = new ClinicDefaults(Page);
        BaseC.EMRMasters.Fonts fonts = new BaseC.EMRMasters.Fonts();
        FieldValue = cd.GetHospitalDefaults("DefaultFontSize", common.myStr(Session["HospitalLocationId"]));
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
        ViewState["CurrentTemplateFontName"] = string.Empty;
        ViewState["CurrentTemplateFontName"] = FontName;
        if (FontName != "")
        {
            sBegin += " font-family: " + FontName + ";";
        }
        else
        {
            FieldValue = cd.GetHospitalDefaults("DefaultFontType", common.myStr(Session["HospitalLocationId"]));
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
    string sFontSize = "";
    protected string getabulerFontSize(string typ, ref string sBegin, ref string sEnd, DataRow item)
    {
        string sFontSize = "";
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

    protected void btnRefresh_OnClick(object sender, EventArgs e)
    {
        RTF1.Content = "";
        bindTemplates();
    }

    private string bindPharmacyData(int EncounterId)
    {
        string[] sDate = getFromAndToDate(string.Empty); //getFromAndToDate(common.myStr(ddlDateRange.SelectedValue));
        BindNotes note = new BindNotes(sConString);
        string sTemp = "";
        try
        {
            sTemp = note.bindVisitNotesPharmacy(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]),
               common.myInt(Session["RegistrationId"]), EncounterId, sDate[0], sDate[1]);
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            sDate = null;
            note = null;
        }
        return sTemp;
    }

    protected void gvDetails_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        DataSet ds = new DataSet();
        try
        {
            if (common.myStr(e.CommandName).ToUpper().Equals("VISITDETAIL"))
            {
                if (common.myInt(e.CommandArgument) > 0)
                {
                    GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    LinkButton lnkVisitDetail = (LinkButton)(row.FindControl("lnkVisitDetail"));
                    HiddenField hdnTemplateType = (HiddenField)(row.FindControl("hdnTemplateType"));
                    //HiddenField hdnEncounterId = (HiddenField)(row.FindControl("hdnEncounterId"));
                    //HiddenField hdnGroupId = (HiddenField)(row.FindControl("hdnGroupId"));

                    ViewState["templateid_new"] = common.myInt(e.CommandArgument).ToString();
                    ViewState["EncounterId"] = common.myInt(Session["EncounterId"]).ToString();
                    ViewState["TemplateName"] = common.myStr(lnkVisitDetail.Text);
                    ViewState["TemplateType"] = common.myStr(hdnTemplateType.Value);
                    //ViewState["GroupId"] = common.myInt(hdnGroupId.Value).ToString();

                    row.BackColor = System.Drawing.Color.LightPink;

                    if (common.myInt(ViewState["templateid_new"]) > 0)
                    {
                        foreach (GridViewRow rowGV in gvDetails.Rows)
                        {
                            rowGV.BackColor = System.Drawing.Color.White;
                        }
                        bindDetails();
                    }

                    row.BackColor = System.Drawing.Color.LightPink;
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
            ds.Dispose();
        }
    }

    protected void lnkBtnViewCaseSheet_OnClick(object sender, EventArgs e)
    {
        try
        {
            RadWindowForNew.NavigateUrl = "/Editor/WordProcessor.aspx?From=POPUP" +
                                        "&DoctorId=" + common.myInt(Session["DoctorID"]) +
                                        "&OPIP=" + common.myStr(Session["OPIP"]) +
                                        "&EncounterDate=" + common.myDate(Session["EncounterDate"]).ToString("yyyy/MM/dd");

            RadWindowForNew.Width = 1200;
            RadWindowForNew.Height = 630;
            RadWindowForNew.Top = 10;
            RadWindowForNew.Left = 10;
            //RadWindowForNew.OnClientClose = "";
            RadWindowForNew.Modal = true;
            RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
            RadWindowForNew.VisibleStatusbar = false;
            RadWindowForNew.InitialBehaviors = WindowBehaviors.Maximize;
        }
        catch
        {
        }
    }

    private void bindAttachments()
    {
        DataSet ds = new DataSet();
        BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
        try
        {
            ds = objEMR.getExternalCenterLabAttachments(common.myInt(Session["FacilityId"]), common.myInt(Session["RegistrationId"]), common.myInt(Session["EncounterId"]));

            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count == 0)
                {
                    DataRow DR = ds.Tables[0].NewRow();
                    ds.Tables[0].Rows.Add(DR);
                    ds.AcceptChanges();
                }
            }
            gvAttachments.DataSource = ds.Tables[0];
            gvAttachments.DataBind();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
        finally
        {
            ds.Dispose();
            objEMR = null;
        }
    }

    protected void gvAttachments_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        DataSet ds = new DataSet();
        try
        {
            if (e.CommandName.ToUpper() == "VISITDETAIL")
            {
                if (common.myInt(e.CommandArgument) > 0)
                {
                    GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    //LinkButton lnkServiceName = (LinkButton)(row.FindControl("lnkServiceName"));
                    HiddenField hdnAttachmentOption = (HiddenField)(row.FindControl("hdnAttachmentOption"));
                    HiddenField hdnEncounterId = (HiddenField)(row.FindControl("hdnEncounterId"));
                    HiddenField hdnDiagSampleId = (HiddenField)(row.FindControl("hdnDiagSampleId"));
                    HiddenField hdnServiceId = (HiddenField)(row.FindControl("hdnServiceId"));
                    HiddenField hdnDocumentName = (HiddenField)(row.FindControl("hdnDocumentName"));

                    row.BackColor = System.Drawing.Color.LightPink;

                    if ((common.myInt(hdnDiagSampleId.Value) > 0 && common.myStr(hdnAttachmentOption.Value).ToUpper().Equals("LAB"))
                        || (common.myStr(hdnAttachmentOption.Value).ToUpper().Equals("EMR")))
                    {
                        foreach (GridViewRow rowGV in gvAttachments.Rows)
                        {
                            rowGV.BackColor = System.Drawing.Color.White;
                        }
                        BaseC.EncryptDecrypt en = new BaseC.EncryptDecrypt();
                        string key = "Word";
                        string sFileName = common.myStr(hdnDocumentName.Value);
                        string sSavePath = ConfigurationManager.AppSettings["LabResultPath"];

                        if (common.myStr(hdnAttachmentOption.Value).ToUpper().Equals("EMR"))
                        {
                            sSavePath = string.Empty;
                        }

                        string path = Server.MapPath(sSavePath + sFileName);
                        string URLPath = "/Editor/LabAttachmentOpen.aspx?FTPFolder=@FTPFolder&FileName=@FileName";
                        URLPath = URLPath.Replace("@FTPFolder", en.Encrypt(sSavePath, key, true, string.Empty)).Replace("@FileName", en.Encrypt(sFileName, key, true, string.Empty));
                        RadWindowForNew.NavigateUrl = URLPath.Replace("+", "%2B");

                        //RadWindowForNew.NavigateUrl = "/Editor/LabAttachmentOpen.aspx?AttachmentOption=" + common.myStr(hdnAttachmentOption.Value) +
                        // "&DocumentName=" + common.myStr(hdnDocumentName.Value);

                        RadWindowForNew.Width = 1200;
                        RadWindowForNew.Height = 630;
                        RadWindowForNew.Top = 10;
                        RadWindowForNew.Left = 10;
                        //RadWindowForNew.OnClientClose = "";
                        RadWindowForNew.Modal = true;
                        RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
                        RadWindowForNew.VisibleStatusbar = false;
                        RadWindowForNew.InitialBehaviors = WindowBehaviors.Maximize;
                    }

                    row.BackColor = System.Drawing.Color.LightPink;
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
            ds.Dispose();
        }
    }

    protected void btnViewSelectedTemplate_OnClick(object sender, EventArgs e)
    {
        gvDetails.Enabled = false;
        btnRefresh.Enabled = false;
        try
        {
            //   RTF1.Content = BindEditor(true, true);
            if (common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]),
                                                 common.myInt(Session["FacilityId"]), "ShowSignatureFooterLine", sConString).Equals("Y"))
            {
                RTF1.Content = BindEditor(true, true);
            }
            else
            {
                RTF1.Content = BindEditor(false, true);
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
            gvDetails.Enabled = true;
            btnRefresh.Enabled = true;
        }
    }

    protected void btnPrintPDF_Click(object sender, EventArgs e)
    {
        if (RTF1.Content.ToString() == string.Empty)
        {
            Alert.ShowAjaxMsg("Please select the template", this.Page);
            return;
        }
        if (common.myStr(Session["OPIP"]) == "I" && common.myBool(Session["EnablePrintCaseSheet"]) == false
            && common.myStr(Request.QueryString["callby"]) != "mrd")
        {
            if (common.myStr(Request.QueryString["OPIP"]) == "I")
            {
                Alert.ShowAjaxMsg("You are not Authorise to print IP Clinical Data", Page);
                return;
            }
        }
        if (common.myStr(Request.QueryString["OPIP"]) == "I" && common.myBool(Session["EnablePrintCaseSheet"]) == false
            && common.myStr(Request.QueryString["callby"]) == "mrd")
        {
            Alert.ShowAjaxMsg("You are not Authorise to print IP Clinical Data", Page);
            return;
        }
        Session["RTF"] = RTF1.Content.ToString().Replace("src=\"/PatientDocuments/DoctorImages/", "src=\"" + Server.MapPath("~") + @"PatientDocuments\DoctorImages\");
        // Session["RTFCaseSheetPrintReportHeader"] = RTF1.Content.ToString().Replace("src=\"/PatientDocuments/DoctorImages/", "src=\"" + Server.MapPath("~") + @"PatientDocuments\DoctorImages\");
        RadWindowForNew.NavigateUrl = "~/Editor/PrintPdf.aspx";
        RadWindowForNew.Height = 550;
        RadWindowForNew.Width = 1000;
        RadWindowForNew.Top = 10;
        RadWindowForNew.Left = 10;
        RadWindowForNew.Behaviors = WindowBehaviors.Maximize | WindowBehaviors.Minimize | WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Pin;
        RadWindowForNew.Modal = true;
        RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
        RadWindowForNew.VisibleStatusbar = false;
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
        string FontName = string.Empty;
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

                    if (ViewState["CurrentTemplateFontName"] != null && !common.myStr(ViewState["CurrentTemplateFontName"]).Equals(string.Empty))
                    {
                        // objStr.Append("<br/>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style='font-family: " + common.myStr(ViewState["CurrentTemplateFontName"]) + "; '>Entered By: " + common.myStr(dvValues.ToTable().Rows[0]["EnterBy"]) + " Date/Time: " + common.myStr(dvValues.ToTable().Rows[0]["VisitDateTime"]) + "</span><br/>");
                        hdnDoctorImage.Value = DivStartTag + "<table border='0' width='99%' cellpadding='2' cellspacing='2' style='font-size:10pt; font-family:" + common.myStr(ViewState["CurrentTemplateFontName"]) + ";'><tbody><tr><td align='right'>" + SignImage + "</td></tr><tr><td align='right'><b>Consultant&nbsp;Signature</b></td></tr><tr><td>" + SignNote + "</td></tr></tbody></table><br />";

                    }
                    else
                    {
                        hdnDoctorImage.Value = DivStartTag + "<table border='0' width='99%' cellpadding='2' cellspacing='2' style='font-size:10pt; font-family:Tahoma;'><tbody><tr><td align='right'>" + SignImage + "</td></tr><tr><td align='right'><b>Consultant&nbsp;Signature</b></td></tr><tr><td>" + SignNote + "</td></tr></tbody></table><br />";
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

    public void IsCopyCaseSheetAuthorized()
    {
        BaseC.Security objSecurity = new BaseC.Security(sConString);
        bool IsAuthorized = objSecurity.IsCopyCaseSheetAuthorized(common.myInt(Session["UserID"]), common.myInt(Session["HospitalLocationID"]));
        hdnIsCopyCaseSheetAuthorized.Value = common.myStr(IsAuthorized);
    }

    //Add By himanshu On Date 22/03/2022
    protected void chk_DoctorTemplet_CheckedChanged(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();
        DataView dv;
        if (chk_DoctorTemplet.Checked == true)
        {

            dt = (DataTable)ViewState["TemplateDetail"];
            dv = new DataView(dt);
            dv.RowFilter = "(EmployeeType='D')";
            gvDetails.DataSource = dv;
            gvDetails.DataBind();
            // dvDataFilter.RowFilter = "(TemplateType='D' AND TemplateId IN (" + sbDynamicTemplateIds.ToString() + ")) OR (TemplateType='S' AND PageId IN (" + sbStaticTemplateIds.ToString() + "))";
        }
        else
        {
            bindTemplates();
        }

    }
}
