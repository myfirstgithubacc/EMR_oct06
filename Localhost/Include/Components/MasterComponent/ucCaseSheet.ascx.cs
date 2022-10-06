using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using Telerik.Web.UI;
using System.Configuration;
using System.IO;
using System.Web;

public partial class Include_Components_MasterComponent_ucCaseSheet : System.Web.UI.UserControl
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    // shaivee 
    private string ftppath = ConfigurationManager.AppSettings["FTP"];
    private string FileFolder = ConfigurationManager.AppSettings["FileFolder"];
    //shaivee end
    //private Hashtable hstInput;
    clsExceptionLog objException = new clsExceptionLog();
    //private int iPrevId = 0;
    //StringBuilder sb = new StringBuilder();
    //DAL.DAL dl;
    //string Fonts = string.Empty;
    int RegId;
    int HospitalId;
    int EncounterId;
    int UserId;
    string path = string.Empty; public string RTF1Content = string.Empty;
    string FromDate = string.Empty;
    string ToDate = string.Empty;
    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";


    }
    protected void Page_Load(object sender, EventArgs e)
    {
        string url = HttpContext.Current.Request.Url.AbsoluteUri;

        path = url.Replace("http://", "");
        path = "http://" + path.Substring(0, path.IndexOf("/") + 1);
        FromDate = common.myStr(Session["EncounterDate"]).Trim();
        ToDate = common.myDate(DateTime.Now).ToString("yyyy/MM/dd");


        if (Request.QueryString["Source"] != null)
        {
            if (common.myStr(Request.QueryString["Source"]).Equals("NurseWorkbench"))
            {
                if (Session["RegistrationID"] != null && Session["RegistrationNo"] != null && Session["EncounterId"] != null &&
                   Session["EncounterNo"] != null)
                {

                    hdnRegid.Value = common.myStr(Session["RegistrationID"]);
                    hdnRegNo.Value = common.myStr(Session["RegistrationNo"]);
                    hdnEncId.Value = common.myStr(Session["EncounterId"]);
                    hdnEncNo.Value = common.myStr(Session["EncounterNo"]);
                    hdnFromWard.Value = common.myStr("Y");
                    hdnOP_IP.Value = "I";
                    hdnCategory.Value = string.Empty;
                    hdnCloseButtonShow.Value = "No";
                    hdnFromEMR.Value = "1";
                }
            }
        }
        else
        {
            hdnRegid.Value = common.myStr(Request.QueryString["Regid"]);
            hdnRegNo.Value = common.myStr(Request.QueryString["RegNo"]);
            hdnEncId.Value = common.myStr(Request.QueryString["EncId"]);
            hdnEncNo.Value = common.myStr(Request.QueryString["EncNo"]);
            hdnFromWard.Value = common.myStr(Request.QueryString["FromWard"]);
            hdnOP_IP.Value = common.myStr(Request.QueryString["OP_IP"]);
            hdnCategory.Value = common.myStr(Request.QueryString["Category"]);
            hdnCloseButtonShow.Value = common.myStr(Request.QueryString["CloseButtonShow"]);
            hdnFromEMR.Value = common.myStr(Request.QueryString["FromEMR"]);
        }

        Session["RegistrationId"] = hdnRegid.Value;
        if (Session["UserID"] == null)
        {
            Response.Redirect("/Login.aspx?Logout=1", false);
        }
        lblMessage.Text = string.Empty;
        if (Request.QueryString["Mpg"] != null)
        {
            Session["CurrentNode"] = common.myStr(Request.QueryString["Mpg"]);
        }
        if (common.myStr(Session["RegistrationId"]) == "")
        {
            return;
        }
        if (common.myStr(hdnEncId.Value).Trim() != "")
        {
            EncounterId = common.myInt(hdnEncId.Value);
            ViewState["EncounterId"] = EncounterId;
        }
        else if (common.myStr(Session["EncounterId"]) != "")
        {
            EncounterId = common.myInt(Session["EncounterId"]);
            ViewState["EncounterId"] = EncounterId;
        }
        if (!IsPostBack)
        {


            if (hdnFromEMR.Value != null)
            {
                if (common.myStr(hdnFromEMR.Value) != string.Empty)
                {
                    if (common.myStr(hdnFromEMR.Value).Equals("1"))
                    {
                        asplHeaderUDEMR.Visible = true;
                        asplHeaderUD.Visible = false;
                    }
                    else
                    {
                        asplHeaderUDEMR.Visible = false;
                        asplHeaderUD.Visible = true;
                    }

                }
                else
                {
                    asplHeaderUDEMR.Visible = false;
                    asplHeaderUD.Visible = true;
                }

            }
            else
            {
                asplHeaderUDEMR.Visible = false;
                asplHeaderUD.Visible = true;
            }



            setOnPageLoad();
            BindTreeColapse();
            BindProvider();
            dtpFromDate.DateInput.DateFormat = Application["OutputDateFormat"].ToString();
            dtpFromDate.SelectedDate = System.DateTime.Now;
            dtpToDate.DateInput.DateFormat = Application["OutputDateFormat"].ToString();

            ViewState["EMRPoliceIntimationTemplateId"] = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]),
                                                common.myInt(Session["FacilityId"]), "EMRPoliceIntimationTemplateId", sConString);
            dtpToDate.SelectedDate = System.DateTime.Now;
            if (common.myStr(hdnCloseButtonShow.Value) == "No")
            {
                btnClose.Visible = false;
            }

            if (Request.QueryString["From"] != null)
            {
                if (common.myStr(Request.QueryString["From"]).Equals("CE"))
                {
                    ddlDoctor.SelectedValue = common.myStr(Session["DoctorId"]);
                    if (Request.QueryString["TemplateidCE"] != null)
                    {
                        ddlTemplate.SelectedValue = common.myStr(Request.QueryString["TemplateidCE"]);
                    }
                    rdoViewType.SelectedValue = "T";
                    btnRefresh_OnClick(null, null);
                    GetChildNodeFirstValue();
                    //  setSelectedNode("C1304|5952|IP|Dr. Vaneet Parmar|3/20/2017 12:00:00 AM|0", "");
                    if (ViewState["GetChildNodeFirstValue"] != null)
                    {
                        setSelectedNode(common.myStr(ViewState["GetChildNodeFirstValue"]), "");
                        RTF1.Content = BindEditor(true, true);
                        tvCategory.ExpandAll();
                    }

                    //if (tvCategory.Nodes.Count > 0)
                    //{
                    //    //tvCategory.SelectedNode= tvCategory.Nodes[0];
                    //    //tvCategory.SelectedNode = tvCategory.Nodes[0];
                    //    //TreeNode node = tvCategory.Nodes[0].FirstNode;
                    //    //tvCategory.SelectedNode = node;

                    //    //if (ViewState["SelectedCategory"] != null)
                    //    //{
                    //        TreeNode node1 = SelectNode(ViewState["SelectedCategory"].ToString(), tvCategory.Nodes);
                    //        if (node1 != null)
                    //        {
                    //            node1.Selected = true;
                    //        }
                    //        node1 = null;
                    //    //}
                    //    //else
                    //    //{
                    //    //    tvCategory.PopulateNodesFromClient = true;
                    //    //    tvCategory.ShowLines = true;
                    //    //}
                    //}
                }

            }
            //if (Request.QueryString["Category"] != null && !common.myStr(Request.QueryString["Category"]).Equals(string.Empty))
            //{
            //    if (common.myStr(Request.QueryString["Category"]).Equals("PopUp"))
            //    {
            //        btnClose.Visible = true;
            //    }

            //}

        }

    }

    private void AddChildNodeFirstValue(TreeView tvName, string iNodeID, int iParentID, string sNodeText, int TemplateId,
        string VisitType, string DoctorName, DateTime VisitDate, int GrouId)
    {
        try
        {

            if (common.myStr(rdoViewType.SelectedItem.Value) == "T")
            {
                TreeNode masternode = new TreeNode();


                if (masternode != null)
                {
                    TreeNode childNode = new TreeNode(sNodeText.ToString(), "C" + iNodeID.ToString() + "|" + TemplateId.ToString() + "|" + VisitType.ToString() + "|" + DoctorName.ToString() + "|" + VisitDate.ToString() + "|" + GrouId.ToString());
                    masternode.ChildNodes.Add(childNode);
                    ViewState["GetChildNodeFirstValue"] = childNode.Value;
                }

                tvName.CollapseAll();

            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private void GetChildNodeFirstValue()
    {
        DataSet ds = new DataSet();
        BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
        string[] sDateRange = getFromAndToDate(common.myStr(ddlDateRange.SelectedValue));
        try
        {
            //tvCategory.Nodes.Clear();
            ds = objEMR.getEMRPatientVisits(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), common.myInt(Session["RegistrationId"]),
                                            common.myStr(rdoViewType.SelectedValue), common.myInt(ddlDoctor.SelectedValue),
                                            common.myInt(ddlTemplate.SelectedValue), sDateRange[0], sDateRange[1]);
            if (ds.Tables.Count > 0)
            {
                //for (int i = 0; i < 1; i++)
                //{
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[1];
                    AddChildNodeFirstValue(tvCategory, common.myStr(dr["EncounterId"]), common.myInt(dr["ParentId"]), common.myStr(dr["VisitDetail"]),
                          common.myInt(dr["TemplateId"]), common.myStr(dr["VisitType"]), common.myStr(dr["DoctorName"]), common.myDate(dr["VisitDate"]), common.myInt(dr["GroupId"]));
                    dr = null;
                }
                //}
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    public void setOnPageLoad()
    {
        try
        {

            BindCategoryTree();
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            if (common.myStr(Session["RegistrationId"]) == "")
            {
                return;
            }

            EncounterId = common.myInt(Session["EncounterId"]);



            ViewState["RequestTemplateId"] = common.myInt(Request.QueryString["RequestTemplateId"]);
            BindPatientHiddenDetails();

            Session["SelectFindPatient"] = null;
            GetFonts();
            bindTemplateControl();
            RTF1.EditModes = EditModes.Preview;
            IsCopyCaseSheetAuthorized();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    void BindPatientHiddenDetails()
    {
        if (Session["PatientDetailString"] != null)
        {
            //lblPatientDetail.Text = Session["PatientDetailString"].ToString();
        }
    }
    private void bindDetails()
    {
        try
        {
            RTF1.Content = BindEditor(true, false);
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }
    private string[] getFromAndToDate(string ddlDateRange)
    {
        string[] sDate = new string[2];

        if (ddlDateRange == "DR")
        {
            if (dtpFromDate.SelectedDate != null && dtpToDate.SelectedDate != null)
            {
                sDate[0] = common.myDate(dtpFromDate.SelectedDate).ToString("yyyy/MM/dd");
                sDate[1] = common.myDate(dtpToDate.SelectedDate).ToString("yyyy/MM/dd");
            }
        }
        else if (ddlDateRange == "LTM")
        {
            sDate[0] = DateTime.Now.AddMonths(-3).ToString("yyyy/MM/dd");
            sDate[1] = DateTime.Now.ToString("yyyy/MM/dd");
        }
        else if (ddlDateRange == "LSM")
        {
            sDate[0] = DateTime.Now.AddDays(-6).ToString("yyyy/MM/dd");
            sDate[1] = DateTime.Now.ToString("yyyy/MM/dd");
        }
        else if (ddlDateRange == "LOY")
        {
            sDate[0] = DateTime.Now.AddYears(-1).ToString("yyyy/MM/dd");
            sDate[1] = DateTime.Now.ToString("yyyy/MM/dd");
        }
        else if (ddlDateRange == "LTY")
        {
            sDate[0] = DateTime.Now.AddYears(-3).ToString("yyyy/MM/dd");
            sDate[1] = DateTime.Now.ToString("yyyy/MM/dd");
        }
        else
        {
            sDate[0] = DateTime.Now.AddYears(-3).ToString("yyyy/MM/dd");
            sDate[1] = DateTime.Now.ToString("yyyy/MM/dd");
        }
        return sDate;
    }
    private void bindTemplateControl()
    {
        DataSet ds = new DataSet();
        clsIVF objIVF = new clsIVF(sConString);
        try
        {
            ds = objIVF.getEMRTemplate(common.myInt(Session["HospitalLocationId"]), "", 0, 0, "", 0, "PN", common.myInt(Session["FacilityID"]), false);
            ddlTemplate.DataSource = ds.Tables[0];
            ddlTemplate.DataTextField = "TemplateName";
            ddlTemplate.DataValueField = "TemplateId";
            ddlTemplate.DataBind();
            ddlTemplate.Items.Insert(0, new RadComboBoxItem("Select All", "0"));
            ddlTemplate.SelectedIndex = 0;
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
            objIVF = null;
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


    //private string BindEditor()
    //{
    //    StringBuilder sb = new StringBuilder();
    //    StringBuilder sbTemplateStyle = new StringBuilder();
    //    StringBuilder TemplateString;
    //    DataSet ds = new DataSet();
    //    DataSet dsTemplateStyle = new DataSet();
    //    DataRow drTemplateStyle = null;
    //    DataTable dtTemplate = new DataTable();
    //    DataView dvDataFilter = new DataView();
    //    DataTable dtEncounter = new DataTable();

    //    string Templinespace = "";
    //    BindNotes bnotes = new BindNotes(sConString);
    //    BaseC.DiagnosisDA fun = new BaseC.DiagnosisDA(sConString);
    //    BaseC.clsEMR emr = new BaseC.clsEMR(sConString);
    //    DataTable dtGroupDateWiseTemplate = new DataTable();
    //    StringBuilder sbTemp = new StringBuilder();
    //    bool bAllergyDisplay = false;
    //    bool bPatientBookingDisplay = false;

    //    int ddlTemplatePatient = 0;
    //    //string sTemplateName = common.myStr(ddlTemplatePatient) == "ALL" ? "" : common.myStr(ddlTemplatePatient);
    //    string sTemplateName = "";
    //    #region Declare DataSet
    //    DataSet dsTemplateData = new DataSet();
    //    #endregion
    //    #region Call Bind Case Sheet class
    //    BindCaseSheet BindCaseSheet = new BindCaseSheet(sConString);
    //    #endregion
    //    try
    //    {
    //        string DoctorId = fun.GetDoctorId(common.myInt(Session["HospitalLocationID"]), Convert.ToInt16(common.myInt(Session["UserID"])));
    //        dsTemplateStyle = bnotes.GetTemplateStyle(common.myInt(Session["HospitalLocationId"]));

    //        #region Call Bind Case Sheet method to get data
    //        if (ddlTemplatePatient != 0)
    //        {
    //            //dsTemplateData = emr.getEMRPrintCaseSheetDate(common.myInt(Session["HospitalLocationId"]),
    //            //    common.myInt(Session["FacilityId"]), common.myInt(ViewState["RegistrationId"]), common.myInt(ViewState["EncounterId"]), common.myDate(txtFromDate.SelectedDate.Value).ToString("yyyy/MM/dd"),
    //            //    common.myDate(txtToDate.SelectedDate.Value).ToString("yyyy/MM/dd"), sTemplateName, common.myInt(ddlTemplatePatient.SelectedValue),
    //            //    common.myStr(ddlTemplatePatient.SelectedItem.Attributes["TemplateType"]), chkChronologicalOrder.Checked, 0);
    //        }
    //        else
    //        {
    //            dsTemplateData = emr.getEMRPrintCaseSheetDate(common.myInt(Session["HospitalLocationId"]),
    //                            common.myInt(Session["FacilityId"]), common.myInt(Session["RegistrationId"]), common.myInt(Session["EncounterId"]), common.myDate(DateTime.Now).AddDays(-365).ToString("yyyy/MM/dd"),
    //                             common.myDate(DateTime.Now).ToString("yyyy/MM/dd"), sTemplateName, 0, "", false, 0);
    //        }
    //        #endregion

    //        dtGroupDateWiseTemplate = emr.GetDateWiseGroupingTemplate(common.myInt(Session["HospitalLocationId"]),
    //            common.myInt(Session["FacilityId"]), common.myInt(Session["RegistrationId"]), common.myInt(Session["EncounterId"]), common.myDate(DateTime.Now).AddDays(-365).ToString("yyyy/MM/dd"),
    //                             common.myDate(DateTime.Now).ToString("yyyy/MM/dd"));
    //        dvDataFilter = new DataView(dsTemplateData.Tables[21]);
    //        dtEncounter = dsTemplateData.Tables[22];
    //        for (int iEn = 0; iEn < dtEncounter.Rows.Count; iEn++)
    //        {
    //            if (dvDataFilter.ToTable().Rows.Count > 0)
    //            {
    //                ViewState["iPrevId"] = null;
    //                ViewState["iTemplateId"] = null;
    //                #region Chronological Order Wise

    //                #endregion
    //                #region Template Wise
    //                //else
    //                //{
    //                dtTemplate = dvDataFilter.ToTable();
    //                TemplateString = new StringBuilder();
    //                for (int i = 0; i < dtTemplate.Rows.Count; i++)
    //                {
    //                    #region Admission Request
    //                    if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim() == "AdmissionRequest"
    //                            && common.myStr(dtTemplate.Rows[i]["TemplateType"]).Trim() == "S" && bPatientBookingDisplay == false)
    //                    {
    //                        sbTemplateStyle = new StringBuilder();
    //                        DataView dv = new DataView(dsTemplateStyle.Tables[0]);
    //                        dv.RowFilter = "PageId=" + common.myInt(dtTemplate.Rows[i]["PageId"]).ToString();
    //                        if (dv.Count > 0)
    //                        {
    //                            drTemplateStyle = dv[0].Row;
    //                            string sBegin = "", sEnd = "";
    //                            Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
    //                            MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
    //                        }
    //                        dv.Dispose();
    //                        #region Call Bind Patient Booking
    //                        sbTemp = new StringBuilder();
    //                        BindCaseSheet.BindPatientBooking(dsTemplateData.Tables[20], sbTemp, sbTemplateStyle, drTemplateStyle,
    //                                           Page, common.myStr(dtTemplate.Rows[i]["PageId"]), "");
    //                        if (sbTemp.ToString() != "")
    //                            TemplateString.Append(sbTemp + "<br/>" + "<br/>");
    //                        bPatientBookingDisplay = true;
    //                        #endregion
    //                    }
    //                    #endregion
    //                    #region Chief Complaints
    //                    if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim() == "Chief Complaints"
    //                         && common.myStr(dtTemplate.Rows[i]["TemplateType"]).Trim() == "S")
    //                    {
    //                        sbTemplateStyle = new StringBuilder();
    //                        DataView dv = new DataView(dsTemplateStyle.Tables[0]);
    //                        dv.RowFilter = "PageId=" + common.myInt(dtTemplate.Rows[i]["PageId"]).ToString();
    //                        if (dv.Count > 0)
    //                        {
    //                            drTemplateStyle = dv[0].Row;
    //                            string sBegin = "", sEnd = "";


    //                            Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
    //                            MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);

    //                        }
    //                        dv.Dispose();
    //                        sbTemp = new StringBuilder();
    //                        if ((common.myInt(ddlTemplatePatient) == 0)
    //                             || (common.myInt(ddlTemplatePatient) == 98))
    //                        {
    //                            #region Call Bind Problem data
    //                            BindCaseSheet.BindProblemsHPI(dsTemplateData.Tables[0], common.myInt(ViewState["RegistrationId"]), common.myInt(dtEncounter.Rows[iEn]["EncounterId"]), sbTemp, sbTemplateStyle, drTemplateStyle,
    //                                           Page, common.myStr(dtTemplate.Rows[i]["PageId"]), "");
    //                            #endregion
    //                            if (sbTemp.ToString() != "")
    //                                //////TemplateString.Append(sbTemp + "<br/>" + "<br/>");
    //                                TemplateString.Append(sbTemp + "<br/>");
    //                            sbTemp = null;
    //                            sbTemplateStyle = null;
    //                        }

    //                        drTemplateStyle = null;
    //                        Templinespace = "";
    //                    }
    //                    #endregion
    //                    #region Template History Type
    //                    if (common.myStr(dtTemplate.Rows[i]["TemplateId"]).Trim() != ""
    //                       && common.myStr(dtTemplate.Rows[i]["TemplateType"]).Trim() == "D"
    //                       && common.myStr(dtTemplate.Rows[i]["TemplateCode"]).Trim() == "HIS")
    //                    {
    //                        DataView dv = new DataView(dsTemplateStyle.Tables[0]);
    //                        dv.RowFilter = "PageId=" + common.myInt(dtTemplate.Rows[i]["TemplateId"]).ToString();
    //                        if (dv.Count > 0)
    //                        {
    //                            drTemplateStyle = dv[0].Row;
    //                            string sBegin = "", sEnd = "";
    //                            Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
    //                            MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
    //                        }
    //                        dv.Dispose();
    //                        sbTemp = new StringBuilder();
    //                        if ((common.myInt(ddlTemplatePatient) == 0)
    //                            || (common.myInt(ddlTemplatePatient) == common.myInt(dtTemplate.Rows[i]["TemplateId"])))
    //                        {
    //                            #region Assign Data and call History Type Dynamic Template
    //                            DataSet dsDymanicTemplateData = new DataSet();

    //                            DataView dvDyTable1 = new DataView(dsTemplateData.Tables[14]);
    //                            DataView dvDyTable2 = new DataView(dsTemplateData.Tables[15]);
    //                            DataView dvDyTable3 = new DataView(dsTemplateData.Tables[16]);
    //                            DataView dvDyTable4 = new DataView(dsTemplateData.Tables[17]);
    //                            DataView dvDyTable5 = new DataView(dsTemplateData.Tables[18]);
    //                            DataView dvDyTable6 = new DataView(dsTemplateData.Tables[19]);
    //                            DataTable dtDyTempTable = new DataTable();

    //                            dvDyTable1.ToTable().TableName = "TemplateSectionName";
    //                            dvDyTable6.ToTable().TableName = "TabularTemplateFieldStyle";
    //                            if (common.myInt(ddlTemplatePatient) == 0)
    //                            {
    //                                dvDyTable1.RowFilter = "TemplateId=" + common.myStr(dtTemplate.Rows[i]["TemplateId"]);
    //                                dvDyTable4.RowFilter = "TemplateId=" + common.myStr(dtTemplate.Rows[i]["TemplateId"]);
    //                                dvDyTable6.RowFilter = "TemplateId=" + common.myStr(dtTemplate.Rows[i]["TemplateId"]);
    //                                dtDyTempTable = dvDyTable4.ToTable();
    //                                dvDyTable4.Sort = "RecordId ASC";
    //                            }
    //                            else
    //                            {
    //                                dvDyTable1.RowFilter = "TemplateId=" + common.myStr(ddlTemplatePatient);
    //                                dvDyTable4.RowFilter = "TemplateId=" + common.myStr(ddlTemplatePatient);
    //                                dvDyTable6.RowFilter = "TemplateId=" + common.myStr(ddlTemplatePatient);
    //                                dtDyTempTable = dvDyTable4.ToTable();
    //                                dvDyTable4.Sort = "RecordId ASC";
    //                            }
    //                            string sSectionId = "0";
    //                            for (int iS = 0; iS < dvDyTable1.ToTable().Rows.Count; iS++)
    //                            {
    //                                sSectionId = iS == 0 ? "'" + dvDyTable1.ToTable().Rows[iS]["SectionId"].ToString() + "'"
    //                                    : sSectionId + ", '" + dvDyTable1.ToTable().Rows[iS]["SectionId"].ToString() + "'";
    //                            }
    //                            dvDyTable2.ToTable().TableName = "FieldName";
    //                            dvDyTable2.RowFilter = "SectionId IN (" + sSectionId + ")";
    //                            dvDyTable6.RowFilter = "SectionId IN (" + sSectionId + ")";

    //                            dvDyTable3.ToTable().TableName = "PatientValue";
    //                            if (dvDyTable3.ToTable().Rows.Count > 0)
    //                            {
    //                                dvDyTable3.RowFilter = "SectionId IN (" + sSectionId + ") AND EncounterId=" + common.myInt(dtEncounter.Rows[iEn]["EncounterId"]);
    //                            }
    //                            if (dvDyTable4.ToTable().Rows.Count > 0)
    //                            {
    //                                dvDyTable4.RowFilter = "SectionId IN (" + sSectionId + ") AND EncounterId=" + common.myInt(dtEncounter.Rows[iEn]["EncounterId"]);
    //                            }

    //                            dsDymanicTemplateData.Tables.Add(dvDyTable1.ToTable());
    //                            dsDymanicTemplateData.Tables.Add(dvDyTable2.ToTable());
    //                            dsDymanicTemplateData.Tables.Add(dvDyTable3.ToTable());

    //                            if (dvDyTable4.ToTable().Rows.Count > 0)
    //                            {
    //                                dsDymanicTemplateData.Tables.Add(dtDyTempTable);
    //                            }
    //                            else
    //                            {
    //                                dsDymanicTemplateData.Tables.Add(dvDyTable4.ToTable());
    //                            }
    //                            dsDymanicTemplateData.Tables.Add(dvDyTable5.ToTable());
    //                            dsDymanicTemplateData.Tables.Add(dvDyTable6.ToTable());
    //                            if (dsDymanicTemplateData.Tables[2].Rows.Count > 0 || dsDymanicTemplateData.Tables[3].Rows.Count > 0)
    //                            {
    //                                bindData(dsDymanicTemplateData, common.myStr(dtTemplate.Rows[i]["TemplateId"]), sbTemp, "");
    //                                if (sbTemp.Length > 20)
    //                                    // TemplateString.Append(sbTemp);
    //                                    TemplateString.Append(sbTemp + "<br/>");
    //                            }

    //                            sbTemp = null;
    //                            dsDymanicTemplateData.Dispose();
    //                            dvDyTable1.Dispose();
    //                            dvDyTable2.Dispose();
    //                            dvDyTable3.Dispose();
    //                            dvDyTable4.Dispose();
    //                            dvDyTable5.Dispose();
    //                            dvDyTable6.Dispose();
    //                            dtDyTempTable.Dispose();
    //                            sSectionId = "";
    //                            #endregion
    //                        }

    //                        Templinespace = "";
    //                    }
    //                    #endregion
    //                    #region Allergy
    //                    if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim() == "Allergies"
    //                         && common.myStr(dtTemplate.Rows[i]["TemplateType"]).Trim() == "S" && bAllergyDisplay == false)
    //                    {
    //                        sbTemplateStyle = new StringBuilder();
    //                        DataView dv = new DataView(dsTemplateStyle.Tables[0]);
    //                        dv.RowFilter = "PageId=" + common.myInt(dtTemplate.Rows[i]["PageId"]).ToString();
    //                        drTemplateStyle = null;// = dv[0].Row;
    //                        if (dv.Count > 0)
    //                        {
    //                            drTemplateStyle = dv[0].Row;
    //                            string sBegin = "", sEnd = "";
    //                            Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
    //                            MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
    //                        }
    //                        dv.Dispose();
    //                        sbTemp = new StringBuilder();

    //                        if ((common.myInt(ddlTemplatePatient) == 0)
    //                             || (common.myInt(ddlTemplatePatient) == 8))
    //                        {
    //                            #region Call Allergy template data
    //                            BindCaseSheet.BindAllergies(dsTemplateData.Tables[1], sbTemp, sbTemplateStyle, drTemplateStyle, Page,
    //                                       common.myStr(dtTemplate.Rows[i]["PageID"]), 0, "");
    //                            #endregion
    //                            if (sbTemp.ToString() != "")
    //                            {
    //                                TemplateString.Append(sbTemp + "<br/>");
    //                                bAllergyDisplay = true;
    //                            }
    //                            sbTemp = null;
    //                            sbTemplateStyle = null;
    //                        }
    //                        drTemplateStyle = null;
    //                        Templinespace = "";
    //                    }
    //                    #endregion
    //                    #region Vital
    //                    if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim() == "Vitals"
    //                         && common.myStr(dtTemplate.Rows[i]["TemplateType"]).Trim() == "S")
    //                    {
    //                        sbTemplateStyle = new StringBuilder();
    //                        DataView dv = new DataView(dsTemplateStyle.Tables[0]);
    //                        dv.RowFilter = "PageId=" + common.myInt(dtTemplate.Rows[i]["PageId"]).ToString();
    //                        if (dv.Count > 0)
    //                        {
    //                            drTemplateStyle = dv[0].Row;
    //                            string sBegin = "", sEnd = "";
    //                            Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
    //                            MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
    //                        }
    //                        dv.Dispose();
    //                        sbTemp = new StringBuilder();

    //                        if ((common.myInt(ddlTemplatePatient) == 0)
    //                             || (common.myInt(ddlTemplatePatient) == 14))
    //                        {
    //                            #region Call Vital Template data
    //                            BindCaseSheet.BindVitals(dsTemplateData.Tables[11], sbTemp, sbTemplateStyle, drTemplateStyle,
    //                                                Page, common.myStr(dtTemplate.Rows[i]["PageId"]), 0, "", common.myInt(dtEncounter.Rows[iEn]["EncounterId"]));

    //                            #endregion
    //                            if (sbTemp.ToString() != "")
    //                                //////TemplateString.Append(sbTemp + "<br/>" + "<br/>");

    //                                TemplateString.Append(sbTemp + "<br/>");
    //                            sbTemp = null;
    //                            sbTemplateStyle = null;
    //                        }

    //                        drTemplateStyle = null;
    //                        Templinespace = "";

    //                    }
    //                    #endregion
    //                    #region All the Templates except Hitory and Plan of case
    //                    if (common.myStr(dtTemplate.Rows[i]["TemplateId"]).Trim() != ""
    //                        && common.myStr(dtTemplate.Rows[i]["TemplateType"]).Trim() == "D"
    //                        && common.myStr(dtTemplate.Rows[i]["TemplateCode"]).Trim() != "POC"
    //                        && common.myStr(dtTemplate.Rows[i]["TemplateCode"]).Trim() != "HIS")
    //                    {
    //                        DataView dv = new DataView(dsTemplateStyle.Tables[0]);
    //                        dv.RowFilter = "PageId=" + common.myInt(dtTemplate.Rows[i]["TemplateId"]).ToString();
    //                        if (dv.Count > 0)
    //                        {
    //                            drTemplateStyle = dv[0].Row;
    //                            string sBegin = "", sEnd = "";
    //                            Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
    //                            MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
    //                        }
    //                        dv.Dispose();
    //                        sbTemp = new StringBuilder();
    //                        if ((common.myInt(ddlTemplatePatient) == 0)
    //                            || (common.myInt(ddlTemplatePatient) == common.myInt(dtTemplate.Rows[i]["TemplateId"])))
    //                        {
    //                            #region Assign Data and call all Dynamic Template Except Hostory and Plan of Care template

    //                            DataSet dsDymanicTemplateData = new DataSet();

    //                            DataView dvDyTable1 = new DataView(dsTemplateData.Tables[14]);
    //                            DataView dvDyTable2 = new DataView(dsTemplateData.Tables[15]);
    //                            DataView dvDyTable3 = new DataView(dsTemplateData.Tables[16]);
    //                            DataView dvDyTable4 = new DataView(dsTemplateData.Tables[17]);
    //                            DataView dvDyTable5 = new DataView(dsTemplateData.Tables[18]);
    //                            DataView dvDyTable6 = new DataView(dsTemplateData.Tables[19]);
    //                            DataTable dtDyTempTable = new DataTable();

    //                            dvDyTable1.ToTable().TableName = "TemplateSectionName";
    //                            dvDyTable6.ToTable().TableName = "TabularTemplateFieldStyle";
    //                            if (common.myInt(ddlTemplatePatient) == 0)
    //                            {
    //                                dvDyTable1.RowFilter = "TemplateId=" + common.myStr(dtTemplate.Rows[i]["TemplateId"]);
    //                                dvDyTable4.RowFilter = "TemplateId=" + common.myStr(dtTemplate.Rows[i]["TemplateId"]);
    //                                dvDyTable6.RowFilter = "TemplateId=" + common.myStr(dtTemplate.Rows[i]["TemplateId"]);
    //                                dtDyTempTable = dvDyTable4.ToTable();
    //                                dvDyTable4.Sort = "RecordId ASC";
    //                            }
    //                            else
    //                            {
    //                                dvDyTable1.RowFilter = "TemplateId=" + common.myStr(ddlTemplatePatient);
    //                                dvDyTable4.RowFilter = "TemplateId=" + common.myStr(ddlTemplatePatient);
    //                                dvDyTable6.RowFilter = "TemplateId=" + common.myStr(ddlTemplatePatient);
    //                                dtDyTempTable = dvDyTable4.ToTable();
    //                                dvDyTable4.Sort = "RecordId ASC";
    //                            }
    //                            string sSectionId = "0";
    //                            for (int iS = 0; iS < dvDyTable1.ToTable().Rows.Count; iS++)
    //                            {
    //                                sSectionId = iS == 0 ? "'" + dvDyTable1.ToTable().Rows[iS]["SectionId"].ToString() + "'"
    //                                    : sSectionId + ", '" + dvDyTable1.ToTable().Rows[iS]["SectionId"].ToString() + "'";
    //                            }
    //                            dvDyTable2.ToTable().TableName = "FieldName";
    //                            dvDyTable2.RowFilter = "SectionId IN (" + sSectionId + ")";
    //                            dvDyTable6.RowFilter = "SectionId IN (" + sSectionId + ")";

    //                            dvDyTable3.ToTable().TableName = "PatientValue";
    //                            if (dvDyTable3.ToTable().Rows.Count > 0)
    //                            {
    //                                dvDyTable3.RowFilter = "SectionId IN (" + sSectionId + ") AND EncounterId=" + common.myInt(dtEncounter.Rows[iEn]["EncounterId"]);
    //                            }
    //                            if (dvDyTable4.ToTable().Rows.Count > 0)
    //                            {
    //                                dvDyTable4.RowFilter = "SectionId IN (" + sSectionId + ") AND EncounterId=" + common.myInt(dtEncounter.Rows[iEn]["EncounterId"]);
    //                            }

    //                            dsDymanicTemplateData.Tables.Add(dvDyTable1.ToTable());
    //                            dsDymanicTemplateData.Tables.Add(dvDyTable2.ToTable());
    //                            dsDymanicTemplateData.Tables.Add(dvDyTable3.ToTable());


    //                            if (dvDyTable4.ToTable().Rows.Count > 0)
    //                            {
    //                                dsDymanicTemplateData.Tables.Add(dtDyTempTable);
    //                            }
    //                            else
    //                            {
    //                                dsDymanicTemplateData.Tables.Add(dvDyTable4.ToTable());
    //                            }
    //                            dsDymanicTemplateData.Tables.Add(dvDyTable5.ToTable());
    //                            dsDymanicTemplateData.Tables.Add(dvDyTable6.ToTable());
    //                            if (dsDymanicTemplateData.Tables[2].Rows.Count > 0 || dsDymanicTemplateData.Tables[3].Rows.Count > 0)
    //                            {
    //                                bindData(dsDymanicTemplateData, common.myStr(dtTemplate.Rows[i]["TemplateId"]), sbTemp, "");
    //                                if (sbTemp.Length > 20)
    //                                    TemplateString.Append(sbTemp + "<br/>");
    //                            }
    //                            sbTemp = null;
    //                            dvDyTable1.Dispose();
    //                            dvDyTable2.Dispose();
    //                            dvDyTable3.Dispose();
    //                            dvDyTable4.Dispose();
    //                            dvDyTable5.Dispose();
    //                            dvDyTable6.Dispose();
    //                            dtDyTempTable.Dispose();
    //                            dsDymanicTemplateData.Dispose();
    //                            sSectionId = "";
    //                            #endregion
    //                        }

    //                        Templinespace = "";
    //                    }
    //                    #endregion
    //                    #region Lab
    //                    if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim() == "Lab Test Result"
    //                        && common.myStr(dtTemplate.Rows[i]["DataStatus"]).Trim() == "AVAILABLE")
    //                    {
    //                        string strTemplateType = common.myStr(dtTemplate.Rows[i]["PageIdentification"]);
    //                        strTemplateType = strTemplateType.Substring(0, 1);
    //                        //sbTemplateStyle = new StringBuilder();
    //                        DataView dv = new DataView(dsTemplateStyle.Tables[0]);
    //                        dv.RowFilter = "PageId=" + common.myInt(dtTemplate.Rows[i]["PageId"]).ToString();
    //                        if (dv.Count > 0)
    //                        {
    //                            drTemplateStyle = dv[0].Row;
    //                            string sBegin = "", sEnd = "";
    //                            Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
    //                            MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
    //                        }
    //                        dv.Dispose();
    //                        sbTemp = new StringBuilder();

    //                        // bnotes.BindLabTestResult(common.myInt(Session["RegistrationId"]), HospitalId, EncounterId, UserId, DoctorId, sbTemp, sbTemplateStyle, drTemplateStyle, Page, common.myInt(Session["FacilityID"]), common.myStr(dtTemplate.Rows[i]["PageId"]), common.myStr(Session["UserID"]));
    //                        TemplateString.Append(sbTemp);
    //                        drTemplateStyle = null;

    //                        sbTemp = null;
    //                        Templinespace = "";
    //                    }
    //                    #endregion
    //                    #region Diagnosis
    //                    if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim() == "Diagnosis"
    //                       && common.myStr(dtTemplate.Rows[i]["TemplateType"]).Trim() == "S")
    //                    {
    //                        sbTemplateStyle = new StringBuilder();
    //                        DataView dv = new DataView(dsTemplateStyle.Tables[0]);
    //                        dv.RowFilter = "PageId=" + common.myInt(dtTemplate.Rows[i]["PageId"]).ToString();
    //                        if (dv.Count > 0)
    //                        {
    //                            drTemplateStyle = dv[0].Row;
    //                            string sBegin = "", sEnd = "";
    //                            Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
    //                            MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
    //                        }
    //                        dv.Dispose();
    //                        sbTemp = new StringBuilder();

    //                        if ((common.myInt(ddlTemplatePatient) == 0)
    //                             || (common.myInt(ddlTemplatePatient) == 133))
    //                        {
    //                            #region Call Diagnosis Template Data


    //                            BindCaseSheet.BindAssessments(dsTemplateData.Tables[4], sbTemp, sbTemplateStyle, drTemplateStyle, Page,
    //                                       common.myStr(dtTemplate.Rows[i]["PageId"]), common.myStr(Session["UserID"]),
    //                                       0, common.myInt(dtEncounter.Rows[iEn]["EncounterId"]), "", common.myStr(ViewState["IsShowDiagnosisGroupHeading"]));
    //                            #endregion
    //                            if (sbTemp.ToString() != "")
    //                                TemplateString.Append(sbTemp + "<br/>");
    //                            sbTemp = null;
    //                            sbTemplateStyle = null;
    //                        }
    //                        drTemplateStyle = null;
    //                        Templinespace = "";
    //                    }
    //                    #endregion
    //                    #region Provisional Diagnosis
    //                    if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim() == "Provisional Diagnosis"
    //                         && common.myStr(dtTemplate.Rows[i]["TemplateType"]).Trim() == "S")
    //                    {
    //                        sbTemplateStyle = new StringBuilder();
    //                        DataView dv = new DataView(dsTemplateStyle.Tables[0]);
    //                        dv.RowFilter = "PageId=" + common.myInt(dtTemplate.Rows[i]["PageId"]).ToString();
    //                        if (dv.Count > 0)
    //                        {
    //                            drTemplateStyle = dv[0].Row;
    //                            string sBegin = "", sEnd = "";
    //                            Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
    //                            MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
    //                        }
    //                        dv.Dispose();
    //                        sbTemp = new StringBuilder();
    //                        if ((common.myInt(ddlTemplatePatient) == 0)
    //                             || (common.myInt(ddlTemplatePatient) == 1085))
    //                        {
    //                            #region Call Provisional Diagnosis template data
    //                            BindCaseSheet.BindPatientProvisionalDiagnosis(dsTemplateData.Tables[2],
    //                                       Request.QueryString["DoctorId"] != null ? Request.QueryString["DoctorId"].ToString() : DoctorId, sbTemp, sbTemplateStyle, drTemplateStyle, Page,
    //                                       common.myStr(dtTemplate.Rows[i]["PageId"]), common.myStr(Session["UserID"]),
    //                                        0, common.myInt(dtEncounter.Rows[iEn]["EncounterId"]), "");
    //                            #endregion
    //                            if (sbTemp.ToString() != "")
    //                                TemplateString.Append(sbTemp + "<br/>");
    //                            sbTemp = null;
    //                            sbTemplateStyle = null;
    //                        }
    //                        drTemplateStyle = null;
    //                        Templinespace = "";
    //                    }
    //                    #endregion
    //                    #region Template Plan of Care
    //                    if (common.myStr(dtTemplate.Rows[i]["TemplateId"]).Trim() != ""
    //                         && common.myStr(dtTemplate.Rows[i]["TemplateType"]).Trim() == "D"
    //                        && common.myStr(dtTemplate.Rows[i]["TemplateCode"]).Trim() == "POC")
    //                    {
    //                        DataView dv = new DataView(dsTemplateStyle.Tables[0]);
    //                        dv.RowFilter = "PageId=" + common.myInt(dtTemplate.Rows[i]["TemplateId"]).ToString();
    //                        if (dv.Count > 0)
    //                        {
    //                            drTemplateStyle = dv[0].Row;
    //                            string sBegin = "", sEnd = "";
    //                            Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
    //                            MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
    //                        }
    //                        dv.Dispose();
    //                        sbTemp = new StringBuilder();
    //                        if ((common.myInt(ddlTemplatePatient) == 0)
    //                            || (common.myInt(ddlTemplatePatient) == common.myInt(dtTemplate.Rows[i]["TemplateId"])))
    //                        {
    //                            #region Assign Data and call Dynamic Template Plan of Care
    //                            DataSet dsDymanicTemplateData = new DataSet();

    //                            DataView dvDyTable1 = new DataView(dsTemplateData.Tables[14]);
    //                            DataView dvDyTable2 = new DataView(dsTemplateData.Tables[15]);
    //                            DataView dvDyTable3 = new DataView(dsTemplateData.Tables[16]);
    //                            DataView dvDyTable4 = new DataView(dsTemplateData.Tables[17]);
    //                            DataView dvDyTable5 = new DataView(dsTemplateData.Tables[18]);
    //                            DataView dvDyTable6 = new DataView(dsTemplateData.Tables[19]);
    //                            DataTable dtDyTempTable = new DataTable();

    //                            dvDyTable1.ToTable().TableName = "TemplateSectionName";
    //                            dvDyTable6.ToTable().TableName = "TabularTemplateFieldStyle";
    //                            if (common.myInt(ddlTemplatePatient) == 0)
    //                            {
    //                                dvDyTable1.RowFilter = "TemplateId=" + common.myStr(dtTemplate.Rows[i]["TemplateId"]);
    //                                dvDyTable4.RowFilter = "TemplateId=" + common.myStr(dtTemplate.Rows[i]["TemplateId"]);
    //                                dvDyTable6.RowFilter = "TemplateId=" + common.myStr(dtTemplate.Rows[i]["TemplateId"]);
    //                                dtDyTempTable = dvDyTable4.ToTable();
    //                                dvDyTable4.Sort = "RecordId ASC";
    //                            }
    //                            else
    //                            {
    //                                dvDyTable1.RowFilter = "TemplateId=" + common.myStr(ddlTemplatePatient);
    //                                dvDyTable4.RowFilter = "TemplateId=" + common.myStr(ddlTemplatePatient);
    //                                dvDyTable6.RowFilter = "TemplateId=" + common.myStr(ddlTemplatePatient);
    //                                dtDyTempTable = dvDyTable4.ToTable();
    //                                dvDyTable4.Sort = "RecordId ASC";
    //                            }

    //                            string sSectionId = "0";
    //                            for (int iS = 0; iS < dvDyTable1.ToTable().Rows.Count; iS++)
    //                            {
    //                                sSectionId = iS == 0 ? "'" + dvDyTable1.ToTable().Rows[iS]["SectionId"].ToString() + "'"
    //                                    : sSectionId + ", '" + dvDyTable1.ToTable().Rows[iS]["SectionId"].ToString() + "'";
    //                            }
    //                            dvDyTable2.ToTable().TableName = "FieldName";
    //                            dvDyTable2.RowFilter = "SectionId IN (" + sSectionId + ")";
    //                            dvDyTable6.RowFilter = "SectionId IN (" + sSectionId + ")";

    //                            dvDyTable3.ToTable().TableName = "PatientValue";
    //                            if (dvDyTable3.ToTable().Rows.Count > 0)
    //                            {
    //                                dvDyTable3.RowFilter = "SectionId IN (" + sSectionId + ") AND EncounterId=" + common.myInt(dtEncounter.Rows[iEn]["EncounterId"]);
    //                            }
    //                            if (dvDyTable4.ToTable().Rows.Count > 0)
    //                            {
    //                                dvDyTable4.RowFilter = "SectionId IN (" + sSectionId + ") AND EncounterId=" + common.myInt(dtEncounter.Rows[iEn]["EncounterId"]);
    //                            }

    //                            dsDymanicTemplateData.Tables.Add(dvDyTable1.ToTable());
    //                            dsDymanicTemplateData.Tables.Add(dvDyTable2.ToTable());
    //                            dsDymanicTemplateData.Tables.Add(dvDyTable3.ToTable());

    //                            if (dvDyTable4.ToTable().Rows.Count > 0)
    //                            {
    //                                dsDymanicTemplateData.Tables.Add(dtDyTempTable);
    //                            }
    //                            else
    //                            {
    //                                dsDymanicTemplateData.Tables.Add(dvDyTable4.ToTable());
    //                            }
    //                            dsDymanicTemplateData.Tables.Add(dvDyTable5.ToTable());
    //                            dsDymanicTemplateData.Tables.Add(dvDyTable6.ToTable());
    //                            if (dsDymanicTemplateData.Tables[2].Rows.Count > 0 || dsDymanicTemplateData.Tables[3].Rows.Count > 0)
    //                            {
    //                                bindData(dsDymanicTemplateData, common.myStr(dtTemplate.Rows[i]["TemplateId"]), sbTemp, "");
    //                                if (sbTemp.Length > 20)
    //                                    TemplateString.Append(sbTemp + "<br/>");
    //                            }
    //                            sbTemp = null;

    //                            dvDyTable1.Dispose();
    //                            dvDyTable2.Dispose();
    //                            dvDyTable3.Dispose();
    //                            dvDyTable4.Dispose();
    //                            dvDyTable5.Dispose();
    //                            dvDyTable6.Dispose();
    //                            dtDyTempTable.Dispose();
    //                            dsDymanicTemplateData.Dispose();
    //                            sSectionId = "";
    //                            #endregion
    //                        }

    //                        Templinespace = "";
    //                    }
    //                    #endregion
    //                    #region Orders And Procedures
    //                    if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim() == "Orders And Procedures"
    //                         && common.myStr(dtTemplate.Rows[i]["TemplateType"]).Trim() == "S")
    //                    {
    //                        sbTemplateStyle = new StringBuilder();
    //                        DataView dv = new DataView(dsTemplateStyle.Tables[0]);
    //                        dv.RowFilter = "PageId=" + common.myInt(dtTemplate.Rows[i]["PageId"]).ToString();
    //                        if (dv.Count > 0)
    //                        {
    //                            drTemplateStyle = dv[0].Row;
    //                            string sBegin = "", sEnd = "";
    //                            Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
    //                            MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
    //                        }
    //                        dv.Dispose();
    //                        sbTemp = new StringBuilder();

    //                        if ((common.myInt(ddlTemplatePatient) == 0)
    //                             || (common.myInt(ddlTemplatePatient) == 17))
    //                        {
    //                            #region Call Bind Order data
    //                            BindCaseSheet.BindOrders(dsTemplateData.Tables[8], DoctorId, sbTemp, sbTemplateStyle, drTemplateStyle,
    //                                           Page, common.myStr(dtTemplate.Rows[i]["PageId"]), common.myInt(dtEncounter.Rows[iEn]["EncounterId"]), "");
    //                            #endregion


    //                            if (sbTemp.ToString() != "")
    //                                TemplateString.Append(sbTemp + "<br/>");

    //                            sbTemp = null;
    //                            sbTemplateStyle = null;
    //                        }

    //                        drTemplateStyle = null;
    //                        Templinespace = "";
    //                    }
    //                    #endregion
    //                    #region Prescription
    //                    if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim() == "Prescription"
    //                        && common.myStr(dtTemplate.Rows[i]["TemplateType"]).Trim() == "S")
    //                    {
    //                        sbTemplateStyle = new StringBuilder();
    //                        DataView dv = new DataView(dsTemplateStyle.Tables[0]);
    //                        dv.RowFilter = "PageId=" + common.myInt(dtTemplate.Rows[i]["PageId"]).ToString();

    //                        if (dv.Count > 0)
    //                        {
    //                            drTemplateStyle = dv[0].Row;
    //                            string sBegin = "", sEnd = "";
    //                            Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
    //                            MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
    //                        }

    //                        dv.Dispose();
    //                        sbTemp = new StringBuilder();

    //                        if ((common.myInt(ddlTemplatePatient) == 0)
    //                             || (common.myInt(ddlTemplatePatient) == 153))
    //                        {
    //                            DataSet dsMedication = new DataSet();
    //                            DataView dvTable1 = new DataView(dsTemplateData.Tables[10]);

    //                            dvTable1.ToTable().TableName = "Item";

    //                            dvTable1.RowFilter = "EncounterId=" + common.myInt(dtEncounter.Rows[iEn]["EncounterId"]);


    //                            dsMedication.Tables.Add(dvTable1.ToTable());


    //                            dvTable1.Dispose();

    //                            #region Call Medication Template data
    //                            BindCaseSheet.BindMedication(dsMedication, sbTemp, sbTemplateStyle, drTemplateStyle, Page, common.myStr(dtTemplate.Rows[i]["PageId"]),
    //                                           common.myInt(Session["UserID"]).ToString(), "");
    //                            #endregion

    //                            dsMedication.Dispose();
    //                            if (sbTemp.ToString() != "")
    //                                TemplateString.Append(sbTemp + "<br/>");
    //                            sbTemp = null;
    //                            sbTemplateStyle = null;
    //                        }

    //                        drTemplateStyle = null;
    //                        Templinespace = "";
    //                    }
    //                    #endregion
    //                    #region Non Drug Order
    //                    if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim() == "Non Drug Order"
    //                         && common.myStr(dtTemplate.Rows[i]["TemplateType"]).Trim() == "S")
    //                    {
    //                        sbTemplateStyle = new StringBuilder();
    //                        DataView dv = new DataView(dsTemplateStyle.Tables[0]);
    //                        dv.RowFilter = "PageId=" + common.myInt(dtTemplate.Rows[i]["PageId"]).ToString();
    //                        if (dv.Count > 0)
    //                        {
    //                            drTemplateStyle = dv[0].Row;
    //                            string sBegin = "", sEnd = "";
    //                            Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
    //                            MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
    //                        }
    //                        dv.Dispose();
    //                        sbTemp = new StringBuilder();
    //                        if ((common.myInt(ddlTemplatePatient) == 0)
    //                             || (common.myInt(ddlTemplatePatient) == 1166))
    //                        {
    //                            #region Call Non Drug Order template data
    //                            BindCaseSheet.BindNonDrugOrder(dsTemplateData.Tables[7], sbTemp, sbTemplateStyle, drTemplateStyle, Page, "0",
    //                                              common.myInt(dtEncounter.Rows[iEn]["EncounterId"]), "");
    //                            #endregion
    //                            if (sbTemp.ToString() != "")
    //                                TemplateString.Append(sbTemp + "<br/>");
    //                            sbTemp = null;
    //                            sbTemplateStyle = null;
    //                        }
    //                        drTemplateStyle = null;
    //                        Templinespace = "";
    //                    }
    //                    #endregion
    //                    #region Diet Order
    //                    if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim() == "Diet Order"
    //                         && common.myStr(dtTemplate.Rows[i]["TemplateType"]).Trim() == "S")
    //                    {
    //                        sbTemplateStyle = new StringBuilder();
    //                        DataView dv = new DataView(dsTemplateStyle.Tables[0]);
    //                        dv.RowFilter = "PageId=" + common.myInt(dtTemplate.Rows[i]["PageId"]).ToString();

    //                        if (dv.Count > 0)
    //                        {
    //                            drTemplateStyle = dv[0].Row;
    //                            string sBegin = "", sEnd = "";
    //                            Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
    //                            MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
    //                        }

    //                        dv.Dispose();
    //                        sbTemp = new StringBuilder();

    //                        if ((common.myInt(ddlTemplatePatient) == 0)
    //                             || (common.myInt(ddlTemplatePatient) == 1172))
    //                        {
    //                            #region Call Diet Order data
    //                            BindCaseSheet.BindDietOrderInNote(dsTemplateData.Tables[9], sbTemp, sbTemplateStyle, drTemplateStyle, Page, "0", "",
    //                                common.myInt(dtEncounter.Rows[iEn]["EncounterId"]));
    //                            #endregion

    //                            if (sbTemp.ToString() != "")
    //                                TemplateString.Append(sbTemp + "<br/>");
    //                            sbTemp = null;
    //                            sbTemplateStyle = null;
    //                        }

    //                        drTemplateStyle = null;
    //                        Templinespace = "";
    //                    }
    //                    #endregion
    //                    #region Doctor Progress Note
    //                    if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim() == "Doctor Progress Note"
    //                         && common.myStr(dtTemplate.Rows[i]["TemplateType"]).Trim() == "S")
    //                    {
    //                        sbTemplateStyle = new StringBuilder();
    //                        DataView dv = new DataView(dsTemplateStyle.Tables[0]);
    //                        dv.RowFilter = "PageId=" + common.myInt(dtTemplate.Rows[i]["PageId"]).ToString();
    //                        if (dv.Count > 0)
    //                        {
    //                            drTemplateStyle = dv[0].Row;
    //                            string sBegin = "", sEnd = "";
    //                            Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
    //                            MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
    //                        }
    //                        dv.Dispose();
    //                        sbTemp = new StringBuilder();
    //                        if ((common.myInt(ddlTemplatePatient) == 0)
    //                             || (common.myInt(ddlTemplatePatient) == 1013))
    //                        {
    //                            #region Call Doctor Progress Note template data
    //                            BindCaseSheet.BindDoctorProgressNote(dsTemplateData.Tables[3], sbTemp, sbTemplateStyle, drTemplateStyle, Page,
    //                                       common.myStr(dtTemplate.Rows[i]["PageId"]), common.myStr(Session["UserID"]), "",
    //                                       common.myInt(dtEncounter.Rows[iEn]["EncounterId"]));
    //                            #endregion
    //                            if (sbTemp.ToString() != "")
    //                                TemplateString.Append(sbTemp + "<br/>");
    //                            sbTemp = null;
    //                            sbTemplateStyle = null;
    //                        }
    //                        Templinespace = "";
    //                    }
    //                    #endregion
    //                    #region Referal History
    //                    if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim() == "Referral History"
    //                         && common.myStr(dtTemplate.Rows[i]["TemplateType"]).Trim() == "S")
    //                    {
    //                        sbTemplateStyle = new StringBuilder();
    //                        DataView dv = new DataView(dsTemplateStyle.Tables[0]);
    //                        dv.RowFilter = "PageId=" + common.myInt(dtTemplate.Rows[i]["PageId"]).ToString();
    //                        if (dv.Count > 0)
    //                        {
    //                            drTemplateStyle = dv[0].Row;
    //                            string sBegin = "", sEnd = "";
    //                            Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
    //                            MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
    //                        }
    //                        dv.Dispose();
    //                        sbTemp = new StringBuilder();

    //                        if ((common.myInt(ddlTemplatePatient) == 0)
    //                             || (common.myInt(ddlTemplatePatient) == 1081))
    //                        {
    //                            StringBuilder temp1 = new StringBuilder();
    //                            #region Call Referral History Template Data
    //                            BindCaseSheet.BindReferalHistory(dsTemplateData.Tables[5], sbTemp, sbTemplateStyle, drTemplateStyle, Page, "0",
    //                                common.myInt(dtEncounter.Rows[iEn]["EncounterId"]), "");
    //                            #endregion
    //                            if (sbTemp.ToString() != "")
    //                                TemplateString.Append(sbTemp + "<br/>");
    //                            sbTemp = null;
    //                            sbTemplateStyle = null;
    //                        }
    //                        Templinespace = "";
    //                    }
    //                    #endregion
    //                    #region Current Medication
    //                    if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim() == "Current Medication"
    //                        && common.myStr(dtTemplate.Rows[i]["DataStatus"]).Trim() == "AVAILABLE")
    //                    {
    //                        sbTemplateStyle = new StringBuilder();
    //                        DataView dv = new DataView(dsTemplateStyle.Tables[0]);
    //                        dv.RowFilter = "PageId=" + common.myInt(dtTemplate.Rows[i]["PageId"]).ToString();
    //                        if (dv.Count > 0)
    //                        {
    //                            drTemplateStyle = dv[0].Row;
    //                            string sBegin = "", sEnd = "";
    //                            Templinespace = common.myStr(common.myInt(drTemplateStyle["TemplateSpaceNumber"]) - 1);
    //                            MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
    //                        }
    //                        dv.Dispose();
    //                        sbTemp = new StringBuilder();

    //                        if ((common.myInt(ddlTemplatePatient) == 0)
    //                             || (common.myInt(ddlTemplatePatient) == 10005))
    //                        {
    //                            bnotes.BindMedication(common.myInt(Session["EncounterId"]), common.myInt(Session["HospitalLocationID"]), common.myInt(Session["RegistrationId"]), sbTemp, sbTemplateStyle, "C", drTemplateStyle,
    //                                            Page, common.myStr(dtTemplate.Rows[i]["PageId"]), common.myInt(Session["UserID"]).ToString(),
    //                                            common.myDate(DateTime.Now).AddDays(-365).ToString("yyyy/MM/dd"),
    //                                 //common.myDate(DateTime.Now).ToString("yyyy/MM/dd"), common.myStr(ViewState["OPIP"]), "");
    //                                 common.myDate(DateTime.Now).ToString("yyyy/MM/dd"), common.myStr(Session["OPIP"]), "");
    //                            if (sbTemp.ToString() != "")
    //                                TemplateString.Append(sbTemp + "<br/>");
    //                            sbTemp = null;
    //                            sbTemplateStyle = null;
    //                        }

    //                        drTemplateStyle = null;
    //                        Templinespace = "";
    //                    }
    //                    #endregion
    //                    #region Immunization
    //                    if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim() == "Immunization"
    //                      && common.myStr(dtTemplate.Rows[i]["TemplateType"]).Trim() == "S")
    //                    {
    //                        sbTemplateStyle = new StringBuilder();
    //                        DataView dv = new DataView(dsTemplateStyle.Tables[0]);
    //                        dv.RowFilter = "PageId=" + common.myInt(dtTemplate.Rows[i]["PageId"]).ToString();
    //                        if (dv.Count > 0)
    //                        {
    //                            drTemplateStyle = dv[0].Row;
    //                            string sBegin = "", sEnd = "";
    //                            Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
    //                            MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
    //                        }
    //                        dv.Dispose();
    //                        sbTemp = new StringBuilder();

    //                        if ((common.myInt(ddlTemplatePatient) == 0)
    //                             || (common.myInt(ddlTemplatePatient) == 113))
    //                        {
    //                            BindCaseSheet.BindImmunization(dsTemplateData.Tables[13], sbTemp, sbTemplateStyle, drTemplateStyle, Page,
    //                                        common.myStr(dtTemplate.Rows[i]["PageId"]), "", common.myInt(dtEncounter.Rows[iEn]["EncounterId"]));
    //                            if (sbTemp.ToString() != "")
    //                                TemplateString.Append(sbTemp + "<br/>");
    //                            sbTemp = null;
    //                            sbTemplateStyle = null;
    //                        }

    //                        Templinespace = "";
    //                    }
    //                    #endregion
    //                    #region Daily Injection
    //                    if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim() == "Daily Injections"
    //                        && common.myStr(dtTemplate.Rows[i]["TemplateType"]).Trim() == "S")
    //                    {
    //                        sbTemplateStyle = new StringBuilder();
    //                        DataView dv = new DataView(dsTemplateStyle.Tables[0]);
    //                        dv.RowFilter = "PageId=" + common.myInt(dtTemplate.Rows[i]["PageId"]).ToString();
    //                        if (dv.Count > 0)
    //                        {
    //                            drTemplateStyle = dv[0].Row;
    //                            string sBegin = "", sEnd = "";
    //                            Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
    //                            MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
    //                        }

    //                        dv.Dispose();
    //                        sbTemp = new StringBuilder();

    //                        if ((common.myInt(ddlTemplatePatient) == 0)
    //                             || (common.myInt(ddlTemplatePatient) == 805))
    //                        {
    //                            BindCaseSheet.BindInjection(dsTemplateData.Tables[12], sbTemp, sbTemplateStyle, drTemplateStyle, Page,
    //                                         common.myStr(dtTemplate.Rows[i]["PageId"]), "", common.myInt(dtEncounter.Rows[iEn]["EncounterId"]));

    //                            TemplateString.Append(sbTemp + "<br/>");
    //                            sbTemp = null;
    //                            sbTemplateStyle = null;
    //                        }
    //                        Templinespace = "";
    //                    }
    //                    #endregion
    //                    #region Follow-up Appointment
    //                    if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim() == "Follow Up Appointment"
    //                        && common.myStr(dtTemplate.Rows[i]["TemplateType"]).Trim() == "S")
    //                    {
    //                        sbTemplateStyle = new StringBuilder();
    //                        DataView dv = new DataView(dsTemplateStyle.Tables[0]);
    //                        dv.RowFilter = "PageId=" + common.myInt(dtTemplate.Rows[i]["PageId"]).ToString();
    //                        if (dv.Count > 0)
    //                        {
    //                            drTemplateStyle = dv[0].Row;
    //                            string sBegin = "", sEnd = "";
    //                            Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
    //                            MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
    //                        }
    //                        dv.Dispose();
    //                        if ((common.myInt(ddlTemplatePatient) == 0)
    //                             || (common.myInt(ddlTemplatePatient) == 919))
    //                        {
    //                            StringBuilder temp = new StringBuilder();
    //                            #region FollowUp Appointment
    //                            BindCaseSheet.GetEncounterFollowUpAppointment(dsTemplateData.Tables[6],
    //                                   temp, sbTemplateStyle, drTemplateStyle, Page, "", common.myInt(dtEncounter.Rows[iEn]["EncounterId"]));
    //                            #endregion

    //                            TemplateString.Append(temp);
    //                            temp = null;
    //                            sbTemplateStyle = null;
    //                        }
    //                        Templinespace = "";
    //                    }
    //                    #endregion
    //                }
    //                if (TemplateString.Length > 30)
    //                {
    //                    //if (iEn == 0)
    //                    //{
    //                    //sb.Append("<span style='font-size:20px; font-family:Tohama;'>");
    //                    //sb.Append("<b><u>Initial Assessment</u></b><br/><br/>");
    //                    //sb.Append("</span>");
    //                    //}
    //                    sb.Append("<span style='" + String.Empty + "'>");
    //                    sb.Append(TemplateString);
    //                    sb.Append("</span><br/>");
    //                    TemplateString = null;
    //                }
    //                // }
    //                #endregion
    //            }
    //        }
    //        Session["NoAllergyDisplay"] = null;
    //        //if (sign == true)
    //        //{
    //        //    sb.Append(hdnDoctorImage.Value);
    //        //}
    //        //else if (sign == false)
    //        //{
    //        //    if (RTF1.Content != null)
    //        //    {
    //        //        if (RTF1.Content.Contains("dvDoctorImage") == true)
    //        //        {
    //        //            string signData = RTF1.Content.Replace('"', '$');
    //        //            string st = "<div id=$dvDoctorImage$>";
    //        //            int start = signData.IndexOf(@st);
    //        //            if (start > 0)
    //        //            {
    //        //                int End = signData.IndexOf("</div>", start);
    //        //                StringBuilder sbte = new StringBuilder();
    //        //                sbte.Append(signData.Substring(start, (End + 6) - start));
    //        //                StringBuilder ne = new StringBuilder();
    //        //                ne.Append(signData.Replace(sbte.ToString(), ""));
    //        //                sb.Append(ne.Replace('$', '"').ToString());
    //        //                sbte = null;
    //        //                ne = null;
    //        //                signData = "";
    //        //                st = "";
    //        //                start = 0;
    //        //                End = 0;
    //        //            }
    //        //        }
    //        //    }
    //        //}
    //    }
    //    catch (Exception ex)
    //    {
    //        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
    //        lblMessage.Text = "Error: " + ex.Message;
    //        objException.HandleException(ex);
    //    }
    //    finally
    //    {
    //        sbTemplateStyle = null;
    //        TemplateString = null;
    //        ds.Dispose();
    //        dsTemplateStyle.Dispose();
    //        dvDataFilter.Dispose();
    //        drTemplateStyle = null;
    //        dtTemplate.Dispose();
    //        Templinespace = "";
    //        bnotes = null;
    //        fun = null;
    //        emr = null;
    //        dtGroupDateWiseTemplate.Dispose();
    //        sbTemp = null;
    //        BindCaseSheet = null;
    //        dsTemplateData.Dispose();
    //        sTemplateName = String.Empty;
    //    }
    //    return sb.ToString();
    //}
    #region final  BindEditor
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
        //  DataView dvAllergies = new DataView();

        string Templinespace = "";
        BindNotes bnotes = new BindNotes(sConString);
        BaseC.DiagnosisDA fun = new BaseC.DiagnosisDA(sConString);
        BaseC.clsEMR emr = new BaseC.clsEMR(sConString);
        DataTable dtGroupDateWiseTemplate = new DataTable();
        StringBuilder sbTemp = new StringBuilder();
        bool bAllergyDisplay = false;
        bool bPatientBookingDisplay = false;

        int ddlTemplatePatient = 0;
        //string sTemplateName = common.myStr(ddlTemplatePatient) == "ALL" ? "" : common.myStr(ddlTemplatePatient);
        string sTemplateName = "";
        #region Declare DataSet
        DataSet dsTemplateData = new DataSet();
        #endregion
        #region Call Bind Case Sheet class
        BindCaseSheet BindCaseSheet = new BindCaseSheet(sConString);
        #endregion
        try
        {
            //yogesh 29/07/2022
            BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
            DataSet Reportds = objEMR.getReportFormatDetails(common.myInt(Session["DoctorId"]));
            ViewState["reportid"] = common.myStr(Reportds.Tables[0].Rows[0]["reportid"]);
            ViewState["headerid"] = common.myStr(Reportds.Tables[0].Rows[0]["headerid"]);


            string DoctorId = fun.GetDoctorId(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["UserID"]));
            dsTemplateStyle = bnotes.GetTemplateStyle(common.myInt(Session["HospitalLocationId"]));
            StringBuilder sbStaticTemplateIds = new StringBuilder();
            StringBuilder sbDynamicTemplateIds = new StringBuilder();
            string[] sDateRange = getFromAndToDate(common.myStr(ddlDateRange.SelectedValue));
            #region Call Bind Case Sheet method to get data
            if (IsViewSelectedTemplate)
            {
                //ViewState["templateid_new"] = string.Empty;
                //ViewState["TemplateName"] = string.Empty;
                //ViewState["TemplateType"] = string.Empty;

                //if (sbDynamicTemplateIds.ToString() == string.Empty && sbStaticTemplateIds.ToString() == string.Empty)
                //{
                //    return string.Empty;
                //}

                dsTemplateData = emr.getEMRPrintCaseSheetDate(common.myInt(Session["HospitalLocationId"]),
                                                    common.myInt(Session["FacilityId"]), common.myInt(Session["RegistrationId"]),
                                                    common.myInt(ViewState["EncounterId"])
                                                    , sDateRange[0], sDateRange[1]
                                         //           , common.myDate(DateTime.Now).AddDays(-365).ToString("yyyy/MM/dd"),
                                         //common.myDate(DateTime.Now).ToString("yyyy/MM/dd")
                                         , string.Empty,
                                                    0, string.Empty, false, common.myInt(ViewState["reportid"]));  //yogesh 29/07/2022

            }
            else
            {
                dsTemplateData = emr.getEMRPrintCaseSheetDate(common.myInt(Session["HospitalLocationId"]),
                                common.myInt(Session["FacilityId"]), common.myInt(Session["RegistrationId"]), common.myInt(Session["EncounterId"])
                                 , sDateRange[0], sDateRange[1]
                                         //, common.myDate(DateTime.Now).AddDays(-365).ToString("yyyy/MM/dd"),
                                         //         common.myDate(DateTime.Now).ToString("yyyy/MM/dd")
                                         , sTemplateName, 0, "", false, 0);
            }
            #endregion

            dtGroupDateWiseTemplate = emr.GetDateWiseGroupingTemplate(common.myInt(Session["HospitalLocationId"]),
                common.myInt(Session["FacilityId"]), common.myInt(Session["RegistrationId"]), common.myInt(Session["EncounterId"])
                // , common.myDate(DateTime.Now).AddDays(-365).ToString("yyyy/MM/dd"),common.myDate(DateTime.Now).ToString("yyyy/MM/dd")
                , sDateRange[0], sDateRange[1]);
            dvDataFilter = new DataView(dsTemplateData.Tables[21]);

            if (IsViewSelectedTemplate)
            {
                if (!common.myInt(ViewState["templateid_new"]).Equals(0))
                {
                    //  string tvCategoryValueDate = common.myStr(ViewState["tvCategoryValue"]).Trim().Replace(" ", "").Substring(2, 10);
                    string tvCategoryValueDate = common.myStr(lblVisitDate.Text);
                    string TemplateDate = common.myDate(tvCategoryValueDate).ToString("yyyy/MM/dd");
                    dvDataFilter.RowFilter = "TemplateId=" + common.myInt(ViewState["templateid_new"]) + "  or PageId = " + common.myInt(ViewState["templateid_new"]) + "  ";
                }
                else
                {
                    dvDataFilter.RowFilter = "1=1";
                    //yogesh 06/07/2022
                    //  dvDataFilter.Sort = "TemplateId ASC";

                }
            }
            dtEncounter = dsTemplateData.Tables[22];
            for (int iEn = 0; iEn < dtEncounter.Rows.Count; iEn++)
            {
                if (dvDataFilter.ToTable().Rows.Count > 0)
                {
                    ViewState["iPrevId"] = null;
                    ViewState["iTemplateId"] = null;
                    #region Chronological Order Wise

                    #endregion
                    #region Template Wise
                    //else
                    //{
                    dtTemplate = dvDataFilter.ToTable();
                    TemplateString = new StringBuilder();
                    for (int i = 0; i < dtTemplate.Rows.Count; i++)
                    {
                        #region Admission Request
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
                            if ((common.myInt(ddlTemplatePatient) == 0)
                                 || (common.myInt(ddlTemplatePatient) == 98))
                            {
                                #region Call Bind Problem data
                                BindCaseSheet.BindProblemsHPI(dsTemplateData.Tables[0], common.myInt(ViewState["RegistrationId"]), common.myInt(dtEncounter.Rows[iEn]["EncounterId"]), sbTemp, sbTemplateStyle, drTemplateStyle,
                                               Page, common.myStr(dtTemplate.Rows[i]["PageId"]), "", true);
                                #endregion
                                if (sbTemp.ToString() != "")
                                    //////TemplateString.Append(sbTemp + "<br/>" + "<br/>");
                                    TemplateString.Append(sbTemp + "<br/>");
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
                            if ((common.myInt(ddlTemplatePatient) == 0)
                                || (common.myInt(ddlTemplatePatient) == common.myInt(dtTemplate.Rows[i]["TemplateId"])))
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
                                if (common.myInt(ddlTemplatePatient) == 0)
                                {
                                    dvDyTable1.RowFilter = "TemplateId=" + common.myStr(dtTemplate.Rows[i]["TemplateId"]);
                                    dvDyTable4.RowFilter = "TemplateId=" + common.myStr(dtTemplate.Rows[i]["TemplateId"]);
                                    dvDyTable6.RowFilter = "TemplateId=" + common.myStr(dtTemplate.Rows[i]["TemplateId"]);
                                    dtDyTempTable = dvDyTable4.ToTable();
                                    dvDyTable4.Sort = "RecordId ASC";
                                }
                                else
                                {
                                    dvDyTable1.RowFilter = "TemplateId=" + common.myStr(ddlTemplatePatient);
                                    dvDyTable4.RowFilter = "TemplateId=" + common.myStr(ddlTemplatePatient);
                                    dvDyTable6.RowFilter = "TemplateId=" + common.myStr(ddlTemplatePatient);
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

                            if ((common.myInt(ddlTemplatePatient) == 0)
                                 || (common.myInt(ddlTemplatePatient) == 8))
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

                            if ((common.myInt(ddlTemplatePatient) == 0)
                                 || (common.myInt(ddlTemplatePatient) == 14))
                            {
                                #region Call Vital Template data
                                BindCaseSheet.BindVitals(dsTemplateData.Tables[11], sbTemp, sbTemplateStyle, drTemplateStyle,
                                                    Page, common.myStr(dtTemplate.Rows[i]["PageId"]), 0, "", common.myInt(dtEncounter.Rows[iEn]["EncounterId"]), true);

                                #endregion
                                if (sbTemp.ToString() != "")
                                    //////TemplateString.Append(sbTemp + "<br/>" + "<br/>");

                                    TemplateString.Append(sbTemp + "<br/>");
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
                            if ((common.myInt(ddlTemplatePatient) == 0)
                                || (common.myInt(ddlTemplatePatient) == common.myInt(dtTemplate.Rows[i]["TemplateId"])))
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
                                if (common.myInt(ddlTemplatePatient) == 0)
                                {
                                    dvDyTable1.RowFilter = "TemplateId=" + common.myStr(dtTemplate.Rows[i]["TemplateId"]);
                                    dvDyTable4.RowFilter = "TemplateId=" + common.myStr(dtTemplate.Rows[i]["TemplateId"]);
                                    dvDyTable6.RowFilter = "TemplateId=" + common.myStr(dtTemplate.Rows[i]["TemplateId"]);
                                    dtDyTempTable = dvDyTable4.ToTable();
                                    dvDyTable4.Sort = "RecordId ASC";
                                }
                                else
                                {
                                    dvDyTable1.RowFilter = "TemplateId=" + common.myStr(ddlTemplatePatient);
                                    dvDyTable4.RowFilter = "TemplateId=" + common.myStr(ddlTemplatePatient);
                                    dvDyTable6.RowFilter = "TemplateId=" + common.myStr(ddlTemplatePatient);
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

                            if ((common.myInt(ddlTemplatePatient) == 0)
                                 || (common.myInt(ddlTemplatePatient) == 133))
                            {
                                #region Call Diagnosis Template Data

                                if (common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]),
                                                        common.myInt(Session["FacilityId"]), "IsShowEMRDiagnosisInTabularFormat", sConString).Equals("Y"))
                                {
                                    BindCaseSheet.BindTabularAssessments(dsTemplateData.Tables[4], sbTemp, sbTemplateStyle, drTemplateStyle, Page,
                                   common.myStr(dtTemplate.Rows[i]["PageId"]), common.myStr(Session["UserID"]),
                                   0, common.myInt(dtEncounter.Rows[iEn]["EncounterId"]), "", common.myStr(ViewState["IsShowDiagnosisGroupHeading"]), true);
                                }
                                else
                                {
                                    BindCaseSheet.BindAssessments(dsTemplateData.Tables[4], sbTemp, sbTemplateStyle, drTemplateStyle, Page,
                                               common.myStr(dtTemplate.Rows[i]["PageId"]), common.myStr(Session["UserID"]),
                                               0, common.myInt(dtEncounter.Rows[iEn]["EncounterId"]), "", common.myStr(ViewState["IsShowDiagnosisGroupHeading"]), true);
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
                            if ((common.myInt(ddlTemplatePatient) == 0)
                                 || (common.myInt(ddlTemplatePatient) == 1085))
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
                            if ((common.myInt(ddlTemplatePatient) == 0)
                                || (common.myInt(ddlTemplatePatient) == common.myInt(dtTemplate.Rows[i]["TemplateId"])))
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
                                if (common.myInt(ddlTemplatePatient) == 0)
                                {
                                    dvDyTable1.RowFilter = "TemplateId=" + common.myStr(dtTemplate.Rows[i]["TemplateId"]);
                                    dvDyTable4.RowFilter = "TemplateId=" + common.myStr(dtTemplate.Rows[i]["TemplateId"]);
                                    dvDyTable6.RowFilter = "TemplateId=" + common.myStr(dtTemplate.Rows[i]["TemplateId"]);
                                    dtDyTempTable = dvDyTable4.ToTable();
                                    dvDyTable4.Sort = "RecordId ASC";
                                }
                                else
                                {
                                    dvDyTable1.RowFilter = "TemplateId=" + common.myStr(ddlTemplatePatient);
                                    dvDyTable4.RowFilter = "TemplateId=" + common.myStr(ddlTemplatePatient);
                                    dvDyTable6.RowFilter = "TemplateId=" + common.myStr(ddlTemplatePatient);
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

                            if ((common.myInt(ddlTemplatePatient) == 0)
                                 || (common.myInt(ddlTemplatePatient) == 17))
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

                            if ((common.myInt(ddlTemplatePatient) == 0)
                                 || (common.myInt(ddlTemplatePatient) == 153))
                            {
                                DataSet dsMedication = new DataSet();
                                DataView dvTable1 = new DataView(dsTemplateData.Tables[10]);

                                dvTable1.ToTable().TableName = "Item";

                                dvTable1.RowFilter = "EncounterId=" + common.myInt(dtEncounter.Rows[iEn]["EncounterId"]);


                                dsMedication.Tables.Add(dvTable1.ToTable());


                                dvTable1.Dispose();

                                #region Call Medication Template data
                                BindCaseSheet.BindMedication(dsMedication, sbTemp, sbTemplateStyle, drTemplateStyle, Page, common.myStr(dtTemplate.Rows[i]["PageId"]),
                                               common.myInt(Session["UserID"]).ToString(), string.Empty, 0, string.Empty, string.Empty, true);
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
                            if ((common.myInt(ddlTemplatePatient) == 0)
                                 || (common.myInt(ddlTemplatePatient) == 1166))
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

                            if ((common.myInt(ddlTemplatePatient) == 0)
                                 || (common.myInt(ddlTemplatePatient) == 1172))
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
                            if ((common.myInt(ddlTemplatePatient) == 0)
                                 || (common.myInt(ddlTemplatePatient) == 1013))
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

                            if ((common.myInt(ddlTemplatePatient) == 0)
                                 || (common.myInt(ddlTemplatePatient) == 1081))
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

                            if ((common.myInt(ddlTemplatePatient) == 0)
                                 || (common.myInt(ddlTemplatePatient) == 10005))
                            {
                                bnotes.BindMedication(common.myInt(Session["EncounterId"]), common.myInt(Session["HospitalLocationID"]), common.myInt(Session["RegistrationId"]), sbTemp, sbTemplateStyle, "C", drTemplateStyle,
                                                Page, common.myStr(dtTemplate.Rows[i]["PageId"]), common.myInt(Session["UserID"]).ToString()
                                                , sDateRange[0], sDateRange[1]
                                     //  common.myDate(DateTime.Now).AddDays(-365).ToString("yyyy/MM/dd"),
                                     //common.myDate(DateTime.Now).ToString("yyyy/MM/dd"), common.myStr(ViewState["OPIP"]), "");
                                     //  common.myDate(DateTime.Now).ToString("yyyy/MM/dd")
                                     , common.myStr(Session["OPIP"]), "");
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

                            if ((common.myInt(ddlTemplatePatient) == 0)
                                 || (common.myInt(ddlTemplatePatient) == 113))
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

                            if ((common.myInt(ddlTemplatePatient) == 0)
                                 || (common.myInt(ddlTemplatePatient) == 805))
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
                            if ((common.myInt(ddlTemplatePatient) == 0)
                                 || (common.myInt(ddlTemplatePatient) == 919))
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
                        sb.Append("</span><br/>");
                        TemplateString = null;
                    }
                    // }
                    #endregion
                }
            }
            Session["NoAllergyDisplay"] = null;
            if (sign == true)
            {
                //sb.Append(hdnDoctorImage.Value);
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
    #endregion

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

    //private string BindEditor()
    //{
    //    BaseC.ICM ObjIcm = new BaseC.ICM(sConString);
    //    DataSet dsDis = new DataSet();
    //    string sEREncounterId = "0";
    //    if (Request.QueryString["EREncounterId"] != null)
    //    {
    //        sEREncounterId = Request.QueryString["EREncounterId"].ToString();
    //    }
    //    StringBuilder sb = new StringBuilder();
    //    StringBuilder sbTemplateStyle = new StringBuilder();
    //    DataSet ds = new DataSet();
    //    DataSet dsTemplate = new DataSet();
    //    DataSet dsTemplateStyle = new DataSet();
    //    DataRow drTemplateStyle = null;
    //    DataTable dtTemplate = new DataTable();
    //    Hashtable hst = new Hashtable();
    //    string Templinespace = "";
    //    string Fonts = string.Empty;
    //    BindNotes bnotes = new BindNotes(sConString);
    //    BaseC.DiagnosisDA fun = new BaseC.DiagnosisDA(sConString);
    //    string[] sDate = getFromAndToDate(ddlDateRange.SelectedValue);
    //    try
    //    {
    //        if (common.myInt(ViewState["templateid_new"]) == 1098
    //            || (common.myInt(ViewState["templateid_new"]) == 0 && (common.myStr(lblVisitType.Text) == "IP" || common.myStr(lblVisitType.Text) == "")))
    //        {
    //            dsDis = ObjIcm.GetICMPatientSummaryDetails(common.myInt(Session["HospitalLocationID"]), common.myStr(Session["RegistrationId"]),
    //                                            common.myStr(ViewState["EncounterId"]), 0, common.myInt(Session["FacilityId"]));
    //            if (dsDis.Tables.Count > 0)
    //            {
    //                if (dsDis.Tables[0].Rows.Count > 0)
    //                {
    //                    return common.myStr(dsDis.Tables[0].Rows[0]["PatientSummary"]);
    //                }
    //            }
    //            return "";
    //        }
    //        else if (common.myInt(ViewState["templateid_new"]) == 153)
    //        {
    //            return bindPharmacyData(common.myInt(ViewState["EncounterId"]));
    //        }
    //        RegId = common.myInt(Session["RegistrationID"]);
    //        HospitalId = common.myInt(Session["HospitalLocationID"]);
    //        EncounterId = common.myInt(ViewState["EncounterId"]) == 0 ? common.myInt(EncounterId) : common.myInt(ViewState["EncounterId"]);
    //        UserId = common.myInt(Session["UserID"]);
    //        string DoctorId = fun.GetDoctorId(HospitalId, Convert.ToInt16(UserId));
    //        dsTemplateStyle = bnotes.GetTemplateStyle(common.myInt(Session["HospitalLocationId"]));
    //        if (common.myStr(ViewState["RequestTemplateId"]) == "0")  //Condtion for OP Case Naushad  all WorkSheet
    //        {
    //            dsTemplate = bnotes.GetEMRTemplates(EncounterId, RegId, sEREncounterId);
    //        }
    //        else
    //        {
    //            dsTemplate = bnotes.GetEMRTemplates(common.myInt(ViewState["RequestTemplateId"]) == 0 ? EncounterId : 0, RegId, sEREncounterId, common.myInt(ViewState["RequestTemplateId"]));
    //            ViewState["RequestTemplateId"] = null;
    //        }
    //        dtTemplate = dsTemplate.Tables[0];
    //        if (common.myInt(ViewState["templateid_new"]) != 1098
    //            && common.myInt(ViewState["templateid_new"]) != 153
    //            && common.myInt(ViewState["templateid_new"]) != 0)
    //        {
    //            DataView dvTemp = dsTemplate.Tables[0].DefaultView;
    //            if (ViewState["GroupTable"] != null)
    //            {
    //                DataTable dtGroupTable = (DataTable)ViewState["GroupTable"];
    //                DataView dvGroup = new DataView(dtGroupTable);
    //                dvGroup.RowFilter = "GroupId=" + common.myInt(ViewState["GroupId"]);
    //                if (dvGroup.ToTable().Rows.Count > 0)
    //                {
    //                    for (int l = 0; l < dvGroup.ToTable().Rows.Count; l++)
    //                    {
    //                        dvTemp.RowFilter = "TemplateId=" + common.myInt(dvGroup.ToTable().Rows[l]["TemplateId"]);
    //                        if (dvTemp.ToTable().Rows.Count > 0)
    //                        {
    //                            break;
    //                        }
    //                    }
    //                    dtGroupTable.Dispose();
    //                    dvGroup.ToTable();
    //                }
    //                else
    //                {
    //                    dvTemp.RowFilter = "TemplateId=" + common.myInt(ViewState["templateid_new"]);
    //                }
    //            }
    //            else
    //            {
    //                dvTemp.RowFilter = "TemplateId=" + common.myInt(ViewState["templateid_new"]);
    //            }

    //            dtTemplate = dvTemp.ToTable();
    //        }
    //        sb.Append("<span style='" + Fonts + "'>");
    //        int i = 0;
    //        if (true)
    //        {
    //            if (dtTemplate.Rows.Count > i)
    //            {
    //                if (common.myStr(ViewState["RequestTemplateId"]) == "0") //For OP Case 
    //                {
    //                    int ddlTemplatePatient = 0;
    //                    for (i = 0; i < dtTemplate.Rows.Count; i++)
    //                    {
    //                        #region Complaints
    //                        if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim() == "Complaints" && common.myStr(dtTemplate.Rows[i]["TemplateIdentification"]).Trim() == "P"
    //                            && common.myStr(dtTemplate.Rows[i]["DataStatus"]).Trim() == "AVAILABLE")
    //                        {
    //                            string strTemplateType = common.myStr(dtTemplate.Rows[i]["PageIdentification"]);
    //                            strTemplateType = strTemplateType.Substring(0, 1);
    //                            sbTemplateStyle = new StringBuilder();
    //                            DataView dv = new DataView(dsTemplateStyle.Tables[0]);
    //                            dv.RowFilter = "PageId=" + common.myInt(dtTemplate.Rows[i]["PageId"]);
    //                            if (dv.Count > 0)
    //                            {
    //                                drTemplateStyle = dv[0].Row;
    //                                string sBegin = "", sEnd = "";
    //                                Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
    //                                MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
    //                            }

    //                            StringBuilder sbTemp = new StringBuilder();

    //                            if ((common.myInt(ddlTemplatePatient) == 0)
    //                                 || (common.myInt(ddlTemplatePatient) == 10003))
    //                            {
    //                                bnotes.BindProblemsHPI(RegId, HospitalId, EncounterId, sbTemp, sbTemplateStyle, drTemplateStyle,
    //                                               Page, common.myStr(dtTemplate.Rows[i]["PageId"]), common.myStr(Session["UserID"]),
    //                                               common.myDate(sDate[0]).ToString(),
    //                                               common.myDate(sDate[1]).ToString(), "");
    //                                if (sbTemp.ToString() != "")
    //                                    //sb.Append(sbTemp + "<br/>");
    //                                    sb.Append(sbTemp + "<br/>" + "<br/>");
    //                            }

    //                            drTemplateStyle = null;
    //                            Templinespace = "";
    //                        }
    //                        #endregion
    //                        #region Templates
    //                        if (common.myStr(dtTemplate.Rows[i]["TemplateId"]).Trim() != "" && common.myStr(dtTemplate.Rows[i]["TemplateIdentification"]).Trim() == "T"
    //                           && common.myStr(dtTemplate.Rows[i]["DataStatus"]).Trim() == "AVAILABLE" && common.myStr(dtTemplate.Rows[i]["TemplateCode"]).Trim() == "HIS")
    //                        {
    //                            string strTemplateType = common.myStr(dtTemplate.Rows[i]["PageIdentification"]);
    //                            strTemplateType = strTemplateType.Substring(0, 1);
    //                            sbTemplateStyle = new StringBuilder();
    //                            DataView dv = new DataView(dsTemplateStyle.Tables[0]);
    //                            dv.RowFilter = "PageId=" + common.myInt(dtTemplate.Rows[i]["TemplateId"]);
    //                            if (dv.Count > 0)
    //                            {
    //                                drTemplateStyle = dv[0].Row;
    //                                string sBegin = "", sEnd = "";
    //                                Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
    //                                MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
    //                            }

    //                            if ((common.myInt(ddlTemplatePatient) == 0)
    //                                || (common.myInt(ddlTemplatePatient) == common.myInt(dtTemplate.Rows[i]["TemplateId"])))
    //                            {
    //                                DataTable dtGroupTable = (DataTable)ViewState["GroupTable"];
    //                                DataView dvGroup = new DataView(dtGroupTable);
    //                                dvGroup.RowFilter = "GroupId=" + common.myInt(ViewState["GroupId"]);
    //                                if (dvGroup.ToTable().Rows.Count > 0)
    //                                {
    //                                    for (int kk = 0; kk < dvGroup.ToTable().Rows.Count; kk++)
    //                                    {
    //                                        StringBuilder sbTemp = new StringBuilder();
    //                                        bindData("1", common.myStr(dvGroup.ToTable().Rows[kk]["TemplateId"]), sbTemp);

    //                                        // bindData("1", common.myStr(dtTemplate.Rows[i]["TemplateId"]), sbTemp);
    //                                        if (sbTemp.ToString() != "")
    //                                            sb.Append(sbTemp + "<br/>");
    //                                        sbTemp = null;
    //                                    }
    //                                }
    //                                else
    //                                {
    //                                    StringBuilder sbTemp = new StringBuilder();
    //                                    bindData("1", common.myStr(dtTemplate.Rows[i]["TemplateId"]), sbTemp);
    //                                    if (sbTemp.ToString() != "")
    //                                        sb.Append(sbTemp + "<br/>");
    //                                    sbTemp = null;
    //                                }
    //                                dtGroupTable.Dispose();
    //                                dvGroup.Dispose();
    //                            }

    //                            Templinespace = "";
    //                        }
    //                        #endregion
    //                        #region Allergy
    //                        if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim() == "Allergies" && common.myStr(dtTemplate.Rows[i]["TemplateIdentification"]).Trim() == "P"
    //                            && common.myStr(dtTemplate.Rows[i]["DataStatus"]).Trim() == "AVAILABLE")
    //                        {
    //                            string strTemplateType = common.myStr(dtTemplate.Rows[i]["PageIdentification"]);
    //                            strTemplateType = strTemplateType.Substring(0, 1);
    //                            sbTemplateStyle = new StringBuilder();
    //                            DataView dv = new DataView(dsTemplateStyle.Tables[0]);
    //                            dv.RowFilter = "PageId=" + common.myInt(dtTemplate.Rows[i]["PageId"]);
    //                            drTemplateStyle = null;// = dv[0].Row;
    //                            if (dv.Count > 0)
    //                            {
    //                                drTemplateStyle = dv[0].Row;
    //                                string sBegin = "", sEnd = "";
    //                                Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
    //                                MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
    //                            }
    //                            StringBuilder sbTemp = new StringBuilder();

    //                            if ((common.myInt(ddlTemplatePatient) == 0)
    //                                 || (common.myInt(ddlTemplatePatient) == 10002))
    //                            {
    //                                bnotes.BindAllergies(RegId, sbTemp, sbTemplateStyle, drTemplateStyle, Page, common.myStr(Session["HospitalLocationId"]),
    //                                           common.myStr(Session["UserID"]), common.myStr(dtTemplate.Rows[i]["PageID"]),
    //                                           common.myDate(sDate[0]).ToString(),
    //                                           common.myDate(sDate[1]).ToString(), 0, "");
    //                                if (sbTemp.ToString() != "")
    //                                    sb.Append(sbTemp + "<br/>");
    //                            }

    //                            drTemplateStyle = null;
    //                            Templinespace = "";
    //                        }
    //                        #endregion
    //                        #region Vitals
    //                        if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim() == "Vitals" && common.myStr(dtTemplate.Rows[i]["TemplateIdentification"]).Trim() == "P"
    //                            && common.myStr(dtTemplate.Rows[i]["DataStatus"]).Trim() == "AVAILABLE")
    //                        {
    //                            string strTemplateType = common.myStr(dtTemplate.Rows[i]["PageIdentification"]);
    //                            strTemplateType = strTemplateType.Substring(0, 1);
    //                            sbTemplateStyle = new StringBuilder();
    //                            DataView dv = new DataView(dsTemplateStyle.Tables[0]);
    //                            dv.RowFilter = "PageId=" + common.myInt(dtTemplate.Rows[i]["PageId"]);
    //                            if (dv.Count > 0)
    //                            {
    //                                drTemplateStyle = dv[0].Row;
    //                                string sBegin = "", sEnd = "";
    //                                Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
    //                                MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
    //                            }
    //                            StringBuilder sbTemp = new StringBuilder();

    //                            if ((common.myInt(ddlTemplatePatient) == 0)
    //                                 || (common.myInt(ddlTemplatePatient) == 10001))
    //                            {
    //                                bnotes.BindVitals(HospitalId.ToString(), EncounterId, sbTemp, sbTemplateStyle, drTemplateStyle,
    //                                                    Page, common.myStr(dtTemplate.Rows[i]["PageId"]), common.myStr(Session["UserID"]),
    //                                                    common.myDate(sDate[0]).ToString(),
    //                                                    common.myDate(sDate[1]).ToString(), 0, sEREncounterId, "");
    //                                if (sbTemp.ToString() != "")
    //                                    sb.Append(sbTemp + "<br/>" + "<br/>");
    //                            }

    //                            drTemplateStyle = null;
    //                            Templinespace = "";
    //                        }
    //                        #endregion
    //                        #region Templates
    //                        if (common.myStr(dtTemplate.Rows[i]["TemplateId"]).Trim() != "" && common.myStr(dtTemplate.Rows[i]["TemplateIdentification"]).Trim() == "T"
    //                            && common.myStr(dtTemplate.Rows[i]["DataStatus"]).Trim() == "AVAILABLE" && common.myStr(dtTemplate.Rows[i]["TemplateCode"]).Trim() != "POC"
    //                            && common.myStr(dtTemplate.Rows[i]["TemplateCode"]).Trim() != "HIS")
    //                        {
    //                            string strTemplateType = common.myStr(dtTemplate.Rows[i]["PageIdentification"]);
    //                            strTemplateType = strTemplateType.Substring(0, 1);
    //                            sbTemplateStyle = new StringBuilder();
    //                            DataView dv = new DataView(dsTemplateStyle.Tables[0]);
    //                            dv.RowFilter = "PageId=" + common.myInt(dtTemplate.Rows[i]["TemplateId"]);
    //                            if (dv.Count > 0)
    //                            {
    //                                drTemplateStyle = dv[0].Row;
    //                                string sBegin = "", sEnd = "";
    //                                Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
    //                                MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
    //                            }


    //                            if ((common.myInt(ddlTemplatePatient) == 0)
    //                                || (common.myInt(ddlTemplatePatient) == common.myInt(dtTemplate.Rows[i]["TemplateId"])))
    //                            {
    //                                DataTable dtGroupTable = (DataTable)ViewState["GroupTable"];
    //                                DataView dvGroup = new DataView(dtGroupTable);
    //                                dvGroup.RowFilter = "GroupId=" + common.myInt(ViewState["GroupId"]);
    //                                if (dvGroup.ToTable().Rows.Count > 0)
    //                                {
    //                                    for (int kk = 0; kk < dvGroup.ToTable().Rows.Count; kk++)
    //                                    {
    //                                        StringBuilder sbTemp = new StringBuilder();
    //                                        bindData("1", common.myStr(dvGroup.ToTable().Rows[kk]["TemplateId"]), sbTemp);

    //                                        // bindData("1", common.myStr(dtTemplate.Rows[i]["TemplateId"]), sbTemp);
    //                                        if (sbTemp.ToString() != "")
    //                                            sb.Append(sbTemp + "<br/>");
    //                                        sbTemp = null;
    //                                    }
    //                                }
    //                                else
    //                                {
    //                                    StringBuilder sbTemp = new StringBuilder();
    //                                    bindData("1", common.myStr(dtTemplate.Rows[i]["TemplateId"]), sbTemp);
    //                                    if (sbTemp.ToString() != "")
    //                                        sb.Append(sbTemp + "<br/>");
    //                                    sbTemp = null;
    //                                }
    //                                dtGroupTable.Dispose();
    //                                dvGroup.Dispose();

    //                                //bindData("1", common.myStr(dtTemplate.Rows[i]["TemplateId"]), sbTemp);
    //                                //if (sbTemp.ToString() != "")
    //                                //    sb.Append(sbTemp + "<br/>");
    //                            }

    //                            Templinespace = "";
    //                        }
    //                        #endregion
    //                        #region Lab
    //                        if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim() == "Lab Test Result" && common.myStr(dtTemplate.Rows[i]["TemplateIdentification"]).Trim() == "P"
    //                            && common.myStr(dtTemplate.Rows[i]["DataStatus"]).Trim() == "AVAILABLE")
    //                        {
    //                            string strTemplateType = common.myStr(dtTemplate.Rows[i]["PageIdentification"]);
    //                            strTemplateType = strTemplateType.Substring(0, 1);
    //                            sbTemplateStyle = new StringBuilder();
    //                            DataView dv = new DataView(dsTemplateStyle.Tables[0]);
    //                            dv.RowFilter = "PageId=" + common.myInt(dtTemplate.Rows[i]["PageId"]);
    //                            if (dv.Count > 0)
    //                            {
    //                                drTemplateStyle = dv[0].Row;
    //                                string sBegin = "", sEnd = "";
    //                                Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
    //                                MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
    //                            }
    //                            StringBuilder sbTemp = new StringBuilder();

    //                            // bnotes.BindLabTestResult(common.myInt(Session["RegistrationId"]), HospitalId, EncounterId, UserId, DoctorId, sbTemp, sbTemplateStyle, drTemplateStyle, Page, common.myInt(Session["FacilityID"]), common.myStr(dtTemplate.Rows[i]["PageId"]), common.myStr(Session["UserID"]));
    //                            sb.Append(sbTemp);
    //                            drTemplateStyle = null;
    //                            Templinespace = "";
    //                        }
    //                        #endregion
    //                        #region Diagnosis
    //                        if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim() == "Diagnosis" && common.myStr(dtTemplate.Rows[i]["TemplateIdentification"]).Trim() == "P"
    //                            && common.myStr(dtTemplate.Rows[i]["DataStatus"]).Trim() == "AVAILABLE")
    //                        {
    //                            string strTemplateType = common.myStr(dtTemplate.Rows[i]["PageIdentification"]);
    //                            strTemplateType = strTemplateType.Substring(0, 1);
    //                            sbTemplateStyle = new StringBuilder();
    //                            DataView dv = new DataView(dsTemplateStyle.Tables[0]);
    //                            dv.RowFilter = "PageId=" + common.myInt(dtTemplate.Rows[i]["PageId"]);
    //                            if (dv.Count > 0)
    //                            {
    //                                drTemplateStyle = dv[0].Row;
    //                                string sBegin = "", sEnd = "";
    //                                Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
    //                                MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
    //                            }
    //                            StringBuilder sbTemp = new StringBuilder();

    //                            if ((common.myInt(ddlTemplatePatient) == 0)
    //                                 || (common.myInt(ddlTemplatePatient) == 10004))
    //                            {
    //                                bnotes.BindAssessments(RegId, HospitalId, EncounterId, Convert.ToInt16(UserId),
    //                                    Request.QueryString["DoctorId"] != null ? Request.QueryString["DoctorId"].ToString() : DoctorId, sbTemp, sbTemplateStyle, drTemplateStyle, Page,
    //                                            common.myStr(dtTemplate.Rows[i]["PageId"]), common.myStr(Session["UserID"]),
    //                                            common.myDate(sDate[0]).ToString(),
    //                                            common.myDate(sDate[1]).ToString(), 0, sEREncounterId, "");
    //                                if (sbTemp.ToString() != "")
    //                                    sb.Append(sbTemp + "<br/>");
    //                            }

    //                            drTemplateStyle = null;
    //                            Templinespace = "";
    //                        }
    //                        #endregion
    //                        #region Provisional Diagnosis
    //                        if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim() == "Provisional Diagnosis" && common.myStr(dtTemplate.Rows[i]["TemplateIdentification"]).Trim() == "P"
    //                            && common.myStr(dtTemplate.Rows[i]["DataStatus"]).Trim() == "AVAILABLE")
    //                        {
    //                            string strTemplateType = common.myStr(dtTemplate.Rows[i]["PageIdentification"]);
    //                            strTemplateType = strTemplateType.Substring(0, 1);
    //                            sbTemplateStyle = new StringBuilder();
    //                            DataView dv = new DataView(dsTemplateStyle.Tables[0]);
    //                            dv.RowFilter = "PageId=" + common.myInt(dtTemplate.Rows[i]["PageId"]);
    //                            if (dv.Count > 0)
    //                            {
    //                                drTemplateStyle = dv[0].Row;
    //                                string sBegin = "", sEnd = "";
    //                                Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
    //                                MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
    //                            }
    //                            StringBuilder sbTemp = new StringBuilder();
    //                            if ((common.myInt(ddlTemplatePatient) == 0)
    //                                 || (common.myInt(ddlTemplatePatient) == 10004))
    //                            {
    //                                bnotes.BindPatientProvisionalDiagnosis(RegId, HospitalId, EncounterId, Convert.ToInt16(UserId),
    //                                            Request.QueryString["DoctorId"] != null ? Request.QueryString["DoctorId"].ToString() : DoctorId, sbTemp, sbTemplateStyle, drTemplateStyle, Page,
    //                                            common.myStr(dtTemplate.Rows[i]["PageId"]), common.myStr(Session["UserID"]),
    //                                            common.myDate(sDate[0]).ToString("yyyy/MM/dd"),
    //                                            common.myDate(sDate[1]).ToString("yyyy/MM/dd"), 0, sEREncounterId, "");
    //                                if (sbTemp.ToString() != "")
    //                                    sb.Append(sbTemp + "<br/>");
    //                            }
    //                            drTemplateStyle = null;
    //                            Templinespace = "";
    //                        }
    //                        #endregion
    //                        #region commented Prescription
    //                        //if (
    //                        //common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim() == "Prescription" 
    //                        //&& 
    //                        //    common.myStr(dtTemplate.Rows[i]["TemplateIdentification"]).Trim() == "P"
    //                        //    && common.myStr(dtTemplate.Rows[i]["DataStatus"]).Trim() == "AVAILABLE")
    //                        //{
    //                        //    string strTemplateType = common.myStr(dtTemplate.Rows[i]["PageIdentification"]);
    //                        //    strTemplateType = strTemplateType.Substring(0, 1);
    //                        //    sbTemplateStyle = new StringBuilder();
    //                        //    DataView dv = new DataView(dsTemplateStyle.Tables[0]);
    //                        //    dv.RowFilter = "PageId=" + common.myInt(dtTemplate.Rows[i]["PageId"]);

    //                        //    if (dv.Count > 0)
    //                        //    {
    //                        //        drTemplateStyle = dv[0].Row;
    //                        //        string sBegin = "", sEnd = "";
    //                        //        Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
    //                        //        MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
    //                        //    }

    //                        //    StringBuilder sbTemp = new StringBuilder();

    //                        //    if ((common.myInt(ddlTemplatePatient) == 0)
    //                        //         || (common.myInt(ddlTemplatePatient) == 10005))
    //                        //    {
    //                        //        bnotes.BindMedication(EncounterId, HospitalId, RegId, sbTemp, sbTemplateStyle, "P", drTemplateStyle,
    //                        //                       Page, common.myStr(dtTemplate.Rows[i]["PageId"]), common.myStr(Session["UserID"]),
    //                        //                       common.myDate(sDate[0]).ToString(),
    //                        //                       common.myDate(sDate[1]).ToString(), common.myStr(lblVisitType.Text.Substring(0, 1)), "");

    //                        //        if (sbTemp.ToString() != "")
    //                        //            sb.Append(sbTemp + "<br/>");
    //                        //    }

    //                        //    drTemplateStyle = null;
    //                        //    Templinespace = "";
    //                        //}
    //                        #endregion
    //                        #region commented Current Medication
    //                        //if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim() == "Current Medication" && common.myStr(dtTemplate.Rows[i]["TemplateIdentification"]).Trim() == "P"
    //                        //    && common.myStr(dtTemplate.Rows[i]["DataStatus"]).Trim() == "AVAILABLE")
    //                        //{
    //                        //    string strTemplateType = common.myStr(dtTemplate.Rows[i]["PageIdentification"]);
    //                        //    strTemplateType = strTemplateType.Substring(0, 1);
    //                        //    sbTemplateStyle = new StringBuilder();
    //                        //    DataView dv = new DataView(dsTemplateStyle.Tables[0]);
    //                        //    dv.RowFilter = "PageId=" + common.myInt(dtTemplate.Rows[i]["PageId"]);
    //                        //    if (dv.Count > 0)
    //                        //    {
    //                        //        drTemplateStyle = dv[0].Row;
    //                        //        string sBegin = "", sEnd = "";
    //                        //        Templinespace = common.myStr(common.myInt(drTemplateStyle["TemplateSpaceNumber"]) - 1);
    //                        //        MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
    //                        //    }
    //                        //    StringBuilder sbTemp = new StringBuilder();
    //                        //    if ((common.myInt(ddlTemplatePatient) == 0)
    //                        //         || (common.myInt(ddlTemplatePatient) == 10005))
    //                        //    {
    //                        //        bnotes.BindMedication(EncounterId, HospitalId, RegId, sbTemp, sbTemplateStyle, "C", drTemplateStyle,
    //                        //                        Page, common.myStr(dtTemplate.Rows[i]["PageId"]), common.myStr(Session["UserID"]),
    //                        //                        common.myDate(sDate[0]).ToString(),
    //                        //                        common.myDate(sDate[1]).ToString(), common.myStr(lblVisitType.Text.Substring(0, 1)), "");
    //                        //        if (sbTemp.ToString() != "")
    //                        //            sb.Append(sbTemp + "<br/>");
    //                        //    }
    //                        //    drTemplateStyle = null;
    //                        //    Templinespace = "";
    //                        //}
    //                        #endregion
    //                        #region Prescription
    //                        DataSet dsTemplateData = new DataSet();
    //                        DataTable dtEncounter = new DataTable();
    //                        DataTable dtTemplatePrescription = new DataTable();
    //                        DataView dvDataFilter = new DataView();
    //                        BaseC.clsEMR emr = new BaseC.clsEMR(sConString);
    //                        //string sTemplateName = common.myStr(ddlTemplatePatient.SelectedItem.Text) == "ALL" ? "" : common.myStr(ddlTemplatePatient.SelectedItem.Text);
    //                        string sTemplateName = "";
    //                        if (ddlTemplatePatient != 0)
    //                        {
    //                            //dsTemplateData = emr.getEMRPrintCaseSheetDate(common.myInt(Session["HospitalLocationId"]),
    //                            //    common.myInt(Session["FacilityId"]), common.myInt(ViewState["RegistrationId"]), common.myInt(ViewState["EncounterId"]), common.myDate(DateTime.Now).AddDays(-365).ToString("yyyy/MM/dd"),
    //                            //    common.myDate((DateTime.Now).ToString("yyyy/MM/dd"), sTemplateName, common.myInt(ddlTemplatePatient.SelectedValue),
    //                            //    common.myStr(ddlTemplatePatient.SelectedItem.Attributes["TemplateType"]), false, 0);
    //                        }
    //                        //endDate.AddDays(addedDays);
    //                        else
    //                        {
    //                            dsTemplateData = emr.getEMRPrintCaseSheetDate(common.myInt(Session["HospitalLocationId"]),
    //                            common.myInt(Session["FacilityId"]), common.myInt(Session["RegistrationId"]), common.myInt(Session["EncounterId"]), common.myDate(DateTime.Now).AddDays(-365).ToString("yyyy/MM/dd"),
    //                             common.myDate(DateTime.Now).ToString("yyyy/MM/dd"), sTemplateName, 0, "", false, 0);
    //                        }
    //                        dvDataFilter = new DataView(dsTemplateData.Tables[21]);
    //                        dtEncounter = dsTemplateData.Tables[22];
    //                        if (dvDataFilter.ToTable().Rows.Count > 0)
    //                        {
    //                            dtTemplatePrescription = dvDataFilter.ToTable();
    //                            #region Template Wise
    //                            {
    //                                for (int iEn = 0; iEn < dtEncounter.Rows.Count; iEn++)
    //                                {
    //                                    if (common.myStr(dtTemplatePrescription.Rows[i]["TemplateName"]).Trim() == "Prescription"
    //                                     && common.myStr(dtTemplatePrescription.Rows[i]["TemplateType"]).Trim() == "S"
    //                                    )
    //                                    {
    //                                        sbTemplateStyle = new StringBuilder();
    //                                        DataView dv = new DataView(dsTemplateStyle.Tables[0]);
    //                                        StringBuilder sbTemp = new StringBuilder();
    //                                        StringBuilder TemplateString = new StringBuilder();
    //                                        BindCaseSheet BindCaseSheet = new BindCaseSheet(sConString);
    //                                        dv.RowFilter = "PageId=" + common.myInt(dtTemplatePrescription.Rows[i]["PageId"]).ToString();

    //                                        if (dv.Count > 0)
    //                                        {
    //                                            drTemplateStyle = dv[0].Row;
    //                                            string sBegin = "", sEnd = "";
    //                                            Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
    //                                            MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
    //                                        }

    //                                        dv.Dispose();
    //                                        sbTemp = new StringBuilder();

    //                                        if ((common.myInt(ddlTemplatePatient) == 0)
    //                                             || (common.myInt(ddlTemplatePatient) == 10005))
    //                                        {
    //                                            DataSet dsMedication = new DataSet();
    //                                            DataView dvTable1 = new DataView(dsTemplateData.Tables[10]);

    //                                            dvTable1.ToTable().TableName = "Item";

    //                                            dvTable1.RowFilter = "EncounterId=" + common.myInt(dtEncounter.Rows[iEn]["EncounterId"]);


    //                                            dsMedication.Tables.Add(dvTable1.ToTable());


    //                                            dvTable1.Dispose();

    //                                            #region Call Medication Template data

    //                                            BindCaseSheet.BindMedication(dsMedication, sbTemp, sbTemplateStyle, drTemplateStyle, Page, common.myStr(dtTemplatePrescription.Rows[i]["PageId"]),
    //                                                           common.myInt(Session["UserID"]).ToString(), "");
    //                                            #endregion

    //                                            dsMedication.Dispose();
    //                                            if (sbTemp.ToString() != "")
    //                                                //TemplateString.Append(sbTemp + "<br/>");
    //                                                sb.Append(sbTemp + "<br/>");
    //                                            sbTemp = null;
    //                                            sbTemplateStyle = null;

    //                                        }
    //                                    }
    //                                    //if (sbTemp.ToString() != "")
    //                                    //    sb.Append(sbTemp + "<br/>");

    //                                    drTemplateStyle = null;
    //                                    Templinespace = "";
    //                                }
    //                            }
    //                        }
    //                        #endregion
    //                        #endregion
    //                        #region Order
    //                        if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim() == "Order" && common.myStr(dtTemplate.Rows[i]["TemplateIdentification"]).Trim() == "P"
    //                                    && common.myStr(dtTemplate.Rows[i]["DataStatus"]).Trim() == "AVAILABLE")
    //                        {
    //                            string strTemplateType = common.myStr(dtTemplate.Rows[i]["PageIdentification"]);
    //                            strTemplateType = strTemplateType.Substring(0, 1);
    //                            sbTemplateStyle = new StringBuilder();
    //                            DataView dv = new DataView(dsTemplateStyle.Tables[0]);
    //                            dv.RowFilter = "PageId=" + common.myInt(dtTemplate.Rows[i]["PageId"]);
    //                            if (dv.Count > 0)
    //                            {
    //                                drTemplateStyle = dv[0].Row;
    //                                string sBegin = "", sEnd = "";
    //                                Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
    //                                MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
    //                            }
    //                            StringBuilder sbTemp = new StringBuilder();

    //                            if ((common.myInt(ddlTemplatePatient) == 0)
    //                                 || (common.myInt(ddlTemplatePatient) == 10006))
    //                            {
    //                                bnotes.BindOrders(common.myInt(Session["RegistrationId"]), HospitalId, EncounterId,
    //                                        Convert.ToInt16(UserId), Request.QueryString["DoctorId"] != null ? Request.QueryString["DoctorId"].ToString() : DoctorId, sbTemp, sbTemplateStyle, drTemplateStyle, Page,
    //                                        common.myStr(dtTemplate.Rows[i]["PageId"]), common.myStr(Session["UserID"]),
    //                                        common.myDate(sDate[0]).ToString(),
    //                                        common.myDate(sDate[1]).ToString(), sEREncounterId, "");
    //                                if (sbTemp.ToString() != "")
    //                                    sb.Append(sbTemp + "<br/>");
    //                            }

    //                            drTemplateStyle = null;
    //                            Templinespace = "";
    //                        }
    //                        #endregion
    //                        #region Immunization
    //                        if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim() == "Immunization" && common.myStr(dtTemplate.Rows[i]["TemplateIdentification"]).Trim() == "P"
    //                            && common.myStr(dtTemplate.Rows[i]["DataStatus"]).Trim() == "AVAILABLE")
    //                        {
    //                            string strTemplateType = common.myStr(dtTemplate.Rows[i]["PageIdentification"]);
    //                            strTemplateType = strTemplateType.Substring(0, 1);
    //                            sbTemplateStyle = new StringBuilder();
    //                            DataView dv = new DataView(dsTemplateStyle.Tables[0]);
    //                            dv.RowFilter = "PageId=" + common.myInt(dtTemplate.Rows[i]["PageId"]);
    //                            if (dv.Count > 0)
    //                            {
    //                                drTemplateStyle = dv[0].Row;
    //                                string sBegin = "", sEnd = "";
    //                                Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
    //                                MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
    //                            }
    //                            StringBuilder sbTemp = new StringBuilder();

    //                            if ((common.myInt(ddlTemplatePatient) == 0)
    //                                 || (common.myInt(ddlTemplatePatient) == 10007))
    //                            {
    //                                bnotes.BindImmunization(HospitalId.ToString(), common.myInt(Session["RegistrationId"]),
    //                                            common.myInt(Session["EncounterId"]), sbTemp, sbTemplateStyle, drTemplateStyle, Page,
    //                                            common.myStr(dtTemplate.Rows[i]["PageId"]), common.myStr(Session["UserID"]),
    //                                            common.myDate(sDate[0]).ToString(),
    //                                            common.myDate(sDate[1]).ToString(), "");
    //                                if (sbTemp.ToString() != "")
    //                                    sb.Append(sbTemp + "<br/>");
    //                            }

    //                            Templinespace = "";
    //                        }
    //                        #endregion
    //                        #region Doctor Progress Note
    //                        if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim() == "Doctor Progress Note" && common.myStr(dtTemplate.Rows[i]["TemplateIdentification"]).Trim() == "P"
    //                            && common.myStr(dtTemplate.Rows[i]["DataStatus"]).Trim() == "AVAILABLE")
    //                        {
    //                            string strTemplateType = common.myStr(dtTemplate.Rows[i]["PageIdentification"]);
    //                            strTemplateType = strTemplateType.Substring(0, 1);
    //                            sbTemplateStyle = new StringBuilder();
    //                            DataView dv = new DataView(dsTemplateStyle.Tables[0]);
    //                            dv.RowFilter = "PageId=" + common.myInt(dtTemplate.Rows[i]["PageId"]);
    //                            if (dv.Count > 0)
    //                            {
    //                                drTemplateStyle = dv[0].Row;
    //                                string sBegin = "", sEnd = "";
    //                                Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
    //                                MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
    //                            }
    //                            StringBuilder sbTemp = new StringBuilder();

    //                            if ((common.myInt(ddlTemplatePatient) == 0)
    //                                 || (common.myInt(ddlTemplatePatient) == 10009))
    //                            {
    //                                bnotes.BindDoctorProgressNote(HospitalId.ToString(), common.myInt(Session["RegistrationId"]),
    //                                            Request.QueryString["DoctorId"] != null ? common.myInt(Request.QueryString["DoctorId"]) : common.myInt(Session["DoctorId"]), sbTemp, sbTemplateStyle, drTemplateStyle, Page,
    //                                            common.myStr(dtTemplate.Rows[i]["PageId"]), common.myStr(Session["UserID"]),
    //                                            common.myDate(sDate[0]).ToString(),
    //                                            common.myDate(sDate[1]).ToString(), "");
    //                                if (sbTemp.ToString() != "")
    //                                    sb.Append(sbTemp + "<br/>");
    //                            }

    //                            Templinespace = "";
    //                        }
    //                        #endregion
    //                        #region Admission Request
    //                        if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim() == "Admission Request" && common.myStr(dtTemplate.Rows[i]["TemplateIdentification"]).Trim() == "P"
    //                            && common.myStr(dtTemplate.Rows[i]["DataStatus"]).Trim() == "AVAILABLE")
    //                        {
    //                            string strTemplateType = common.myStr(dtTemplate.Rows[i]["PageIdentification"]);
    //                            strTemplateType = strTemplateType.Substring(0, 1);
    //                            sbTemplateStyle = new StringBuilder();
    //                            DataView dv = new DataView(dsTemplateStyle.Tables[0]);
    //                            dv.RowFilter = "PageId=" + common.myInt(dtTemplate.Rows[i]["PageId"]);
    //                            if (dv.Count > 0)
    //                            {
    //                                drTemplateStyle = dv[0].Row;
    //                                string sBegin = "", sEnd = "";
    //                                Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
    //                                MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
    //                            }
    //                            StringBuilder sbTemp = new StringBuilder();

    //                            if ((common.myInt(ddlTemplatePatient) == 0)
    //                                 || (common.myInt(ddlTemplatePatient) == 10009))
    //                            {
    //                                bnotes.BindPatientBooking(HospitalId, common.myInt(Session["FacilityId"]), common.myInt(Session["RegistrationId"]),
    //                                            common.myInt(Session["EncounterId"]), sbTemp, sbTemplateStyle, drTemplateStyle, Page,
    //                                            common.myStr(dtTemplate.Rows[i]["PageId"]));
    //                                if (sbTemp.ToString() != "")
    //                                    sb.Append(sbTemp + "<br/>");
    //                            }

    //                            Templinespace = "";
    //                        }
    //                        #endregion
    //                        #region Referral Request
    //                        if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim() == "Referral Request" && common.myStr(dtTemplate.Rows[i]["TemplateIdentification"]).Trim() == "P"
    //                            && common.myStr(dtTemplate.Rows[i]["DataStatus"]).Trim() == "AVAILABLE")
    //                        {
    //                            string strTemplateType = common.myStr(dtTemplate.Rows[i]["PageIdentification"]);
    //                            strTemplateType = strTemplateType.Substring(0, 1);
    //                            sbTemplateStyle = new StringBuilder();
    //                            DataView dv = new DataView(dsTemplateStyle.Tables[0]);
    //                            dv.RowFilter = "PageId=" + common.myInt(dtTemplate.Rows[i]["PageId"]);
    //                            if (dv.Count > 0)
    //                            {
    //                                drTemplateStyle = dv[0].Row;
    //                                string sBegin = "", sEnd = "";
    //                                Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
    //                                MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
    //                            }
    //                            StringBuilder sbTemp = new StringBuilder();

    //                            if ((common.myInt(ddlTemplatePatient) == 0)
    //                                 || (common.myInt(ddlTemplatePatient) == 10009))
    //                            {
    //                                bnotes.BindReferalHistory(common.myStr(Session["RegistrationNo"]), HospitalId, common.myInt(EncounterId), 0,
    //                                             sbTemp, sbTemplateStyle, drTemplateStyle, Page,
    //                                            common.myStr(dtTemplate.Rows[i]["PageId"]), sDate[0], sDate[1], "", "");
    //                                if (sbTemp.ToString() != "")
    //                                    sb.Append(sbTemp + "<br/>");
    //                            }

    //                            Templinespace = "";
    //                        }
    //                        #endregion
    //                        #region Non Drug Order(Doctor Order)
    //                        if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim() == "Non Drug Order" && common.myStr(dtTemplate.Rows[i]["TemplateIdentification"]).Trim() == "P"
    //                            && common.myStr(dtTemplate.Rows[i]["DataStatus"]).Trim() == "AVAILABLE")
    //                        {
    //                            string strTemplateType = common.myStr(dtTemplate.Rows[i]["PageIdentification"]);
    //                            strTemplateType = strTemplateType.Substring(0, 1);
    //                            sbTemplateStyle = new StringBuilder();
    //                            DataView dv = new DataView(dsTemplateStyle.Tables[0]);
    //                            dv.RowFilter = "PageId=" + common.myInt(dtTemplate.Rows[i]["PageId"]);
    //                            if (dv.Count > 0)
    //                            {
    //                                drTemplateStyle = dv[0].Row;
    //                                string sBegin = "", sEnd = "";
    //                                Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
    //                                MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
    //                            }
    //                            StringBuilder sbTemp = new StringBuilder();

    //                            if ((common.myInt(ddlTemplatePatient) == 0)
    //                                 || (common.myInt(ddlTemplatePatient) == 10009))
    //                            {
    //                                bnotes.BindNonDrugOrder(common.myInt(Session["RegistrationId"]), HospitalId, common.myInt(EncounterId), common.myInt(Session["UserId"]),
    //                                             sbTemp, sbTemplateStyle, drTemplateStyle, Page,
    //                                            common.myStr(dtTemplate.Rows[i]["PageId"]), sDate[0], sDate[1], "", "");
    //                                if (sbTemp.ToString() != "")
    //                                    sb.Append(sbTemp + "<br/>");
    //                            }

    //                            Templinespace = "";
    //                        }
    //                        #endregion
    //                        #region Diet Order
    //                        if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim() == "Diet Order" && common.myStr(dtTemplate.Rows[i]["TemplateIdentification"]).Trim() == "P"
    //                            && common.myStr(dtTemplate.Rows[i]["DataStatus"]).Trim() == "AVAILABLE")
    //                        {
    //                            string strTemplateType = common.myStr(dtTemplate.Rows[i]["PageIdentification"]);
    //                            strTemplateType = strTemplateType.Substring(0, 1);
    //                            sbTemplateStyle = new StringBuilder();
    //                            DataView dv = new DataView(dsTemplateStyle.Tables[0]);
    //                            dv.RowFilter = "PageId=" + common.myInt(dtTemplate.Rows[i]["PageId"]);
    //                            if (dv.Count > 0)
    //                            {
    //                                drTemplateStyle = dv[0].Row;
    //                                string sBegin = "", sEnd = "";
    //                                Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
    //                                MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
    //                            }
    //                            StringBuilder sbTemp = new StringBuilder();

    //                            if ((common.myInt(ddlTemplatePatient) == 0)
    //                                 || (common.myInt(ddlTemplatePatient) == 10009))
    //                            {
    //                                bnotes.BindDietOrderInNote(common.myInt(Session["RegistrationId"]), HospitalId, common.myInt(EncounterId),
    //                                             sbTemp, sbTemplateStyle, drTemplateStyle, Page, common.myStr(dtTemplate.Rows[i]["PageId"]), sDate[0], sDate[1], "");
    //                                if (sbTemp.ToString() != "")
    //                                    sb.Append(sbTemp + "<br/>");
    //                            }

    //                            Templinespace = "";
    //                        }
    //                        #endregion
    //                        #region Templates
    //                        if (common.myStr(dtTemplate.Rows[i]["TemplateId"]).Trim() != "" && common.myStr(dtTemplate.Rows[i]["TemplateIdentification"]).Trim() == "T"
    //                            && common.myStr(dtTemplate.Rows[i]["DataStatus"]).Trim() == "AVAILABLE" && common.myStr(dtTemplate.Rows[i]["TemplateCode"]).Trim() == "POC")
    //                        {
    //                            string strTemplateType = common.myStr(dtTemplate.Rows[i]["PageIdentification"]);
    //                            strTemplateType = strTemplateType.Substring(0, 1);
    //                            sbTemplateStyle = new StringBuilder();
    //                            DataView dv = new DataView(dsTemplateStyle.Tables[0]);
    //                            dv.RowFilter = "PageId=" + common.myInt(dtTemplate.Rows[i]["TemplateId"]);
    //                            if (dv.Count > 0)
    //                            {
    //                                drTemplateStyle = dv[0].Row;
    //                                string sBegin = "", sEnd = "";
    //                                Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
    //                                MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
    //                            }

    //                            if ((common.myInt(ddlTemplatePatient) == 0)
    //                                || (common.myInt(ddlTemplatePatient) == common.myInt(dtTemplate.Rows[i]["TemplateId"])))
    //                            {
    //                                DataTable dtGroupTable = (DataTable)ViewState["GroupTable"];
    //                                DataView dvGroup = new DataView(dtGroupTable);
    //                                dvGroup.RowFilter = "GroupId=" + common.myInt(ViewState["GroupId"]);
    //                                if (dvGroup.ToTable().Rows.Count > 0)
    //                                {
    //                                    for (int kk = 0; kk < dvGroup.ToTable().Rows.Count; kk++)
    //                                    {
    //                                        StringBuilder sbTemp = new StringBuilder();
    //                                        bindData("1", common.myStr(dvGroup.ToTable().Rows[kk]["TemplateId"]), sbTemp);

    //                                        // bindData("1", common.myStr(dtTemplate.Rows[i]["TemplateId"]), sbTemp);
    //                                        if (sbTemp.ToString() != "")
    //                                            sb.Append(sbTemp + "<br/>");
    //                                        sbTemp = null;
    //                                    }
    //                                }
    //                                else
    //                                {
    //                                    StringBuilder sbTemp = new StringBuilder();
    //                                    bindData("1", common.myStr(dtTemplate.Rows[i]["TemplateId"]), sbTemp);
    //                                    if (sbTemp.ToString() != "")
    //                                        sb.Append(sbTemp + "<br/>");
    //                                    sbTemp = null;
    //                                }
    //                                dtGroupTable.Dispose();
    //                                dvGroup.Dispose();
    //                                //bindData("1", common.myStr(dtTemplate.Rows[i]["TemplateId"]), sbTemp);
    //                                //if (sbTemp.ToString() != "")
    //                                //    sb.Append(sbTemp + "<br/>");
    //                            }
    //                            Templinespace = "";
    //                        }
    //                        #endregion
    //                        #region Daily Injections
    //                        if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim() == "Daily Injections" && common.myStr(dtTemplate.Rows[i]["TemplateIdentification"]).Trim() == "P"
    //                            && common.myStr(dtTemplate.Rows[i]["DataStatus"]).Trim() == "AVAILABLE")
    //                        {
    //                            string strTemplateType = common.myStr(dtTemplate.Rows[i]["PageIdentification"]);
    //                            strTemplateType = strTemplateType.Substring(0, 1);
    //                            sbTemplateStyle = new StringBuilder();
    //                            DataView dv = new DataView(dsTemplateStyle.Tables[0]);
    //                            dv.RowFilter = "PageId=" + common.myInt(dtTemplate.Rows[i]["PageId"]);
    //                            if (dv.Count > 0)
    //                            {
    //                                drTemplateStyle = dv[0].Row;
    //                                string sBegin = "", sEnd = "";
    //                                Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
    //                                MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
    //                            }
    //                            StringBuilder sbTemp = new StringBuilder();
    //                            if ((common.myInt(ddlTemplatePatient) == 0)
    //                                || (common.myInt(ddlTemplatePatient) == 10008))
    //                            {
    //                                bnotes.BindInjection(HospitalId.ToString(), common.myInt(Session["RegistrationId"]),
    //                                            common.myInt(Session["EncounterId"]), sbTemp, sbTemplateStyle, drTemplateStyle, Page,
    //                                            common.myStr(dtTemplate.Rows[i]["PageId"]), common.myStr(Session["UserID"]),
    //                                            common.myDate(sDate[0]).ToString(),
    //                                            common.myDate(sDate[1]).ToString(), "");

    //                                sb.Append(sbTemp + "<br/>");
    //                            }
    //                            Templinespace = "";
    //                        }
    //                        #endregion
    //                        sb.Append("</span>");
    //                    }
    //                }
    //                else
    //                {
    //                    #region Complaints
    //                    if ((common.myInt(ViewState["templateid_new"]) == 0)
    //                             || (common.myInt(ViewState["templateid_new"]) == 98))
    //                    {
    //                        string strTemplateType = common.myStr(dtTemplate.Rows[i]["PageIdentification"]);
    //                        strTemplateType = strTemplateType.Substring(0, 1);
    //                        sbTemplateStyle = new StringBuilder();
    //                        DataView dv = new DataView(dsTemplateStyle.Tables[0]);
    //                        dv.RowFilter = "PageId=" + common.myInt(dtTemplate.Rows[i]["PageId"]);
    //                        if (dv.Count > 0)
    //                        {
    //                            drTemplateStyle = dv[0].Row;
    //                            string sBegin = "", sEnd = "";
    //                            Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
    //                            MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
    //                        }
    //                        StringBuilder sbTemp = new StringBuilder();
    //                        if ((common.myInt(ViewState["templateid_new"]) == 0)
    //                             || (common.myInt(ViewState["templateid_new"]) == 98))
    //                        {
    //                            //Replace encounterId make it 0  ------ 24-07-2014  -- previous   bnotes.BindProblemsHPI(RegId, HospitalId, EncounterId, sbTemp, sbTemplateStyle, drTemplateStyle,
    //                            bnotes.BindProblemsHPI(RegId, HospitalId, EncounterId, sbTemp, sbTemplateStyle, drTemplateStyle,
    //                                           Page, common.myStr(dtTemplate.Rows[i]["PageId"]), common.myStr(Session["UserID"]),
    //                                           common.myDate(sDate[0]).ToString(),
    //                                           common.myDate(sDate[1]).ToString(), "");
    //                            if (sbTemp.ToString() != "")
    //                                sb.Append(sbTemp + "<br/>" + "<br/>");
    //                        }
    //                        drTemplateStyle = null;
    //                        Templinespace = "";
    //                    }
    //                    #endregion
    //                    #region Templates
    //                    if ((common.myInt(ViewState["templateid_new"]) != 8) && (common.myInt(ViewState["templateid_new"]) != 14)
    //                    && (common.myInt(ViewState["templateid_new"]) != 17) && (common.myInt(ViewState["templateid_new"]) != 98)
    //                    && (common.myInt(ViewState["templateid_new"]) != 133) && (common.myInt(ViewState["templateid_new"]) != 153)
    //                    && (common.myInt(ViewState["templateid_new"]) != 1013) && (common.myInt(ViewState["templateid_new"]) != 1081)
    //                         && (common.myInt(ViewState["templateid_new"]) != 1085) && (common.myInt(ViewState["templateid_new"]) != 1098)
    //                         && (common.myInt(ViewState["templateid_new"]) != 1166) && (common.myInt(ViewState["templateid_new"]) != 1172))
    //                    {
    //                        string strTemplateType = common.myStr(dtTemplate.Rows[i]["PageIdentification"]);
    //                        strTemplateType = strTemplateType.Substring(0, 1);
    //                        sbTemplateStyle = new StringBuilder();
    //                        DataView dv = new DataView(dsTemplateStyle.Tables[0]);
    //                        dv.RowFilter = "PageId=" + common.myInt(ViewState["templateid_new"]).ToString();
    //                        if (dv.Count > 0)
    //                        {
    //                            drTemplateStyle = dv[0].Row;
    //                            string sBegin = "", sEnd = "";
    //                            Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
    //                            MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
    //                        }



    //                        DataTable dtGroupTable = (DataTable)ViewState["GroupTable"];
    //                        DataView dvGroup = new DataView(dtGroupTable);
    //                        dvGroup.RowFilter = "GroupId=" + common.myInt(ViewState["GroupId"]);
    //                        if (dvGroup.ToTable().Rows.Count > 0)
    //                        {
    //                            for (int kk = 0; kk < dvGroup.ToTable().Rows.Count; kk++)
    //                            {
    //                                StringBuilder sbTemp = new StringBuilder();
    //                                bindData("1", common.myStr(dvGroup.ToTable().Rows[kk]["TemplateId"]), sbTemp);

    //                                // bindData("1", common.myStr(dtTemplate.Rows[i]["TemplateId"]), sbTemp);
    //                                if (sbTemp.ToString() != "")
    //                                    sb.Append(sbTemp + "<br/>");
    //                                sbTemp = null;
    //                            }
    //                        }
    //                        else
    //                        {
    //                            StringBuilder sbTemp = new StringBuilder();
    //                            bindData("1", common.myStr(dtTemplate.Rows[i]["TemplateId"]), sbTemp);
    //                            if (sbTemp.ToString() != "")
    //                                sb.Append(sbTemp + "<br/>");
    //                            sbTemp = null;
    //                        }
    //                        dtGroupTable.Dispose();
    //                        dvGroup.Dispose();

    //                        //bindData("1", common.myStr(ViewState["templateid_new"]), sbTemp);
    //                        //if (sbTemp.ToString() != "")
    //                        //    sb.Append(sbTemp + "<br/>");
    //                        Templinespace = "";
    //                    }
    //                    #endregion
    //                    #region Allergies
    //                    if ((common.myInt(ViewState["templateid_new"]) == 0) || (common.myInt(ViewState["templateid_new"]) == 8))
    //                    {
    //                        string strTemplateType = common.myStr(dtTemplate.Rows[i]["PageIdentification"]);
    //                        strTemplateType = strTemplateType.Substring(0, 1);
    //                        sbTemplateStyle = new StringBuilder();
    //                        DataView dv = new DataView(dsTemplateStyle.Tables[0]);
    //                        dv.RowFilter = "PageId=" + common.myInt(dtTemplate.Rows[i]["PageId"]);
    //                        drTemplateStyle = null;// = dv[0].Row;
    //                        if (dv.Count > 0)
    //                        {
    //                            drTemplateStyle = dv[0].Row;
    //                            string sBegin = "", sEnd = "";
    //                            Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
    //                            MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
    //                        }
    //                        StringBuilder sbTemp = new StringBuilder();
    //                        if ((common.myInt(ViewState["templateid_new"]) == 0)
    //                             || (common.myInt(ViewState["templateid_new"]) == 8))
    //                        {
    //                            bnotes.BindAllergies(RegId, sbTemp, sbTemplateStyle, drTemplateStyle, Page, common.myStr(Session["HospitalLocationId"]),
    //                                       common.myStr(Session["UserID"]), common.myStr("0"),//common.myStr(Session["UserID"]), common.myStr(dtTemplate.Rows[i]["PageID"]),
    //                                       common.myDate(sDate[0]).ToString(),
    //                                       common.myDate(sDate[1]).ToString(), 0, "");
    //                            if (sbTemp.ToString() != "")
    //                                sb.Append(sbTemp + "<br/>");
    //                        }

    //                        drTemplateStyle = null;
    //                        Templinespace = "";
    //                    }
    //                    #endregion
    //                    #region vital
    //                    if ((common.myInt(ViewState["templateid_new"]) == 0)
    //                             || (common.myInt(ViewState["templateid_new"]) == 14))
    //                    {
    //                        string strTemplateType = common.myStr(dtTemplate.Rows[i]["PageIdentification"]);
    //                        strTemplateType = strTemplateType.Substring(0, 1);
    //                        sbTemplateStyle = new StringBuilder();
    //                        DataView dv = new DataView(dsTemplateStyle.Tables[0]);
    //                        dv.RowFilter = "PageId=" + common.myInt(dtTemplate.Rows[i]["PageId"]);
    //                        if (dv.Count > 0)
    //                        {
    //                            drTemplateStyle = dv[0].Row;
    //                            string sBegin = "", sEnd = "";
    //                            Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
    //                            MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
    //                        }
    //                        StringBuilder sbTemp = new StringBuilder();
    //                        if ((common.myInt(ViewState["templateid_new"]) == 0)
    //                             || (common.myInt(ViewState["templateid_new"]) == 14))
    //                        {
    //                            bnotes.BindVitals(HospitalId.ToString(), EncounterId, sbTemp, sbTemplateStyle, drTemplateStyle,
    //                                                Page, common.myStr("0"), common.myStr(Session["UserID"]),//Page, common.myStr(dtTemplate.Rows[i]["PageId"]), common.myStr(Session["UserID"]),
    //                                                common.myDate(sDate[0]).ToString(),
    //                                                common.myDate(sDate[1]).ToString(), 0, sEREncounterId, "");
    //                            if (sbTemp.ToString() != "")
    //                                sb.Append(sbTemp + "<br/>" + "<br/>");
    //                        }
    //                        drTemplateStyle = null;
    //                        Templinespace = "";
    //                    }
    //                    #endregion
    //                    #region  Templates
    //                    if (common.myStr(dtTemplate.Rows[i]["TemplateId"]).Trim() != ""
    //                        && common.myStr(dtTemplate.Rows[i]["DataStatus"]).Trim() == "AVAILABLE" && common.myStr(dtTemplate.Rows[i]["TemplateCode"]).Trim() != "POC"
    //                        && common.myStr(dtTemplate.Rows[i]["TemplateCode"]).Trim() != "HIS")
    //                    {
    //                        string strTemplateType = common.myStr(dtTemplate.Rows[i]["PageIdentification"]);
    //                        strTemplateType = strTemplateType.Substring(0, 1);
    //                        sbTemplateStyle = new StringBuilder();
    //                        DataView dv = new DataView(dsTemplateStyle.Tables[0]);
    //                        dv.RowFilter = "PageId=" + common.myInt(dtTemplate.Rows[i]["TemplateId"]);
    //                        if (dv.Count > 0)
    //                        {
    //                            drTemplateStyle = dv[0].Row;
    //                            string sBegin = "", sEnd = "";
    //                            Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
    //                            MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
    //                        }

    //                        if ((common.myInt(ViewState["templateid_new"]) == 0)
    //                            || (common.myInt(ViewState["templateid_new"]) == common.myInt(dtTemplate.Rows[i]["TemplateId"])))
    //                        {
    //                            DataTable dtGroupTable = (DataTable)ViewState["GroupTable"];
    //                            DataView dvGroup = new DataView(dtGroupTable);
    //                            dvGroup.RowFilter = "GroupId=" + common.myInt(ViewState["GroupId"]);
    //                            if (dvGroup.ToTable().Rows.Count > 0)
    //                            {
    //                                for (int kk = 0; kk < dvGroup.ToTable().Rows.Count; kk++)
    //                                {
    //                                    StringBuilder sbTemp = new StringBuilder();
    //                                    bindData("1", common.myStr(dvGroup.ToTable().Rows[kk]["TemplateId"]), sbTemp);

    //                                    // bindData("1", common.myStr(dtTemplate.Rows[i]["TemplateId"]), sbTemp);
    //                                    if (sbTemp.ToString() != "")
    //                                        sb.Append(sbTemp + "<br/>");
    //                                    sbTemp = null;
    //                                }
    //                            }
    //                            else
    //                            {
    //                                StringBuilder sbTemp = new StringBuilder();
    //                                bindData("1", common.myStr(dtTemplate.Rows[i]["TemplateId"]), sbTemp);
    //                                if (sbTemp.ToString() != "")
    //                                    sb.Append(sbTemp + "<br/>");
    //                                sbTemp = null;
    //                            }
    //                            dtGroupTable.Dispose();
    //                            dvGroup.Dispose();
    //                            //bindData("1", common.myStr(dtTemplate.Rows[i]["TemplateId"]), sbTemp);
    //                            //if (sbTemp.ToString() != "")
    //                            //    sb.Append(sbTemp + "<br/>");
    //                        }
    //                        Templinespace = "";
    //                    }
    //                    #endregion
    //                    #region Lab Test Result

    //                    if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim() == "Lab Test Result"
    //                        && common.myStr(dtTemplate.Rows[i]["DataStatus"]).Trim() == "AVAILABLE")
    //                    {
    //                        string strTemplateType = common.myStr(dtTemplate.Rows[i]["PageIdentification"]);
    //                        strTemplateType = strTemplateType.Substring(0, 1);
    //                        sbTemplateStyle = new StringBuilder();
    //                        DataView dv = new DataView(dsTemplateStyle.Tables[0]);
    //                        dv.RowFilter = "PageId=" + common.myInt(dtTemplate.Rows[i]["PageId"]);
    //                        if (dv.Count > 0)
    //                        {
    //                            drTemplateStyle = dv[0].Row;
    //                            string sBegin = "", sEnd = "";
    //                            Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
    //                            MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
    //                        }
    //                        StringBuilder sbTemp = new StringBuilder();

    //                        // bnotes.BindLabTestResult(common.myInt(Session["RegistrationId"]), HospitalId, EncounterId, UserId, DoctorId, sbTemp, sbTemplateStyle, drTemplateStyle, Page, common.myInt(Session["FacilityID"]), common.myStr(dtTemplate.Rows[i]["PageId"]), common.myStr(Session["UserID"]));
    //                        sb.Append(sbTemp);
    //                        drTemplateStyle = null;
    //                        Templinespace = "";
    //                    }
    //                    #endregion
    //                    #region  Diagnosis
    //                    if ((common.myInt(ViewState["templateid_new"]) == 113))
    //                    {
    //                        string strTemplateType = common.myStr(dtTemplate.Rows[i]["PageIdentification"]);
    //                        strTemplateType = strTemplateType.Substring(0, 1);
    //                        sbTemplateStyle = new StringBuilder();
    //                        DataView dv = new DataView(dsTemplateStyle.Tables[0]);
    //                        dv.RowFilter = "PageId=" + common.myInt(dtTemplate.Rows[i]["PageId"]);
    //                        if (dv.Count > 0)
    //                        {
    //                            drTemplateStyle = dv[0].Row;
    //                            string sBegin = "", sEnd = "";
    //                            Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
    //                            MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
    //                        }
    //                        StringBuilder sbTemp = new StringBuilder();

    //                        if ((common.myInt(ViewState["templateid_new"]) == 0)
    //                             || (common.myInt(ViewState["templateid_new"]) == 113))
    //                        {
    //                            bnotes.BindAssessments(RegId, HospitalId, EncounterId, Convert.ToInt16(UserId),
    //                                        DoctorId, sbTemp, sbTemplateStyle, drTemplateStyle, Page,
    //                                        common.myStr(dtTemplate.Rows[i]["PageId"]), common.myStr(Session["UserID"]),
    //                                        common.myDate(sDate[0]).ToString(),
    //                                        common.myDate(sDate[1]).ToString(), 0, sEREncounterId, "");
    //                            if (sbTemp.ToString() != "")
    //                                sb.Append(sbTemp + "<br/>");
    //                        }

    //                        drTemplateStyle = null;
    //                        Templinespace = "";
    //                    }
    //                    #endregion
    //                    #region  Prescription
    //                    if ((common.myInt(ViewState["templateid_new"]) == 0) || (common.myInt(ViewState["templateid_new"]) == 153))
    //                    {
    //                        string strTemplateType = common.myStr(dtTemplate.Rows[i]["PageIdentification"]);
    //                        strTemplateType = strTemplateType.Substring(0, 1);
    //                        sbTemplateStyle = new StringBuilder();
    //                        DataView dv = new DataView(dsTemplateStyle.Tables[0]);
    //                        dv.RowFilter = "PageId=" + common.myInt(dtTemplate.Rows[i]["PageId"]);

    //                        if (dv.Count > 0)
    //                        {
    //                            drTemplateStyle = dv[0].Row;
    //                            string sBegin = "", sEnd = "";
    //                            Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
    //                            MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
    //                        }

    //                        StringBuilder sbTemp = new StringBuilder();

    //                        if ((common.myInt(ViewState["templateid_new"]) == 0)
    //                             || (common.myInt(ViewState["templateid_new"]) == 153))
    //                        {
    //                            // replace EnconterId, make it 0 
    //                            bnotes.BindMedication(EncounterId, HospitalId, RegId, sbTemp, sbTemplateStyle, "P", drTemplateStyle,
    //                                            Page, common.myStr(dtTemplate.Rows[i]["PageId"]), common.myStr(Session["UserID"]),
    //                                            common.myDate(sDate[0]).ToString(),
    //                                            common.myDate(sDate[1]).ToString());
    //                            if (sbTemp.ToString() != "")
    //                                sb.Append(sbTemp + "<br/>");
    //                        }

    //                        drTemplateStyle = null;
    //                        Templinespace = "";
    //                    }
    //                    #endregion
    //                    #region  Current Medication
    //                    if ((common.myInt(ViewState["templateid_new"]) == 0) || (common.myInt(ViewState["templateid_new"]) == 10005))
    //                    {
    //                        string strTemplateType = common.myStr(dtTemplate.Rows[i]["PageIdentification"]);
    //                        strTemplateType = strTemplateType.Substring(0, 1);
    //                        sbTemplateStyle = new StringBuilder();
    //                        DataView dv = new DataView(dsTemplateStyle.Tables[0]);
    //                        dv.RowFilter = "PageId=" + common.myInt(dtTemplate.Rows[i]["PageId"]);
    //                        if (dv.Count > 0)
    //                        {
    //                            drTemplateStyle = dv[0].Row;
    //                            string sBegin = "", sEnd = "";
    //                            Templinespace = common.myStr(common.myInt(drTemplateStyle["TemplateSpaceNumber"]) - 1);
    //                            MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
    //                        }
    //                        StringBuilder sbTemp = new StringBuilder();

    //                        if ((common.myInt(ViewState["templateid_new"]) == 0)
    //                             || (common.myInt(ViewState["templateid_new"]) == 10005))
    //                        {
    //                            //// replace EnconterId, make it 0 
    //                            bnotes.BindMedication(0, HospitalId, RegId, sbTemp, sbTemplateStyle, "C", drTemplateStyle,
    //                                            Page, common.myStr(dtTemplate.Rows[i]["PageId"]), common.myStr(Session["UserID"]),
    //                                            common.myDate(sDate[0]).ToString(),
    //                                            common.myDate(sDate[1]).ToString());
    //                            if (sbTemp.ToString() != "")
    //                                sb.Append(sbTemp + "<br/>");
    //                        }

    //                        drTemplateStyle = null;
    //                        Templinespace = "";
    //                    }
    //                    #endregion
    //                    #region  Order
    //                    if ((common.myInt(ViewState["templateid_new"]) == 0) || (common.myInt(ViewState["templateid_new"]) == 17))
    //                    {
    //                        string strTemplateType = common.myStr(dtTemplate.Rows[i]["PageIdentification"]);
    //                        strTemplateType = strTemplateType.Substring(0, 1);
    //                        sbTemplateStyle = new StringBuilder();
    //                        DataView dv = new DataView(dsTemplateStyle.Tables[0]);
    //                        dv.RowFilter = "PageId=" + common.myInt(dtTemplate.Rows[i]["PageId"]);
    //                        if (dv.Count > 0)
    //                        {
    //                            drTemplateStyle = dv[0].Row;
    //                            string sBegin = "", sEnd = "";
    //                            Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
    //                            MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
    //                        }
    //                        StringBuilder sbTemp = new StringBuilder();

    //                        if ((common.myInt(ViewState["templateid_new"]) == 0)
    //                             || (common.myInt(ViewState["templateid_new"]) == 17))
    //                        {
    //                            //// replace EnconterId, make it 0 
    //                            bnotes.BindOrders(common.myInt(Session["RegistrationId"]), HospitalId, EncounterId,
    //                                    Convert.ToInt16(UserId), DoctorId, sbTemp, sbTemplateStyle, drTemplateStyle, Page,
    //                                    common.myStr(dtTemplate.Rows[i]["PageId"]), common.myStr(Session["UserID"]),
    //                                    common.myDate(sDate[0]).ToString(),
    //                                    common.myDate(sDate[1]).ToString(), sEREncounterId, "");
    //                            if (sbTemp.ToString() != "")
    //                                sb.Append(sbTemp + "<br/>");
    //                        }

    //                        drTemplateStyle = null;
    //                        Templinespace = "";
    //                    }
    //                    #endregion
    //                    #region  Immunization
    //                    if ((common.myInt(ViewState["templateid_new"]) == 0) || (common.myInt(ViewState["templateid_new"]) == 10007))
    //                    {
    //                        string strTemplateType = common.myStr(dtTemplate.Rows[i]["PageIdentification"]);
    //                        strTemplateType = strTemplateType.Substring(0, 1);
    //                        sbTemplateStyle = new StringBuilder();
    //                        DataView dv = new DataView(dsTemplateStyle.Tables[0]);
    //                        dv.RowFilter = "PageId=" + common.myInt(dtTemplate.Rows[i]["PageId"]);
    //                        if (dv.Count > 0)
    //                        {
    //                            drTemplateStyle = dv[0].Row;
    //                            string sBegin = "", sEnd = "";
    //                            Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
    //                            MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
    //                        }
    //                        StringBuilder sbTemp = new StringBuilder();

    //                        if ((common.myInt(ViewState["templateid_new"]) == 0)
    //                             || (common.myInt(ViewState["templateid_new"]) == 10007))
    //                        {
    //                            //// replace EnconterId, make it 0 
    //                            bnotes.BindImmunization(HospitalId.ToString(), common.myInt(Session["RegistrationId"]),
    //                                        0, sbTemp, sbTemplateStyle, drTemplateStyle, Page,//common.myInt(Session["EncounterId"]), sbTemp, sbTemplateStyle, drTemplateStyle, Page,
    //                                        common.myStr(dtTemplate.Rows[i]["PageId"]), common.myStr(Session["UserID"]),
    //                                        common.myDate(sDate[0]).ToString(),
    //                                        common.myDate(sDate[1]).ToString(), "");
    //                            if (sbTemp.ToString() != "")
    //                                sb.Append(sbTemp + "<br/>");
    //                        }

    //                        Templinespace = "";
    //                    }
    //                    #endregion
    //                    #region Doctor Progress Note
    //                    if ((common.myInt(ViewState["templateid_new"]) == 1013))
    //                    {
    //                        string strTemplateType = common.myStr(dtTemplate.Rows[i]["PageIdentification"]);
    //                        strTemplateType = strTemplateType.Substring(0, 1);
    //                        sbTemplateStyle = new StringBuilder();
    //                        DataView dv = new DataView(dsTemplateStyle.Tables[0]);
    //                        dv.RowFilter = "PageId=" + common.myInt(dtTemplate.Rows[i]["PageId"]);
    //                        if (dv.Count > 0)
    //                        {
    //                            drTemplateStyle = dv[0].Row;
    //                            string sBegin = "", sEnd = "";
    //                            Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
    //                            MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
    //                        }
    //                        StringBuilder sbTemp = new StringBuilder();

    //                        if ((common.myInt(ViewState["templateid_new"]) == 0)
    //                             || (common.myInt(ViewState["templateid_new"]) == 1013))
    //                        {
    //                            bnotes.BindDoctorProgressNote(HospitalId.ToString(), common.myInt(Session["RegistrationId"]),
    //                                        common.myInt("0"), sbTemp, sbTemplateStyle, drTemplateStyle, Page,//common.myInt(Session["DoctorId"]), sbTemp, sbTemplateStyle, drTemplateStyle, Page,
    //                                        common.myStr(dtTemplate.Rows[i]["PageId"]), common.myStr(Session["UserID"]),
    //                                        common.myDate(sDate[0]).ToString(),
    //                                        common.myDate(sDate[1]).ToString(), "");
    //                            if (sbTemp.ToString() != "")
    //                                sb.Append(sbTemp + "<br/>");
    //                        }

    //                        Templinespace = "";
    //                    }
    //                    #endregion
    //                    #region  Daily Injections
    //                    if ((common.myInt(ViewState["templateid_new"]) == 10008))
    //                    {
    //                        string strTemplateType = common.myStr(dtTemplate.Rows[i]["PageIdentification"]);
    //                        strTemplateType = strTemplateType.Substring(0, 1);
    //                        sbTemplateStyle = new StringBuilder();
    //                        DataView dv = new DataView(dsTemplateStyle.Tables[0]);
    //                        dv.RowFilter = "PageId=" + common.myInt(dtTemplate.Rows[i]["PageId"]);
    //                        if (dv.Count > 0)
    //                        {
    //                            drTemplateStyle = dv[0].Row;
    //                            string sBegin = "", sEnd = "";
    //                            Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
    //                            MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
    //                        }

    //                        StringBuilder sbTemp = new StringBuilder();

    //                        if ((common.myInt(ViewState["templateid_new"]) == 0)
    //                             || (common.myInt(ViewState["templateid_new"]) == 10008))
    //                        {
    //                            bnotes.BindInjection(HospitalId.ToString(), common.myInt(Session["RegistrationId"]),
    //                                        0, sbTemp, sbTemplateStyle, drTemplateStyle, Page,// common.myInt(Session["EncounterId"]), sbTemp, sbTemplateStyle, drTemplateStyle, Page,
    //                                        common.myStr(dtTemplate.Rows[i]["PageId"]), common.myStr(Session["UserID"]),
    //                                        common.myDate(sDate[0]).ToString(),
    //                                        common.myDate(sDate[1]).ToString(), "");
    //                            sb.Append(sbTemp + "<br/>");
    //                        }
    //                        Templinespace = "";
    //                    }
    //                    #endregion
    //                    #region Admission Request
    //                    if (common.myInt(ViewState["templateid_new"]) == 1121)
    //                    {
    //                        string strTemplateType = common.myStr(dtTemplate.Rows[i]["PageIdentification"]);
    //                        strTemplateType = strTemplateType.Substring(0, 1);
    //                        sbTemplateStyle = new StringBuilder();
    //                        DataView dv = new DataView(dsTemplateStyle.Tables[0]);
    //                        dv.RowFilter = "PageId=" + common.myInt(dtTemplate.Rows[i]["PageId"]);
    //                        if (dv.Count > 0)
    //                        {
    //                            drTemplateStyle = dv[0].Row;
    //                            string sBegin = "", sEnd = "";
    //                            Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
    //                            MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
    //                        }
    //                        StringBuilder sbTemp = new StringBuilder();

    //                        if ((common.myInt(ViewState["templateid_new"]) == 0)
    //                             || (common.myInt(ViewState["templateid_new"]) == 1121))
    //                        {
    //                            bnotes.BindPatientBooking(HospitalId, common.myInt(Session["FacilityId"]), common.myInt(Session["RegistrationId"]),
    //                                        common.myInt(Session["EncounterId"]), sbTemp, sbTemplateStyle, drTemplateStyle, Page,
    //                                        common.myStr(dtTemplate.Rows[i]["PageId"]));
    //                            if (sbTemp.ToString() != "")
    //                                sb.Append(sbTemp + "<br/>");
    //                        }

    //                        Templinespace = "";
    //                    }
    //                    #endregion
    //                    #region Referral Request
    //                    if (common.myInt(ViewState["templateid_new"]) == 1124)
    //                    {
    //                        string strTemplateType = common.myStr(dtTemplate.Rows[i]["PageIdentification"]);
    //                        strTemplateType = strTemplateType.Substring(0, 1);
    //                        sbTemplateStyle = new StringBuilder();
    //                        DataView dv = new DataView(dsTemplateStyle.Tables[0]);
    //                        dv.RowFilter = "PageId=" + common.myInt(dtTemplate.Rows[i]["PageId"]);
    //                        if (dv.Count > 0)
    //                        {
    //                            drTemplateStyle = dv[0].Row;
    //                            string sBegin = "", sEnd = "";
    //                            Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
    //                            MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
    //                        }
    //                        StringBuilder sbTemp = new StringBuilder();

    //                        if ((common.myInt(ViewState["templateid_new"]) == 0)
    //                             || (common.myInt(ViewState["templateid_new"]) == 1124))
    //                        {
    //                            bnotes.BindReferalHistory(common.myStr(Session["RegistrationNo"]), HospitalId, common.myInt(EncounterId), common.myInt(Session["UserId"]),
    //                                         sbTemp, sbTemplateStyle, drTemplateStyle, Page,
    //                                        common.myStr(dtTemplate.Rows[i]["PageId"]), sDate[0], sDate[1], "", "");
    //                            if (sbTemp.ToString() != "")
    //                                sb.Append(sbTemp + "<br/>");
    //                        }

    //                        Templinespace = "";
    //                    }
    //                    #endregion
    //                    #region Non Drug Order(Doctor Order)
    //                    if (common.myInt(ViewState["templateid_new"]) == 1166)
    //                    {
    //                        string strTemplateType = common.myStr(dtTemplate.Rows[i]["PageIdentification"]);
    //                        strTemplateType = strTemplateType.Substring(0, 1);
    //                        sbTemplateStyle = new StringBuilder();
    //                        DataView dv = new DataView(dsTemplateStyle.Tables[0]);
    //                        dv.RowFilter = "PageId=" + common.myInt(dtTemplate.Rows[i]["PageId"]);
    //                        if (dv.Count > 0)
    //                        {
    //                            drTemplateStyle = dv[0].Row;
    //                            string sBegin = "", sEnd = "";
    //                            Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
    //                            MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
    //                        }
    //                        StringBuilder sbTemp = new StringBuilder();

    //                        if ((common.myInt(ViewState["templateid_new"]) == 0)
    //                            || (common.myInt(ViewState["templateid_new"]) == 1166))
    //                        {
    //                            bnotes.BindNonDrugOrder(common.myInt(Session["RegistrationId"]), HospitalId, common.myInt(EncounterId), common.myInt(Session["UserId"]),
    //                                         sbTemp, sbTemplateStyle, drTemplateStyle, Page,
    //                                        common.myStr(dtTemplate.Rows[i]["PageId"]), sDate[0], sDate[1], "", "");
    //                            if (sbTemp.ToString() != "")
    //                                sb.Append(sbTemp + "<br/>");
    //                        }

    //                        Templinespace = "";
    //                    }
    //                    #endregion
    //                    #region Diet Order
    //                    if (common.myInt(ViewState["templateid_new"]) == 1172)
    //                    {
    //                        string strTemplateType = common.myStr(dtTemplate.Rows[i]["PageIdentification"]);
    //                        strTemplateType = strTemplateType.Substring(0, 1);
    //                        sbTemplateStyle = new StringBuilder();
    //                        DataView dv = new DataView(dsTemplateStyle.Tables[0]);
    //                        dv.RowFilter = "PageId=" + common.myInt(dtTemplate.Rows[i]["PageId"]);
    //                        if (dv.Count > 0)
    //                        {
    //                            drTemplateStyle = dv[0].Row;
    //                            string sBegin = "", sEnd = "";
    //                            Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
    //                            MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
    //                        }
    //                        StringBuilder sbTemp = new StringBuilder();

    //                        if ((common.myInt(ViewState["templateid_new"]) == 0)
    //                            || (common.myInt(ViewState["templateid_new"]) == 1172))
    //                        {
    //                            bnotes.BindDietOrderInNote(common.myInt(Session["RegistrationId"]), HospitalId, common.myInt(EncounterId),
    //                                         sbTemp, sbTemplateStyle, drTemplateStyle, Page, common.myStr(dtTemplate.Rows[i]["PageId"]), sDate[0], sDate[1], "");
    //                            if (sbTemp.ToString() != "")
    //                                sb.Append(sbTemp + "<br/>");
    //                        }

    //                        Templinespace = "";
    //                    }
    //                    #endregion

    //                }
    //            }
    //        }
    //        return sb.ToString();
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //    finally
    //    {
    //        ObjIcm = null;
    //        dsDis.Dispose();
    //        sEREncounterId = string.Empty;
    //        sEREncounterId = string.Empty;

    //        sbTemplateStyle = null;
    //        ds.Dispose();
    //        dsTemplate.Dispose();
    //        dsTemplateStyle.Dispose();
    //        drTemplateStyle = null;
    //        dtTemplate.Dispose();
    //        hst = null;
    //        Templinespace = string.Empty;
    //        bnotes = null;
    //        fun = null;
    //        sDate = null;
    //    }
    //}

    private string BindStaticTemplates(int StaticTemplateId, int TemplateFieldId)
    {
        string sEREncounterId = "0";
        if (Request.QueryString["EREncounterId"] != null)
        {
            sEREncounterId = Request.QueryString["EREncounterId"].ToString();
        }
        string[] sDate = getFromAndToDate(ddlDateRange.SelectedValue);
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
            string DoctorId = fun.GetDoctorId(HospitalId, UserId);
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
                        bnotes.BindAssessments(RegId, HospitalId, EncounterId, UserId,
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
    //protected string CreateString(DataSet objDs, int iRootId, string iRootName, string TabularType, string sectionId, string EntryType, int RecordId)
    //{
    //    DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
    //    StringBuilder objStr = new StringBuilder();
    //    DataView objDv = null;
    //    DataTable objDt = null;
    //    DataSet dsMain = new DataSet();
    //    StringBuilder objStrTmp = new StringBuilder();
    //    int MaxLength = 0, FieldsLength = 0;
    //    if (objDs != null)
    //    {
    //        if (bool.Parse(TabularType) == true)
    //        {
    //            if (objDs.Tables[0].Rows.Count > 0)
    //            {
    //                DataRow dr = objDs.Tables[0].Rows[0];
    //                DataView dvValues = new DataView(objDs.Tables[1]);
    //                dvValues.RowFilter = "FieldId=" + common.myInt(dr["FieldId"]);
    //                MaxLength = dvValues.ToTable().Rows.Count;
    //                DataTable dtFilter = dvValues.ToTable();

    //                DataView dvFilter = new DataView(dtFilter);
    //                dvFilter.RowFilter = "RowCaption='0'";
    //                DataTable dtNewTable = dvFilter.ToTable();

    //                if (dtNewTable.Rows.Count > 0)
    //                {
    //                    if (MaxLength != 0)
    //                    {
    //                        if (EntryType != "M")
    //                        {
    //                            objStr.Append("<br /><table border='1' style='border-color:#000000; border:solid;  border-collapse:collapse; " + sFontSize + "'  cellpadding='2' cellspacing='3' ><tr align='center'>");
    //                            FieldsLength = objDs.Tables[0].Rows.Count;
    //                            //if (common.myStr(objDs.Tables[0].Rows[0]["TRows"]).Trim() != ""
    //                            //    && common.myStr(objDs.Tables[0].Rows[0]["TRows"]).Trim() != "0")
    //                            //{
    //                            //    objStr.Append("<th align='center' style='border-bottom-color:Black; border-bottom-style:solid; border-bottom-width:1px; " + sFontSize + " ' >&nbsp;</th>");
    //                            //}
    //                            for (int i = 0; i < FieldsLength; i++)   // it makes table
    //                            {
    //                                objStr.Append("<th align='center' style='border-bottom-color:Black; border-bottom-style:solid; border-bottom-width:1px; " + sFontSize + " ' >" + common.myStr(objDs.Tables[0].Rows[i]["FieldName"]) + "</th>");

    //                                dr = objDs.Tables[0].Rows[i];
    //                                dvValues = new DataView(objDs.Tables[1]);
    //                                dvValues.RowFilter = "FieldId=" + common.myInt(dr["FieldId"]);
    //                                dsMain.Tables.Add(dvValues.ToTable("table" + i.ToString()));

    //                                if (dvValues.ToTable().Rows.Count > MaxLength)
    //                                {
    //                                    MaxLength = dvValues.ToTable().Rows.Count;
    //                                }
    //                            }
    //                        }
    //                        objStr.Append("</tr>");
    //                        if (MaxLength == 0)
    //                        {
    //                        }
    //                        else
    //                        {
    //                            if (EntryType != "M")
    //                            {
    //                                if (dsMain.Tables[0].Rows.Count > 0)
    //                                {
    //                                    for (int i = 0; i < MaxLength; i++)
    //                                    {
    //                                        objStr.Append("<tr>");
    //                                        for (int j = 0; j < dsMain.Tables.Count; j++)
    //                                        {
    //                                            if (dsMain.Tables[j].Rows.Count > i
    //                                                && dsMain.Tables[j].Rows.Count > 0)
    //                                            {
    //                                                objStr.Append("<td style=' " + sFontSize + "' align='right'>" + common.myStr(dsMain.Tables[j].Rows[i]["TextValue"]) + "</td>");
    //                                            }
    //                                            else
    //                                            {
    //                                                objStr.Append("<td style=' " + sFontSize + "' align='right'>&nbsp;</td>");
    //                                            }
    //                                        }
    //                                        objStr.Append("</tr>");
    //                                    }
    //                                    objStr.Append("</table>");
    //                                }
    //                            }
    //                            else
    //                            {
    //                                Hashtable hstInput = new Hashtable();
    //                                hstInput.Add("@intTemplateId", iRootId);
    //                                if (common.myInt(Session["Gender"]) == 1)
    //                                {
    //                                    hstInput.Add("chrGenderType", "F");
    //                                }
    //                                else if (common.myInt(Session["Gender"]) == 2)
    //                                {
    //                                    hstInput.Add("chrGenderType", "M");
    //                                }
    //                                else
    //                                {
    //                                    hstInput.Add("chrGenderType", "M");
    //                                }
    //                                hstInput.Add("@intEncounterId", common.myInt(ViewState["EncounterId"]) == 0 ? common.myInt(EncounterId) : common.myInt(ViewState["EncounterId"]));
    //                                hstInput.Add("@intSecId", Convert.ToInt32(sectionId));
    //                                hstInput.Add("@intRecordId", RecordId);
    //                                DataSet dsTabulerTemplate = dl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetTemplateDetailsTabular", hstInput);
    //                                DataView dvRowCaption = new DataView(dsTabulerTemplate.Tables[0]);
    //                                StringBuilder sbCation = new StringBuilder();

    //                                if (dvRowCaption.ToTable().Rows.Count > 0)
    //                                {
    //                                    dvRowCaption.RowFilter = "RowNum>0";
    //                                    DataTable dt = dvRowCaption.ToTable();
    //                                    if (dt.Rows.Count > 0)
    //                                    {
    //                                        sbCation.Append("<br /><table border='1' style='border-color:#000000; border:solid;  border-collapse:collapse; " + sFontSize + "'  cellpadding='2' cellspacing='3' ><tr align='center'>");
    //                                        int column = dt.Columns.Count;
    //                                        int ColumnCount = 0;
    //                                        int count = 1;

    //                                        //Added by rakesh because caption tabular template showing last column missiong start
    //                                        for (int k = 1; k < (column - 5); k++)
    //                                        //Added by rakesh because caption tabular template showing last column missiong start
    //                                        {
    //                                            sbCation.Append("<td>");
    //                                            sbCation.Append(common.myStr(dt.Rows[0]["Col" + count]));
    //                                            sbCation.Append("</td>");
    //                                            count++;
    //                                            ColumnCount++;
    //                                        }
    //                                        sbCation.Append("</tr>");

    //                                        DataView dvRow = new DataView(dt);
    //                                        DataTable dtRow = dvRow.ToTable();
    //                                        for (int l = 1; l <= dtRow.Rows.Count - 3; l++)
    //                                        {
    //                                            sbCation.Append("<tr>");
    //                                            for (int i = 1; i < ColumnCount + 1; i++)
    //                                            {
    //                                                if (dt.Rows[1]["Col" + i].ToString() == "D" || dt.Rows[1]["Col" + i].ToString() == "IM")
    //                                                {
    //                                                    DataView dvDrop = new DataView(dsTabulerTemplate.Tables[1]);
    //                                                    if (dt.Rows[l + 1]["Col" + i].ToString() != "")
    //                                                    {
    //                                                        dvDrop.RowFilter = "ValueId=" + common.myInt(dt.Rows[l + 1]["Col" + i]);
    //                                                        if (dvDrop.ToTable().Rows.Count > 0)
    //                                                        {
    //                                                            if (dt.Rows[1]["Col" + i].ToString() == "D")
    //                                                            {
    //                                                                sbCation.Append("<td style=' " + sFontSize + "' align='center'>" + dvDrop.ToTable().Rows[0]["ValueName"].ToString() + "</td>");
    //                                                            }
    //                                                            else if (dt.Rows[1]["Col" + i].ToString() == "IM")
    //                                                            {
    //                                                                sbCation.Append("<td align='center' ><img  id='dvImageType' runat='server'  alt='Image' width='30px' heigth='30px' src='" + dvDrop.ToTable().Rows[0]["ImagePath"].ToString() + "' /></td>");
    //                                                            }
    //                                                            else
    //                                                            {
    //                                                                sbCation.Append("<td style=' " + sFontSize + "' align='center'>&nbsp;</td>");
    //                                                            }
    //                                                        }
    //                                                        else
    //                                                        {
    //                                                            sbCation.Append("<td style=' " + sFontSize + "' align='center'>&nbsp;</td>");
    //                                                        }
    //                                                    }
    //                                                    else
    //                                                    {
    //                                                        sbCation.Append("<td style=' " + sFontSize + "' align='center'>&nbsp;</td>");
    //                                                    }
    //                                                }
    //                                                else
    //                                                {
    //                                                    if (dt.Rows[l + 1]["Col" + i].ToString() != "")
    //                                                    {
    //                                                        sbCation.Append("<td style=' " + sFontSize + "' align='center'>" + dt.Rows[l + 1]["Col" + i].ToString() + "</td>");
    //                                                    }
    //                                                    else
    //                                                    {
    //                                                        sbCation.Append("<td style=' " + sFontSize + "' align='center'>&nbsp;</td>");
    //                                                    }
    //                                                }
    //                                            }
    //                                            sbCation.Append("</tr>");
    //                                        }
    //                                    }
    //                                    sbCation.Append("</table>");
    //                                }
    //                                objStr.Append(sbCation);
    //                            }

    //                        }
    //                    }
    //                }
    //                else
    //                {
    //                    Hashtable hstInput = new Hashtable();
    //                    hstInput.Add("@intTemplateId", iRootId);

    //                    if (common.myInt(Session["Gender"]) == 1)
    //                    {
    //                        hstInput.Add("chrGenderType", "F");
    //                    }
    //                    else if (common.myInt(Session["Gender"]) == 2)
    //                    {
    //                        hstInput.Add("chrGenderType", "M");
    //                    }
    //                    else
    //                    {
    //                        hstInput.Add("chrGenderType", "M");
    //                    }

    //                    hstInput.Add("@intEncounterId", common.myInt(ViewState["EncounterId"]) == 0 ? common.myInt(EncounterId) : common.myInt(ViewState["EncounterId"]));
    //                    hstInput.Add("@intSecId", Convert.ToInt32(sectionId));
    //                    hstInput.Add("@intRecordId", RecordId);
    //                    DataSet dsTabulerTemplate = dl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetTemplateDetailsTabular", hstInput);

    //                    DataView dvRowCaption = new DataView(dsTabulerTemplate.Tables[0]);

    //                    dvRowCaption.RowFilter = "RowCaptionId>0";
    //                    if (dvRowCaption.ToTable().Rows.Count > 0)
    //                    {
    //                        StringBuilder sbCation = new StringBuilder();
    //                        dvRowCaption.RowFilter = "RowNum>0";
    //                        DataTable dt = dvRowCaption.ToTable();
    //                        if (dt.Rows.Count > 0)
    //                        {
    //                            sbCation.Append("<br /><table border='1' style='border-color:#000000; border:solid;  border-collapse:collapse; " + sFontSize + "'  cellpadding='2' cellspacing='3' ><tr align='center'>");
    //                            int column = dt.Columns.Count;
    //                            int ColumnCount = 0;
    //                            int count = 1;
    //                            for (int k = 1; k < (column - 4); k++)
    //                            {
    //                                if (common.myStr(dt.Rows[0]["RowCaptionName"]) == ""
    //                                    && ColumnCount == 0)
    //                                {
    //                                    sbCation.Append("<td>");
    //                                    sbCation.Append(" + ");
    //                                    sbCation.Append("</td>");
    //                                }
    //                                else
    //                                {
    //                                    sbCation.Append("<td>");
    //                                    sbCation.Append(common.myStr(dt.Rows[0]["Col" + count]));
    //                                    sbCation.Append("</td>");
    //                                    count++;
    //                                }
    //                                ColumnCount++;
    //                            }
    //                            sbCation.Append("</tr>");

    //                            DataView dvRow = new DataView(dt);
    //                            dvRow.RowFilter = "RowCaptionId>0";
    //                            DataTable dtRow = dvRow.ToTable();
    //                            for (int l = 1; l <= dtRow.Rows.Count; l++)
    //                            {
    //                                sbCation.Append("<tr>");
    //                                for (int i = 0; i < ColumnCount; i++)
    //                                {
    //                                    if (i == 0)
    //                                    {
    //                                        sbCation.Append("<td style=' " + sFontSize + "' align='center'>" + common.myStr(dt.Rows[l + 1]["RowCaptionName"]) + "</td>");
    //                                    }
    //                                    else
    //                                    {
    //                                        if (dt.Rows[1]["Col" + i].ToString() == "D" || dt.Rows[1]["Col" + i].ToString() == "IM")
    //                                        {
    //                                            DataView dvDrop = new DataView(dsTabulerTemplate.Tables[1]);
    //                                            if (dt.Rows[l + 1]["Col" + i].ToString() != "")
    //                                            {
    //                                                dvDrop.RowFilter = "ValueId=" + common.myInt(dt.Rows[l + 1]["Col" + i]);
    //                                                if (dvDrop.ToTable().Rows.Count > 0)
    //                                                {
    //                                                    if (dt.Rows[1]["Col" + i].ToString() == "D")
    //                                                    {
    //                                                        sbCation.Append("<td style=' " + sFontSize + "' align='center'>" + dvDrop.ToTable().Rows[0]["ValueName"].ToString() + "</td>");
    //                                                    }
    //                                                    else if (dt.Rows[1]["Col" + i].ToString() == "IM")
    //                                                    {
    //                                                        sbCation.Append("<td align='center' ><img  id='dvImageType' runat='server'  alt='Image' width='30px' heigth='30px' src='" + dvDrop.ToTable().Rows[0]["ImagePath"].ToString() + "' /></td>");
    //                                                    }
    //                                                    else
    //                                                    {
    //                                                        sbCation.Append("<td style=' " + sFontSize + "' align='center'>&nbsp;</td>");
    //                                                    }
    //                                                }
    //                                                else
    //                                                {
    //                                                    sbCation.Append("<td style=' " + sFontSize + "' align='center'>&nbsp;</td>");
    //                                                }
    //                                            }
    //                                            else
    //                                            {
    //                                                sbCation.Append("<td style=' " + sFontSize + "' align='center'>&nbsp;</td>");
    //                                            }
    //                                        }
    //                                        else
    //                                        {
    //                                            if (dt.Rows[l + 1]["Col" + i].ToString() != "")
    //                                            {
    //                                                sbCation.Append("<td style=' " + sFontSize + "' align='center'>" + dt.Rows[l + 1]["Col" + i].ToString() + "</td>");
    //                                            }
    //                                            else
    //                                            {
    //                                                sbCation.Append("<td style=' " + sFontSize + "' align='center'>&nbsp;</td>");
    //                                            }
    //                                        }
    //                                    }

    //                                }
    //                                sbCation.Append("</tr>");
    //                            }
    //                            sbCation.Append("</table>");
    //                        }
    //                        objStr.Append(sbCation);
    //                    }

    //                }
    //            }
    //        }
    //        else // For Non Tabular Templates
    //        {
    //            string BeginList = "", EndList = "";
    //            string sBegin = "", sEnd = "";
    //            int t = 0;
    //            string FieldId = "";
    //            string sStaticTemplate = "";
    //            string sEnterBy = "";
    //            string sVisitDate = "";
    //            foreach (DataRow item in objDs.Tables[0].Rows)
    //            {
    //                objDv = new DataView(objDs.Tables[1]);
    //                objDv.RowFilter = "FieldId=" + common.myInt(item["FieldId"]);
    //                objDt = objDv.ToTable();
    //                if (t == 0)
    //                {
    //                    t = 1;
    //                    if (common.myStr(item["FieldsListStyle"]) == "1")
    //                    {
    //                        BeginList = "<ul>"; EndList = "</ul>";
    //                    }
    //                    else if (item["FieldsListStyle"].ToString() == "2")
    //                    {
    //                        BeginList = "<ol>"; EndList = "</ol>";
    //                    }
    //                }
    //                if (common.myStr(item["FieldsBold"]) != ""
    //                    || common.myStr(item["FieldsItalic"]) != ""
    //                    || common.myStr(item["FieldsUnderline"]) != ""
    //                    || common.myStr(item["FieldsFontSize"]) != ""
    //                    || common.myStr(item["FieldsForecolor"]) != ""
    //                    || common.myStr(item["FieldsListStyle"]) != "")
    //                {
    //                    if (objDt.Rows.Count > 0)
    //                    {
    //                        sEnd = "";
    //                        MakeFont("Fields", ref sBegin, ref sEnd, item);
    //                        if (Convert.ToBoolean(item["DisplayTitle"]))
    //                        {
    //                            if (sBegin.Contains("<br/>") == true)
    //                            {
    //                                sBegin = sBegin.Remove(0, 5);

    //                                objStr.Append(BeginList + sBegin + common.myStr(item["FieldName"]));
    //                            }
    //                            if (objStr.ToString() != "")
    //                            {
    //                                objStr.Append(sEnd + "</li>");
    //                            }
    //                        }
    //                        BeginList = "";
    //                        //////sBegin = "";
    //                    }
    //                }
    //                else
    //                {
    //                    if (objDt.Rows.Count > 0)
    //                    {
    //                        if (sStaticTemplate != "<br/><br/>")
    //                        {
    //                            objStr.Append(common.myStr(item["FieldName"]));
    //                        }
    //                    }
    //                }
    //                if (objDs.Tables.Count > 1)
    //                {
    //                    objDv = new DataView(objDs.Tables[1]);
    //                    objDv.RowFilter = "FieldId=" + common.myInt(item["FieldId"]);
    //                    objDt = objDv.ToTable();
    //                    DataView dvFieldType = new DataView(objDs.Tables[0]);
    //                    dvFieldType.RowFilter = "FieldId=" + common.myInt(item["FieldId"]);
    //                    DataTable dtFieldType = dvFieldType.ToTable("FieldType");

    //                    sBegin = "";
    //                    sEnd = "";

    //                    MakeFontWithoutBR("Fields", ref sBegin, ref sEnd, item);


    //                    for (int i = 0; i < objDv.ToTable().Rows.Count; i++)
    //                    {
    //                        if (objDt.Rows.Count > 0)
    //                        {
    //                            string FType = common.myStr(dtFieldType.Rows[0]["FieldType"]);
    //                            if (FType == "C")
    //                            {
    //                                FType = "C";
    //                            }
    //                            if (FType == "C" || FType == "D" || FType == "B" || FType == "R")
    //                            {
    //                                if (FType == "B")
    //                                {
    //                                    objStr.Append(" " + objDt.Rows[i]["TextValue"]);
    //                                }
    //                                else
    //                                {
    //                                    //////    BindDataValue(objDs, objDt, objStr, i, FType);

    //                                    BindDataValue(objDs, objDt, objStr, i, FType, sBegin, sEnd);

    //                                    // // BindDataValue(objDs, objDt, objStr, i, FType, sBegin, sEnd);
    //                                }
    //                            }
    //                            else if (FType == "T" || FType == "M" || FType == "S" || FType == "ST" || FType == "SB" || FType == "W")
    //                            {
    //                                if (common.myStr(ViewState["iTemplateId"]) != "163")
    //                                {
    //                                    //////if (i == 0)
    //                                    //////{
    //                                    //////    objStr.Append(" " + common.myStr(objDt.Rows[i]["TextValue"]));
    //                                    //////}
    //                                    //////else
    //                                    //////{
    //                                    //////    objStr.Append(", " + common.myStr(objDt.Rows[i]["TextValue"]));
    //                                    //////}



    //                                    if (i == 0)
    //                                    {
    //                                        if (FType == "M" || FType == "W")
    //                                        {
    //                                            ////// objStr.Append(" " + common.myStr(objDt.Rows[i]["TextValue"]));
    //                                            objStr.Append(sBegin + " " + common.myStr(objDt.Rows[i]["TextValue"]) + sEnd);
    //                                        }
    //                                        else
    //                                        {
    //                                            //////   objStr.Append(", " + common.myStr(objDt.Rows[i]["TextValue"]));
    //                                            objStr.Append(sBegin + ", " + common.myStr(objDt.Rows[i]["TextValue"]) + sEnd);
    //                                        }


    //                                    }
    //                                    else
    //                                    {
    //                                        objStr.Append(sBegin + ", " + common.myStr(objDt.Rows[i]["TextValue"]) + sEnd);
    //                                        //if (FType == "M" || FType == "W")
    //                                        //{
    //                                        //    objStr.Append(sBegin + ", " + common.myStr(objDt.Rows[i]["TextValue"]) + sEnd);
    //                                        //}
    //                                        //else
    //                                        //{
    //                                        //    objStr.Append(sBegin + ", " + common.myStr(objDt.Rows[i]["TextValue"]) + sEnd);

    //                                        //}

    //                                    }
    //                                }
    //                                else
    //                                {
    //                                    if (i == 0)
    //                                    {
    //                                        objStr.Append(": " + common.myStr(objDt.Rows[i]["TextValue"]));
    //                                    }
    //                                    else
    //                                    {
    //                                        objStr.Append(", " + common.myStr(objDt.Rows[i]["TextValue"]));
    //                                    }
    //                                }
    //                            }
    //                            else if (FType == "L")
    //                            {
    //                                objStr.Append(BindStaticTemplates(common.myInt(objDt.Rows[0]["StaticTemplateId"]), common.myInt(objDt.Rows[0]["FieldId"])));
    //                            }
    //                            else if (FType == "IM")
    //                            {
    //                                try
    //                                {
    //                                    if (objDt.Rows.Count > 0)
    //                                    {
    //                                        DataView dvImg = new DataView(objDs.Tables[2]);
    //                                        dvImg.RowFilter = "FieldId=" + common.myInt(item["FieldId"]) + " AND ValueId=" + common.myInt(objDt.Rows[0]["FieldValue"]);

    //                                        objStr.Append(BindNonTabularImageTypeFieldValueTemplates(dvImg.ToTable()));
    //                                    }
    //                                }
    //                                catch
    //                                {
    //                                }
    //                            }
    //                            if (common.myStr(item["FieldsListStyle"]) == "")
    //                            {
    //                                if (ViewState["iTemplateId"].ToString() != "163")
    //                                {
    //                                    if (FType != "C")
    //                                    {
    //                                        objStr.Append("<br />");
    //                                    }

    //                                }
    //                                else
    //                                {
    //                                    if (FType != "C" && FType != "T")
    //                                    {
    //                                        objStr.Append("<br />");
    //                                    }
    //                                }
    //                            }
    //                        }
    //                        sEnterBy = objDt.Rows[i]["EnterBy"].ToString();
    //                        sVisitDate = objDt.Rows[i]["VisitDateTime"].ToString();
    //                        //if (EntryType == "M" && sEnterBy != "" && sVisitDate != "")
    //                        //{
    //                        //    objStr.Append("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style='font-size:10px'>(Entered By: " + sEnterBy + " Date/Time: " + sVisitDate + ")</span>");
    //                        //}
    //                    }
    //                    sBegin = "";
    //                    sEnd = "";

    //                    // Cmt 25/08/2011
    //                    //if (objDt.Rows.Count > 0)
    //                    //{
    //                    //    if (objStr.ToString() != "")
    //                    //        objStr.Append(sEnd + "</li>");
    //                    //}
    //                }

    //                //objStr.Append(" <span style=\" font-weight:bold; font-size:medium; color:Gray\">|</span> ");
    //            }

    //            if (objStr.ToString() != "")
    //            {
    //                objStr.Append(EndList);
    //            }
    //        }
    //        //////if (objStr.ToString() != "")
    //        //////{
    //        //////    DataView dvValues = new DataView(objDs.Tables[1]);
    //        //////    dvValues.RowFilter = "SectionId=" + common.myInt(sectionId);
    //        //////    if (dvValues.ToTable().Rows.Count > 0)
    //        //////    {
    //        //////       objStr.Append("<br/>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style='font-family: Tahoma;'>Entered By: " + common.myStr(dvValues.ToTable().Rows[0]["EnterBy"]) + " Date/Time: " + common.myStr(dvValues.ToTable().Rows[0]["VisitDateTime"]) + "</span><br/>");
    //        //////    }
    //        //////}
    //    }

    //    return objStr.ToString();
    //}


    protected string CreateString(DataSet objDs, int iRootId, string iRootName, string TabularType,
   string sectionId, string EntryType, int RecordId, string GroupingDate = "", bool IsConfidential = false)
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
                                    if (Convert.ToBoolean(item["DisplayTitle"]))
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

                                        objStr.Append(BeginList + sBegin + common.myStr(item["FieldName"]) + sEnd);
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
                                sBegin = "";
                                sEnd = "";

                                string sbeginTemp = string.Empty;
                                MakeFontWithoutBR("Fields", ref sBegin, ref sEnd, item);
                                // MakeFont("Fields", ref sBegin, ref sEnd, item);
                                sBegin = sBegin.Replace("font-weight: bold;", string.Empty);

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

                                                    //if (FType == "M" || FType == "W")
                                                    //{

                                                    //    objStr.Append(sBegin + " <br /> " + common.myStr(objDt.Rows[i]["TextValue"]) + sEnd);
                                                    //}
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
                                            objStr.Append(BindStaticTemplates(common.myInt(objDt.Rows[0]["StaticTemplateId"]), common.myInt(objDt.Rows[0]["FieldId"])));
                                        }
                                        else if (FType == "O")
                                        {
                                            objStr.Append(sBegin + " : " + common.myStr(objDt.Rows[i]["TextValue"]) + sEnd);
                                        }
                                        else if (FType == "IM")
                                        {
                                            try
                                            {
                                                if (objDt.Rows.Count > 0)
                                                {
                                                    DataView dvImg = new DataView(objDs.Tables[2]);
                                                    dvImg.RowFilter = "FieldId=" + common.myInt(item["FieldId"]) + " AND ValueId=" + common.myInt(objDt.Rows[0]["FieldValue"]);

                                                    objStr.Append(BindNonTabularImageTypeFieldValueTemplates(dvImg.ToTable()));
                                                }
                                            }
                                            catch
                                            {
                                            }
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
                string EnterByDate = string.Empty;
                if ((sDisplayEnteredBy == "Y") || (sDisplayEnteredBy == "N" && common.myStr(Session["OPIP"]) == "I"))
                {
                    int EnterById = 0;

                    if ((objStr.ToString() != "" || IsConfidential == true) && bool.Parse(TabularType) == false)
                    {
                        DataView dvValues = new DataView(objDs.Tables[1]);
                        dvValues.RowFilter = "SectionId=" + common.myStr(sectionId);
                        if (dvValues.ToTable().Rows.Count > 0)
                        {
                            EnterById = common.myInt(dvValues.ToTable().Rows[0]["EnteredById"]);
                            objStr.Append("<b><br/>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style='font-family: Tahoma; font-size:8pt;'>Entered By: " + common.myStr(dvValues.ToTable().Rows[0]["EnterBy"]) + "</span></b>");
                            EnterByDate = " on " + common.myStr(dvValues.ToTable().Rows[0]["VisitDateTime"]);
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
                                objStr.Append("<b><br/>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style='font-family: Tahoma; font-size:8pt;'>Entered By: " + common.myStr(dvValues.ToTable().Rows[0]["EnterBy"]) + "</span></b>");
                                EnterByDate = " on " + common.myStr(dvValues.ToTable().Rows[0]["VisitDateTime"]);
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
                    objStr.Append("<b> <span style='font-family: " + common.myStr(System.Web.HttpContext.Current.Session["CurrentTemplateFontName"]) + "; font-size:8pt;'> " + EnterByDate + "</span></b>");

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
                sb.Append("<table id='dvImageType' runat='server'><tr><td>" + common.myStr(dtIMTypeTemplate.Rows[0]["ValueName"]) + "</td></tr><tr align='left'><td align='center'><img src='" + common.myStr(dtIMTypeTemplate.Rows[0]["ImagePath"]) + "' width='80px' height='80px' border='0' align='left' alt='Image' /></td></tr></table>");
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
        // Sikandar General Exmination  Section id 22741

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
        char[] chr = { '|' };
        string[] FromDate = tvCategory.SelectedNode.Value.Split(chr);
        if (FromDate.LongLength == 6)
        {
            hstInput.Add("@chrFromDate", Convert.ToDateTime(FromDate[4]).ToString("yyyy/MM/dd"));//sDate[0].SelectedDate.Value.ToString("yyyy/MM/dd"));
        }
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

                if (dsAllSectionDetails.Tables.Count > 2)
                {
                    dsAllFieldsDetails.Tables.Add(dsAllSectionDetails.Tables[1].Copy());
                }
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
                                if (iRecordId != common.myInt(dtEntry.Rows[it]["RecordId"]))
                                {
                                    if (sEntryType == "M")
                                    {
                                        //////objStrTmp.Append("<br/><br/><b>" + common.myStr(dtFieldValue.Rows[0]["VisitDateTime"]) + "</b>");
                                        objStrTmp.Append("<b>" + common.myStr(dtFieldValue.Rows[0]["VisitDateTime"]) + "</b>");
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
                                        objStrTmp.Append("<br />");
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
                                        //////objStrTmp.Append("<br/><br/><b> " + common.myStr(dtFieldValue.Rows[0]["VisitDateTime"]) + "</b>");
                                        objStrTmp.Append("<b> " + common.myStr(dtFieldValue.Rows[0]["VisitDateTime"]) + "</b><br/>");
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
                                        //////objStrTmp.Append("<br/><br/><b> " + common.myStr(dtFieldValue.Rows[0]["VisitDateTime"]) + "</b>");
                                        objStrTmp.Append("<br/><b> " + common.myStr(dtFieldValue.Rows[0]["VisitDateTime"]) + "</b>");
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


                                            objStrTmp.Append(BeginList2 + sBegin + common.myStr(item["SectionName"]) + sEnd + "<br/>");
                                            //////objStrTmp.Append(BeginList2 + sBegin + common.myStr(item["SectionName"]) + sEnd);


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
                                            //////objStrTmp.Append(common.myStr(item["SectionName"])); //Comment On 19June2010

                                            objStrTmp.Append(common.myStr(item["SectionName"]));
                                            objStrTmp.Append("<br/>");




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
                            iRecordId = common.myInt(dtEntry.Rows[it]["RecordId"]);
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
    protected void imgPharmacy_Onclick(object sender, EventArgs e)
    {
        RTF1.Content = bindPharmacyData(0);
    }
    protected void imgVital_Onclick(object sender, EventArgs e)
    {
        ViewState["templateid_new"] = null;
        BindEditorForVital();
    }
    protected void imgDiagnosis_Onclick(object sender, EventArgs e)
    {
        bindDiagnosis();
    }
    protected void btnRefresh_OnClick(object sender, EventArgs e)
    {
        RTF1.Content = "";
        BindCategoryTree();
    }

    private string bindPharmacyData(int EncounterId)
    {
        string[] sDate = getFromAndToDate(common.myStr(ddlDateRange.SelectedValue));
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
    private void BindEditorForVital()
    {
        string sEREncounterId = "0";
        if (Request.QueryString["EREncounterId"] != null)
        {
            sEREncounterId = Request.QueryString["EREncounterId"].ToString();
        }
        StringBuilder sb = new StringBuilder();
        StringBuilder sbTemplateStyle = new StringBuilder();
        DataSet ds = new DataSet();
        DataSet dsTemplate = new DataSet();
        DataSet dsTemplateStyle = new DataSet();
        DataRow drTemplateStyle = null;
        DataTable dtTemplate = new DataTable();
        Hashtable hst = new Hashtable();
        string Templinespace = "";
        string Fonts = string.Empty;

        RegId = common.myInt(Session["RegistrationID"]);
        HospitalId = common.myInt(Session["HospitalLocationID"]);
        EncounterId = common.myInt(ViewState["EncounterId"]) == 0 ? common.myInt(EncounterId) : common.myInt(ViewState["EncounterId"]);
        UserId = common.myInt(Session["UserID"]);
        BindNotes bnotes = new BindNotes(sConString);
        BaseC.DiagnosisDA fun = new BaseC.DiagnosisDA(sConString);
        string[] sDate = getFromAndToDate(ddlDateRange.SelectedValue);
        try
        {
            string DoctorId = fun.GetDoctorId(HospitalId, UserId);
            dsTemplateStyle = bnotes.GetTemplateStyle(common.myInt(Session["HospitalLocationId"]));
            dsTemplate = bnotes.GetEMRTemplates(common.myInt(ViewState["RequestTemplateId"]) == 0 ? EncounterId : 0, RegId, sEREncounterId, common.myInt(ViewState["RequestTemplateId"]));
            dtTemplate = dsTemplate.Tables[0];
            if (common.myInt(ViewState["RequestTemplateId"]) != 0)
            {
                ViewState["templateid_new"] = ViewState["RequestTemplateId"];
            }
            ViewState["RequestTemplateId"] = null;
            sb.Append("<span style='" + Fonts + "'>");
            int i = 0;
            if (true)
            {
                if (dtTemplate.Rows.Count > i)
                {
                    #region vital
                    if ((common.myInt(ViewState["templateid_new"]) == 0)
                             || (common.myInt(ViewState["templateid_new"]) == 10001))
                    {
                        string strTemplateType = common.myStr(dtTemplate.Rows[i]["PageIdentification"]);
                        strTemplateType = strTemplateType.Substring(0, 1);
                        sbTemplateStyle = new StringBuilder();
                        DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                        dv.RowFilter = "PageId=" + common.myInt(dtTemplate.Rows[i]["PageId"]);
                        if (dv.Count > 0)
                        {
                            drTemplateStyle = dv[0].Row;
                            string sBegin = "", sEnd = "";
                            Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                            MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                        }
                        StringBuilder sbTemp = new StringBuilder();
                        if ((common.myInt(ViewState["templateid_new"]) == 0)
                             || (common.myInt(ViewState["templateid_new"]) == 10001))
                        {
                            bnotes.BindVitals(HospitalId.ToString(), 0, sbTemp, sbTemplateStyle, drTemplateStyle,
                                                Page, common.myStr("0"), common.myStr(Session["UserID"]),//Page, common.myStr(dtTemplate.Rows[i]["PageId"]), common.myStr(Session["UserID"]),
                                                sDate[0], sDate[1], 0, sEREncounterId, "");
                            if (sbTemp.ToString() != "")
                                sb.Append(sbTemp + "<br/>" + "<br/>");
                        }
                        drTemplateStyle = null;
                        Templinespace = "";
                    }
                    #endregion
                    sb.Append("</span>");
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
            sbTemplateStyle = null;
            ds.Dispose();
            dsTemplate.Dispose();
            dsTemplateStyle.Dispose();
            drTemplateStyle = null;
            dtTemplate.Dispose();
            hst = null;
            Templinespace = string.Empty;
            RegId = 0;
            HospitalId = 0;
            EncounterId = 0;
            UserId = 0;
            bnotes = null;
            fun = null;
        }
        RTF1.Content = sb.ToString();
    }
    private void bindDiagnosis()
    {
        string[] sDate = getFromAndToDate(common.myStr(ddlDateRange.SelectedValue));
        BindNotes note = new BindNotes(sConString);
        try
        {
            RTF1.Content = note.BindVisitNotesDiagnosis(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]),
                common.myInt(Session["RegistrationId"]), sDate[0], sDate[1]);
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
    }

    private void BindCategoryTree()
    {
        DataSet ds = new DataSet();
        BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
        string[] sDateRange = getFromAndToDate(common.myStr(ddlDateRange.SelectedValue));
        try
        {
            tvCategory.Nodes.Clear();
            ds = objEMR.getEMRPatientVisits(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), common.myInt(Session["RegistrationId"]),
                                            common.myStr(rdoViewType.SelectedValue), common.myInt(ddlDoctor.SelectedValue),
                                            common.myInt(ddlTemplate.SelectedValue), sDateRange[0], sDateRange[1]);
            if (ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataRow dr = ds.Tables[0].Rows[i];
                    AddNodes(tvCategory, common.myStr(dr["EncounterId"]), common.myInt(dr["ParentId"]), common.myStr(dr["VisitDetail"]),
                          common.myInt(dr["TemplateId"]), common.myStr(dr["VisitType"]), common.myStr(dr["DoctorName"]), common.myDate(dr["VisitDate"]),
                          common.myInt(dr["GroupId"]), common.myBool(dr["IsLegacyData"]));
                    dr = null;
                }
                if (tvCategory.Nodes.Count > 0)
                {
                    if (ViewState["SelectedCategory"] != null)
                    {
                        TreeNode node1 = SelectNode(ViewState["SelectedCategory"].ToString(), tvCategory.Nodes);
                        if (node1 != null)
                        {
                            node1.Selected = true;
                        }
                        node1 = null;
                    }
                    else
                    {
                        tvCategory.PopulateNodesFromClient = true;
                        tvCategory.ShowLines = true;
                    }
                }
                ViewState["GroupTable"] = ds.Tables[1];
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
            objEMR = null;
            sDateRange = null;
        }
    }
    TreeNode node { get; set; }
    private TreeNode SelectNode(string nodetext, TreeNodeCollection parentCollection)
    {
        foreach (TreeNode childnode in parentCollection)
        {
            if (childnode.Text == nodetext)
            {
                node = childnode;
            }
            if ((node != null)) break;
        }
        return node;
    }

    private void AddNodes(TreeView tvName, string iNodeID, int iParentID, string sNodeText, int TemplateId,
        string VisitType, string DoctorName, DateTime VisitDate, int GrouId, bool IsLegacyData)
    {
        try
        {
            if (iParentID == 0)
            {
                TreeNode masternode;
                if (common.myStr(rdoViewType.SelectedItem.Value) == "V")
                {
                    masternode = new TreeNode(sNodeText.ToString(), "P" + VisitType.ToString() + iNodeID.ToString());
                    masternode.ToolTip = "P" + iNodeID.ToString() + "|" + TemplateId.ToString() + "|" + VisitType.ToString() + "|" + DoctorName.ToString() + "|" + VisitDate.ToString() + "|" + GrouId.ToString() + "|" + IsLegacyData;
                }
                else
                {
                    masternode = new TreeNode(sNodeText.ToString(), "P" + iNodeID.ToString());
                    masternode.ToolTip = "P" + iNodeID.ToString() + "|" + TemplateId.ToString() + "|" + VisitType.ToString() + "|" + DoctorName.ToString() + "|" + VisitDate.ToString() + "|" + "|" + GrouId.ToString() + "|" + IsLegacyData;
                }

                tvName.Nodes.Add(masternode);
                tvName.CollapseAll();
            }
            else
            {
                TreeNode masternode = new TreeNode();
                if (common.myStr(rdoViewType.SelectedItem.Value) == "V")
                {
                    masternode = tvName.FindNode("P" + VisitType.ToString() + iParentID.ToString());
                    // masternode.ToolTip = "P" + iNodeID.ToString() + "|" + TemplateId.ToString() + "|" + VisitType.ToString() + "|" + DoctorName.ToString() + "|" + VisitDate.ToString() + "|" + "|" + GrouId.ToString();
                    //if (("P" + iNodeID.ToString() + "|" + TemplateId.ToString() + "|" + VisitType.ToString() + "|" + DoctorName.ToString() + "|" + VisitDate.ToString() + "|" + "|" + GrouId.ToString()) != null)

                    //{
                    //    masternode.ToolTip = "P" + iNodeID.ToString() + "|" + TemplateId.ToString() + "|" + VisitType.ToString() + "|" + DoctorName.ToString() + "|" + VisitDate.ToString() + "|" + "|" + GrouId.ToString();
                    //}



                }
                else
                {
                    masternode = tvName.FindNode("P" + iParentID.ToString());
                    //masternode.ToolTip = "P" + iNodeID.ToString() + "|" + TemplateId.ToString() + "|" + VisitType.ToString() + "|" + DoctorName.ToString() + "|" + VisitDate.ToString() + "|" + GrouId.ToString();

                    //if (("P" + iNodeID.ToString() + "|" + TemplateId.ToString() + "|" + VisitType.ToString() + "|" + DoctorName.ToString() + "|" + VisitDate.ToString() + "|" + GrouId.ToString()) != null)

                    //{
                    //    masternode.ToolTip = "P" + iNodeID.ToString() + "|" + TemplateId.ToString() + "|" + VisitType.ToString() + "|" + DoctorName.ToString() + "|" + VisitDate.ToString() + "|" + GrouId.ToString();
                    //}

                }

                if (masternode != null)
                {
                    TreeNode childNode = new TreeNode(sNodeText.ToString(), "C" + iNodeID.ToString() + "|" + TemplateId.ToString() + "|" + VisitType.ToString() + "|" + DoctorName.ToString() + "|" + VisitDate.ToString() + "|" + GrouId.ToString() + "|" + IsLegacyData);
                    masternode.ChildNodes.Add(childNode);
                }
                else
                {
                    CallRecursive(tvName, iNodeID, "C" + iParentID.ToString() + "|" + TemplateId.ToString() + "|" + VisitType.ToString() + "|" + DoctorName.ToString() + "|" + VisitDate.ToString() + "|" + GrouId.ToString() + "|" + IsLegacyData, sNodeText);
                }
                tvName.CollapseAll();
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    private void CallRecursive(TreeView tvName, String iNodeID, String sParentID, String sNodeText)
    {
        TreeNodeCollection nodes = tvName.Nodes;
        foreach (TreeNode n in nodes)
        {
            ReCallRecursive(n, iNodeID, sParentID, sNodeText);
        }
    }

    private void ReCallRecursive(TreeNode treeNode, String iNodeID, String sParentID, String sNodeText)
    {
        foreach (TreeNode tn in treeNode.ChildNodes)
        {
            if (common.myStr(tn.Value) == common.myStr(sParentID))
            {
                TreeNode childNode = new TreeNode(sNodeText.ToString(), "C" + iNodeID.ToString());
                tn.ChildNodes.Add(childNode);
            }
            ReCallRecursive(tn, iNodeID, sParentID, sNodeText);
        }
    }
    protected void tvCategory_SelectedNodeChanged(object sender, EventArgs e)
    {
        try
        {
            if (tvCategory.SelectedNode != null)
            {
                // setSelectedNode(tvCategory.SelectedNode.Value, tvCategory.SelectedNode.ToolTip);
                ViewState["tvCategoryText"] = tvCategory.SelectedNode.Text;
                ViewState["tvCategoryValue"] = tvCategory.SelectedNode.Value;

                setSelectedNode(tvCategory.SelectedNode.Value, tvCategory.SelectedNode.ToolTip);

                string[] nodeid = tvCategory.SelectedNode.Value.Split('|');
                //bindDetails();
                if (nodeid.Length > 1)
                {
                    if (common.myInt(nodeid[1]).Equals(common.myInt(ViewState["EMRPoliceIntimationTemplateId"])))
                    {
                        lnkSendToPoliceStation.Visible = true;
                        ViewState["SelectedCheckedTemplateIdForObjectDetails"] = common.myInt(nodeid[1]);
                    }
                }
                if (common.myBool(ViewState["IsLegacyData"]))
                {
                    clsIVF obj = new clsIVF(sConString);
                    RTF1.Content = obj.getEncounterLegacyData(common.myInt(ViewState["EncounterId"]));

                    obj = null;
                }
                else
                {
                    RTF1.Content = BindEditor(true, true);
                }
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private void BindProvider()
    {
        DataSet ds = new DataSet();
        BaseC.Hospital emr = new BaseC.Hospital(sConString);
        try
        {
            ds = emr.fillDoctorCombo(common.myInt(Session["HospitalLocationId"]), 0, common.myInt(Session["FacilityId"]));
            ddlDoctor.DataSource = ds.Tables[0];
            ddlDoctor.DataTextField = "DoctorName";
            ddlDoctor.DataValueField = "DoctorID";
            ddlDoctor.DataBind();
            ddlDoctor.Items.Insert(0, new RadComboBoxItem("Select All", "0"));


            //ddlDoctor.SelectedValue = common.myStr(Session["DoctorId"]);
            ddlDoctor.SelectedIndex = 0;
            btnRefresh_OnClick(null, null);


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
            emr = null;
        }
    }
    protected void ddlDateRange_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlDateRange.SelectedValue == "DR")
        {
            tblDateRange.Visible = true;
        }
        else
        {
            tblDateRange.Visible = false;
        }
    }
    public void setSelectedNode(string Node, string ToolTip)
    {
        ViewState["EncounterId"] = "0";
        ViewState["templateid_new"] = "0";
        ViewState["IsLegacyData"] = "0";
        lblVisitType.Text = string.Empty;
        lblDoctorName.Text = string.Empty;
        lblVisitDate.Text = string.Empty;
        int length = 0;
        string nodeType = string.Empty;
        string nodeValues = string.Empty;
        int idx1 = 1;
        int idx = 1;
        string[] valColl1;
        string[] valColl;
        try
        {
            if (Node.Length > 0)
            {
                length = Node.Length;
                nodeType = Node.Substring(0, 1);
                nodeValues = Node.Substring(1, length - 1);
                if (nodeType == "P")
                {
                    length = nodeValues.Length;
                    if (common.myStr(rdoViewType.SelectedItem.Value) == "V")
                    {
                        lblVisitType.Text = nodeValues.Substring(0, 2);
                        ViewState["EncounterId"] = nodeValues.Substring(2, length - 2);
                    }
                    else
                    {
                        ViewState["EncounterId"] = nodeValues;
                        ViewState["templateid_new"] = common.myInt(nodeValues);
                    }

                    ViewState["templateid_new"] = "0";
                    if (common.myStr(rdoViewType.SelectedItem.Value) == "V")
                    {
                        valColl1 = ToolTip.Split('|');
                        foreach (string val1 in valColl1)
                        {
                            switch (idx1)
                            {
                                case 4:
                                    lblDoctorName.Text = val1;
                                    break;
                                case 5:
                                    DateTime Vdt = Convert.ToDateTime(val1);
                                    if (common.myStr(Vdt) != "")
                                    {
                                        lblVisitDate.Text = common.myStr(Vdt.ToString("dd/MM/yyyy"));

                                    }
                                    break;
                                case 6:
                                    ViewState["GroupId"] = val1;
                                    break;
                                case 7:
                                    ViewState["IsLegacyData"] = val1;
                                    break;
                            }
                            idx1++;
                        }
                    }
                }
                if (nodeType == "C")
                {
                    valColl = nodeValues.Split('|');
                    foreach (string val in valColl)
                    {
                        switch (idx)
                        {
                            case 1:
                                ViewState["EncounterId"] = val;
                                break;
                            case 2:
                                ViewState["templateid_new"] = val;
                                break;
                            case 3:
                                lblVisitType.Text = val;
                                break;
                            case 4:
                                lblDoctorName.Text = val;
                                break;
                            case 5:
                                DateTime Vdt = Convert.ToDateTime(val);
                                if (common.myStr(Vdt) != "")
                                {
                                    lblVisitDate.Text = common.myStr(Vdt.ToString("dd/MM/yyyy"));//.Substring(0, 10); //common.myDate(val).ToString("dd/MM/yyyy");
                                }
                                break;
                            case 6:
                                ViewState["GroupId"] = val;
                                break;
                            case 7:
                                ViewState["IsLegacyData"] = val;
                                break;
                        }
                        idx++;
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
            length = 0;
            nodeType = string.Empty;
            nodeValues = string.Empty;
            idx1 = 0;
            idx = 0;
            valColl1 = null;
            valColl = null;
        }
    }

    protected void rdoTreeExpandCollapse_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        BindTreeColapse();
    }

    protected void rdoViewType_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        BindCategoryTree();
        rdoTreeExpandCollapse_OnSelectedIndexChanged(this, null);
    }
    private void BindTreeColapse()
    {
        if (rdoTreeExpandCollapse.SelectedItem.Value == "E")
        {
            tvCategory.CollapseAll();
        }
        else if (rdoTreeExpandCollapse.SelectedItem.Value == "C")
        {
            tvCategory.ExpandAll();
        }
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

    public void IsCopyCaseSheetAuthorized()
    {
        BaseC.Security objSecurity = new BaseC.Security(sConString);
        bool IsAuthorized = objSecurity.IsCopyCaseSheetAuthorized(common.myInt(Session["UserID"]), common.myInt(Session["HospitalLocationID"]));
        hdnIsCopyCaseSheetAuthorized.Value = common.myStr(IsAuthorized);
    }

    #region Print Report
    protected void btnPrintReport_OnClick(object sender, EventArgs e)
    {
        try
        {
            if (RTF1.Content.Equals(string.Empty))
            {
                Alert.ShowAjaxMsg("Report Format not tagged", this.Page);
                return;
            }
            DataSet ds = new DataSet();
            BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);

            ds = objEMR.getReportFormatDetails(common.myInt(Session["DoctorId"]));
            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ViewState["reportid"] = common.myStr(ds.Tables[0].Rows[0]["reportid"]);
                        ViewState["reportname"] = common.myStr(ds.Tables[0].Rows[0]["reportname"]);
                        ViewState["headerid"] = common.myStr(ds.Tables[0].Rows[0]["headerid"]);
                    }
                    else
                    {
                        Alert.ShowAjaxMsg("Report Format not tagged", this.Page);
                        return;
                    }
                }
                else
                {
                    Alert.ShowAjaxMsg("Report Format not tagged", this.Page);
                    return;
                }
            }
            else
            {
                Alert.ShowAjaxMsg("Report Format not tagged", this.Page);
                return;
            }
            getDoctorImage();
            generateReport();

            if (common.myLen(hdnReportContent.ClientID) > 0 && common.myInt(ViewState["reportid"]) > 0)
            {
                Session["PrintReportPastClinicalNotes"] = common.myStr(hdnReportContent.Value);

                RadWindow2.NavigateUrl = path + "Editor/PrintPastClinicalNotes.aspx?ReportId=" + common.myInt(ViewState["reportid"]) +
                             "&HeaderId=" + common.myInt(ViewState["headerid"]) +
                             "&RegistrationId=" + common.myInt(Session["RegistrationId"]); //yogesh 29/07/2022
                RadWindow2.Height = 500;
                RadWindow2.Width = 1000;
                RadWindow2.Top = 10;
                RadWindow2.Left = 10;
                RadWindow2.Behaviors = WindowBehaviors.Maximize | WindowBehaviors.Minimize | WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Pin;
                RadWindow2.Modal = true;
                RadWindow2.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
                RadWindow2.VisibleStatusbar = false;
            }
            else
            {
                Alert.ShowAjaxMsg("Data not found!", this.Page);
                return;
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
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
    protected void generateReport()
    {
        bool IsPrintDoctorSignature = false;
        DataSet ds = new DataSet();
        clsIVF objivf = new clsIVF(sConString);
        try
        {
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
            hdnReportContent.Value = "";

            //if (common.myInt(ddlReport.SelectedValue) > 0)
            //{
            if (common.myInt(ViewState["reportid"]) > 0)
            {
                //ds = objivf.EditReportName(common.myInt(ddlReport.SelectedValue));
                ds = objivf.EditReportName(common.myInt(ViewState["reportid"]));

                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        IsPrintDoctorSignature = common.myBool(ds.Tables[0].Rows[0]["IsPrintDoctorSignature"]);
                    }
                }

                PrintReport(true);
                hdnReportContent.Value = RTF1.Content;

                //comment as follow-up appointment is check inside the printreport function --Saten
                StringBuilder sbD = new StringBuilder();
                sbD.Append(" <table border='0' width='100%' style='border-collapse:collapse;' cellpadding='5' cellspacing='5' >");
                //sbD.Append("<tr><td>Follow Up : </td></tr>");

                //string SignatureLabel = common.myStr(ddlReport.SelectedItem.Attributes["SignatureLabel"]).Trim();
                string SignatureLabel = string.Empty;
                if (IsPrintDoctorSignature.Equals(true))
                {
                    sbD.Append("<tr><td align='right'>" + PrintReportSignature(IsPrintDoctorSignature) + "</td></tr>");
                }
                else
                {
                    if (SignatureLabel == "")
                    {
                        sbD.Append("<tr><td align='right'><b>Consultant&nbsp;Signature</b></td></tr>");
                    }
                    else
                    {
                        sbD.Append("<tr><td align='right'><b>" + SignatureLabel + "</b></td></tr>");
                    }
                }
                sbD.Append("<tr><td align='right'> </td></tr>");
                sbD.Append("</table>");
                hdnReportContent.Value = "<div style='margin-left:3em; '>" + hdnReportContent.Value + sbD.ToString() + "</div>";
            }
            else
            {
                //btnPrintReport.Visible = false;
                return;
            }
            //btnPrintReport.Visible = true;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    private string PrintReport(bool sign)
    {
        string strDisplayEnteredByInCaseSheet = common.myStr(Session["DisplayEnteredByInCaseSheet"]);
        Session["DisplayEnteredByInCaseSheet"] = string.Empty;

        StringBuilder sb = new StringBuilder();
        StringBuilder sbTemplateStyle = new StringBuilder();
        DataSet ds = new DataSet();
        DataSet dsTemplateStyle = new DataSet();
        DataTable dtTemplate = new DataTable();
        DataView dvDataFilter = new DataView();
        DataTable dtEncounter = new DataTable();


        BindNotes bnotes = new BindNotes(sConString);
        BaseC.DiagnosisDA fun = new BaseC.DiagnosisDA(sConString);
        BaseC.clsEMR emr = new BaseC.clsEMR(sConString);
        StringBuilder sbTemp = new StringBuilder();

        //sb.Append(getReportHeader(common.myInt(ddlReport.SelectedValue)));
        //sb.Append(getReportHeader(common.myInt(ViewState["reportid"])));

        clsIVF objIVF = new clsIVF(sConString);

        string strPatientHeader = objIVF.getCustomizedPatientReportHeader(common.myInt(ViewState["headerid"]));
        if (common.myLen(strPatientHeader).Equals(0))
        {
            // sb.Append(getIVFPatient().ToString());
            Session["strPatientHeader"] = getIVFPatient().ToString();
        }
        else
        {
            Session["strPatientHeader"] = sb.Append(getReportHeader(common.myInt(ViewState["reportid"]))) + strPatientHeader;
            //sb.Append(strPatientHeader);
        }

        string sTemplateName = common.myStr("ALL") == "ALL" ? "" : common.myStr("ALL");

        DataSet dsTemplateData = new DataSet();
        BindCaseSheet BindCaseSheet = new BindCaseSheet(sConString);

        try
        {


            Session["NoAllergyDisplay"] = null;
            if (sign == true)
            {
                //sb.Append(hdnDoctorImage.Value);
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
            Session["DisplayEnteredByInCaseSheet"] = strDisplayEnteredByInCaseSheet;
            sbTemplateStyle = null;
            ds.Dispose();
            dsTemplateStyle.Dispose();
            dvDataFilter.Dispose();
            dtTemplate.Dispose();
            bnotes = null;
            fun = null;
            emr = null;
            sbTemp = null;
            BindCaseSheet = null;
            dsTemplateData.Dispose();
        }
        return sb.ToString();
    }
    private StringBuilder getIVFPatient()
    {
        StringBuilder sb = new StringBuilder();
        try
        {

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

        catch (Exception Ex)
        {
            objException.HandleException(Ex);
            sb = new StringBuilder();
            return sb;
        }
    }
    //Yogesh 6/05/2022
    private StringBuilder getReportHeader(int ReportId)
    {
        StringBuilder sb = new StringBuilder();
        try
        {
            DataSet ds = new DataSet();
            bool IsPrintHospitalHeader = false;
            clsIVF objivf = new clsIVF(sConString);
            string ShowPrescriptionNABHLogoImage = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]),
            common.myInt(Session["FacilityId"]), "ShowPrescriptionNABHLogoImage", sConString);
            ds = objivf.EditReportName(ReportId);

            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    IsPrintHospitalHeader = common.myBool(ds.Tables[0].Rows[0]["IsPrintHospitalHeader"]);
                    ViewState["ShowPrintHeaderImage"] = common.myBool(ds.Tables[0].Rows[0]["PrintHeaderImage"]);
                    ViewState["PrintHeaderImagePath"] = common.myStr(ds.Tables[0].Rows[0]["PrintHeaderImagePath"]);

                    //yogesh
                    ViewState["HospitalWidth"] = common.myStr(ds.Tables[1].Rows[0]["Width"]);
                    ViewState["HospitalHeight"] = common.myStr(ds.Tables[1].Rows[0]["Height"]);
                    ViewState["NABHWidth"] = common.myStr(ds.Tables[1].Rows[1]["Width"]);
                    ViewState["NABHHeight"] = common.myStr(ds.Tables[1].Rows[1]["Height"]);

                    // shaiveee
                    ViewState["SignDoctorHeight"] = common.myStr(ds.Tables[1].Rows[2]["Width"]);
                    ViewState["SignDoctorWidth"] = common.myStr(ds.Tables[1].Rows[2]["Height"]);


                }
            }
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


            sb.Append("<div>");
            if (common.myBool(ViewState["ShowPrintHeaderImage"]).Equals(true))
            {
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
                sb.Append("<td align=center><b>" + common.myStr(ViewState["reportname"]) + "</b></td>");
                sb.Append("</tr></table>");

            }
            if (IsPrintHospitalHeader)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (ShowPrescriptionNABHLogoImage.Equals("Y"))
                    {

                        sb.Append("<table border='0' width='100%' cellpadding='0' cellspacing='0' style='font-size:small'>");
                        sb.Append("<tr>");
                        sb.Append("<td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td>");
                        sb.Append("</tr>");
                        sb.Append("<tr>");
                        sb.Append("<td colspan='2'>");
                        //yogesh
                        sb.Append("<img src='" + Server.MapPath("/Icons/SmallLogo.jpg") + "' border='0'  width='" + ViewState["HospitalWidth"] + "px' height='" + ViewState["HospitalHeight"] + "px' alt='Image'/>");
                        sb.Append("</td>");
                        sb.Append("<td colspan='7' >"); //yogesh
                        for (int idx = 0; idx < ds.Tables[0].Rows.Count; idx++)
                        {
                            DataRow DR = ds.Tables[0].Rows[idx];
                            sb.Append("<table border='0' cellpadding='0' cellspacing='0' style='font-size:small;margin-left: 10px !important;'>");
                            sb.Append("<tr>");
                            sb.Append("<td  align ='center' style='font-size:16pt' ><b>" + common.myStr(DR["FacilityName"]).Trim() + "</b></td>");
                            sb.Append("</tr>");
                            sb.Append("<tr>");

                            //yogesh 19/07/2022
                            if (common.myStr(DR["FacilityName"]).Equals("PRACHI HOSPITAL "))
                            {

                                sb.Append("<td align ='center' style='font-size:12pt'>" + common.myStr(DR["Name2"]).Trim() + "</td>");
                                sb.Append("</tr>");
                                sb.Append("<tr>");
                                sb.Append("<td align ='center' style='font-size:12pt'>" + common.myStr(DR["HeaderLine2"]).Trim() + "</td>");
                                sb.Append("</tr>");

                                sb.Append("<tr>");
                                sb.Append("<td align ='center' style='font-size:12pt'>" + common.myStr(DR["HeaderLine3"]).Trim() + "</td>");
                                sb.Append("</tr>");

                                sb.Append("<tr>");
                                sb.Append("<td align ='center' style='font-size:9pt'><p>Toll Free: " + common.myStr(DR["TollFree"]) + " Phone No: " + common.myStr(DR["Fax"]) + " Mob No: " + common.myStr(DR["Phone"]) + "</p></td>");
                                sb.Append("</tr>");

                                sb.Append("<tr>");
                                sb.Append("<td align ='center'  style='font-size:9px'><b> Email : " + common.myStr(DR["EmailId"]) + " Website : " + common.myStr(DR["WebSite"]) + "</b></td>");
                                sb.Append("</tr>");

                                sb.Append("</table>");
                            }
                            else
                            {

                                sb.Append("<td align ='center' style='font-size:10pt'>" + common.myStr(DR["Address1"]).Trim() + ", " + common.myStr(DR["Address2"]).Trim() + "</td>");
                                sb.Append("</tr>");
                                sb.Append("<tr>");
                                sb.Append("<td align ='center' style='font-size:10pt'>Mobile : " + common.myStr(DR["Phone"]) + " Phone : " + common.myStr(DR["Fax"]) + "</td>");
                                sb.Append("</tr>");
                                //sb.Append("<tr>");
                                //sb.Append("<td align ='center' style='font-size:10pt'>E-mail : " + common.myStr(DR["EmailId"]) + " Website : " + common.myStr(DR["WebSite"]) + "</td>");
                                //sb.Append("</tr>");
                                sb.Append("</table>");
                            }




                        }
                        sb.Append("</td>");
                        sb.Append("<td colspan='2'>");
                        #region nabh logo 
                        string FileNameNABHLogoImagePath = Server.MapPath("/Images/Logo/NABHLogo.jpg");
                        System.IO.FileInfo file = new System.IO.FileInfo(FileNameNABHLogoImagePath);
                        if (file.Exists)
                        {

                            StringBuilder sbPrescribed = new StringBuilder();
                            sb.Append("<table border='0' cellpadding='3' cellspacing='2'>");
                            sb.Append("<tr>");
                            //yogesh
                            sb.Append("<td><img align='left' src='" + FileNameNABHLogoImagePath + "' border='0' width='" + ViewState["NABHWidth"] + "px' height='" + ViewState["NABHHeight"] + "px'  alt='Image'/> </td>");
                            sb.Append("</tr>");
                            sb.Append("</table>");
                        }

                        #endregion
                        sb.Append("</td>");
                        sb.Append("</table>");
                        sb.Append("<table border='0' width='100%' style='text-align:center;'  cellpadding='2' cellspacing='3' ><tr>");
                        sb.Append("<td align=center><b>" + common.myStr("Prescription") + "</b></td>");
                        sb.Append("</tr></table></div>");
                        if (common.myBool(ViewState["ShowPrintHeaderImage"]).Equals(false) && IsPrintHospitalHeader == false)
                        {
                            sb.Append("<table border='0' width='100%' style='text-align:center;'  cellpadding='2' cellspacing='3' ><tr>");
                            sb.Append("<td align=center><b>" + common.myStr("Prescription") + "</b></td>");
                            sb.Append("</tr></table></div>");
                        }
                    }
                    else
                    {
                        sb.Append("<table border='0' width='100%' cellpadding='0' cellspacing='0' style='font-size:small'>");
                        for (int idx = 0; idx < ds.Tables[0].Rows.Count; idx++)
                        {
                            DataRow DR = ds.Tables[0].Rows[idx];
                            sb.Append("<tr>");
                            sb.Append("<td align ='center'>");
                            sb.Append("<table border='0' cellpadding='0' cellspacing='0'>");
                            sb.Append("<tr>");
                            sb.Append("<td></td><td></td><td></td><td></td><td></td>");
                            sb.Append("</tr>");
                            sb.Append("<tr>");
                            sb.Append("<td colspan='1' align ='right' valign='middle' style='font-size:9px'><img src='" + Server.MapPath("/Icons/SmallLogo.jpg") + "' border='0'  width='" + ViewState["HospitalLogoWidth"] + "px' height='" + ViewState["HospitalLogoHeight"] + "px' alt='Image'/></td>");
                            sb.Append("<td colspan='3' align ='center' valign='middle' style='font-size:9px'><b>" + common.myStr(DR["FacilityName"]).Trim() + "</b></td>");
                            sb.Append("<td colspan='3' ></td>");
                            sb.Append("</tr>");
                            sb.Append("</table>");
                            sb.Append("</td>");
                            sb.Append("</tr>");
                            sb.Append("<tr>");
                            sb.Append("<td align ='center'  style='font-size:9px'>" + common.myStr(DR["Address1"]).Trim() + ", " + common.myStr(DR["Address2"]).Trim() + "</td>");
                            sb.Append("</tr>");
                            sb.Append("<tr>");
                            sb.Append("<td align ='center'  style='font-size:9px'>Phone : " + common.myStr(DR["Phone"]) + " Fax : " + common.myStr(DR["Fax"]) + "</td>");
                            sb.Append("</tr>");
                        }
                        sb.Append("</table>");
                        sb.Append("<table border='0' width='100%' style='text-align:center;'  cellpadding='2' cellspacing='3' ><tr>");
                        sb.Append("<td align=center><b>" + common.myStr("Prescription") + "</b></td>");
                        sb.Append("</tr></table></div>");
                        //Awadhesh
                        if (common.myBool(ViewState["ShowPrintHeaderImage"]).Equals(false) && IsPrintHospitalHeader == false)
                        {
                            sb.Append("<table border='0' width='100%' style='text-align:center;'  cellpadding='2' cellspacing='3' ><tr>");
                            sb.Append("<td align=center><b>" + common.myStr("Prescription") + "</b></td>");
                            sb.Append("</tr></table></div>");
                        }

                    }
                }
            }

            return sb;
        }
        catch (Exception Ex)
        {
            sb = new StringBuilder();
            return sb;
        }
    }
    private StringBuilder getReportHeaderold(int ReportId)
    {
        StringBuilder sb = new StringBuilder();
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
                    sb.Append("<table border='0' width='100%' cellpadding='0' cellspacing='0' style='font-size:small'>");
                    for (int idx = 0; idx < ds.Tables[0].Rows.Count; idx++)
                    {
                        DataRow DR = ds.Tables[0].Rows[idx];

                        sb.Append("<tr>");

                        sb.Append("<td align ='center'>");
                        sb.Append("<table border='0' cellpadding='0' cellspacing='0'>");
                        sb.Append("<tr>");
                        sb.Append("<td></td><td></td><td></td><td></td><td></td>");
                        sb.Append("</tr>");
                        sb.Append("<tr>");
                        //sb.Append("<td colspan='2' align ='right' valign='middle' style='font-size:9px'><img src='../Icons/SmallLogo.jpg' border='0' width='30px' height='25px'  alt='Image'/></td>");
                        sb.Append("<td colspan='2' align ='right' valign='middle' style='font-size:9px'><img src='" + Server.MapPath("/Icons/SmallLogo.jpg") + "' border='0' width='100px' height='25px'  alt='Image'/></td>");
                        sb.Append("<td colspan='3' align ='left' valign='middle' style='font-size:9px'><b>" + common.myStr(DR["FacilityName"]).Trim() + "</b></td>");
                        sb.Append("</tr>");
                        sb.Append("</table>");
                        sb.Append("</td>");

                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td align ='center'  style='font-size:9px'>" + common.myStr(DR["Address1"]).Trim() + ", " + common.myStr(DR["Address2"]).Trim() + "</td>");
                        sb.Append("</tr>");

                        //sb.Append("<tr>");
                        //sb.Append("<td align ='center'>" + common.myStr(DR["CityName"]) + "(" + common.myStr(DR["PinNo"]) + "), " + common.myStr(DR["StateName"]) + "</td>");
                        //sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td align ='center'  style='font-size:9px'>Phone : " + common.myStr(DR["Phone"]) + " Fax : " + common.myStr(DR["Fax"]) + "</td>");
                        sb.Append("</tr>");
                    }
                    sb.Append("</table>");
                }
            }
            else
            {
                //sb.Append("<br />");
                //sb.Append("<br />");
                //sb.Append("<br />");
            }

            // sb.Append("<br />");
            sb.Append("<table border='0' width='100%' style='text-align:center;'  cellpadding='2' cellspacing='3' ><tr>");
            //sb.Append("<td align=center><U>" + common.myStr(ddlReport.SelectedItem.Text) + "</U></td>");
            sb.Append("<td align=center><U>" + "Past Clinical Notes" + "</U></td>");
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
    private string PrintReportSignature(bool Isdoctorsignature)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(getReportsSignature(Isdoctorsignature));
        return sb.ToString();
    }
    private StringBuilder getReportsSignature(bool IsPrintDoctorSignature)
    {
        StringBuilder sb = new StringBuilder();
        try
        {

            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            clsIVF objivf = new clsIVF(sConString);
            ds = new DataSet();
            dt = objivf.getDoctorSignatureDetails(common.myInt(Session["DoctorId"]), common.myInt(Session["FacilityId"]), common.myInt(Session["HospitalLocationID"])).Tables[0];
            if (IsPrintDoctorSignature)
            {
                sb.Append(" <table border='0' width='100%' style='border-collapse:collapse;' cellpadding='0' cellspacing='0'  >");
                if (dt.Rows.Count > 0)
                {

                    // shaiveee 
                    DataRow dr = dt.Rows[0] as DataRow;
                    string FileName = common.myStr(dr["ImageType"]).Trim();
                    string Splitter = ConfigurationManager.AppSettings["Split"];
                    if (common.myLen(Splitter).Equals(0))
                    {
                        Splitter = "!";
                    }
                    var csplitter = Splitter.ToCharArray();
                    string ftp = ftppath.Split(csplitter)[0].ToString();
                    string Filepath = FileFolder + "DoctorImages/" + FileName;
                    //Fetch the Response and read it into a MemoryStream object.
                    if (File.Exists(Server.MapPath("~/" + Filepath)))
                    {
                        File.Delete(Server.MapPath("~/" + Filepath));
                    }

                    Stream strm;
                    Object img = dr["SignatureImage"];
                    strm = new MemoryStream((byte[])img);
                    byte[] buffer = new byte[strm.Length];
                    int byteSeq = strm.Read(buffer, 0, System.Convert.ToInt32(strm.Length));
                    FileStream fs = new FileStream(Server.MapPath("/PatientDocuments/DoctorImages/" + FileName), FileMode.Create, FileAccess.Write);
                    fs.Write(buffer, 0, byteSeq);
                    fs.Dispose();
                    if (FileName != "")
                    {

                        sb.Append("<tr>");
                        if ((common.myInt(ViewState["SignDoctorHeight"].ToString()) > 0) && (common.myInt(ViewState["SignDoctorWidth"].ToString()) > 0))
                        {
                            int SignDoctorHeight = common.myInt(ViewState["SignDoctorHeight"].ToString());
                            int SignDoctorWidth = common.myInt(ViewState["SignDoctorWidth"].ToString());
                            sb.Append("<td align ='right'><img width='" + SignDoctorWidth + "px' align ='right' height='" + SignDoctorHeight + "px' src='" + Server.MapPath("~") + @"\PatientDocuments\DoctorImages\" + FileName + "' /></td>");
                        }
                        else
                        {
                            sb.Append("<td align ='right'><img width='100px' align ='right' height='50px' src='" + Server.MapPath("~") + @"\PatientDocuments\DoctorImages\" + FileName + "' /></td>");
                        }
                        sb.Append("</tr>");
                    }
                    // shaivee end





                    if (common.myStr(dt.Rows[0]["DoctorName"]).Trim().Length > 0)
                    {
                        sb.Append("<tr>");
                        sb.Append("<td align ='right'><b>" + common.myStr(dt.Rows[0]["DoctorName"]).Trim() + "</b></td>");
                        sb.Append("</tr>");
                    }
                    //if (common.myStr(dt.Rows[0]["Education"]).Trim().Length > 0)
                    //{
                    //    sb.Append("<tr>");
                    //    sb.Append("<td align ='right'><b>" + common.myStr(dt.Rows[0]["Education"]).Trim() + "</b></td>");
                    //    sb.Append("</tr>");
                    //}
                    //if (common.myStr(dt.Rows[0]["Designation"]).Trim().Length > 0)
                    //{
                    //    sb.Append("<tr>");
                    //    sb.Append("<td align ='right'><b>" + common.myStr(dt.Rows[0]["Designation"]).Trim() + "</b></td>");
                    //    sb.Append("</tr>");
                    //}
                    //if (common.myStr(dt.Rows[0]["UPIN"]).Trim().Length > 0)
                    //{
                    //    sb.Append("<tr>");

                    //    if (common.isNumeric(common.myStr(dt.Rows[0]["UPIN"]).Trim()))
                    //    {
                    //        sb.Append("<td align ='right'><b>Regn. No. : " + common.myStr(dt.Rows[0]["UPIN"]).Trim() + "</b></td>");
                    //    }
                    //    else
                    //    {
                    //        sb.Append("<td align ='right'><b>" + common.myStr(dt.Rows[0]["UPIN"]).Trim() + "</b></td>");
                    //    }

                    //    sb.Append("</tr>");
                    //}
                    if (common.myStr(dt.Rows[0]["SignatureLine1"]).Trim().Length > 0)
                    {
                        sb.Append("<tr>");
                        sb.Append("<td align ='right'><b>" + common.myStr(dt.Rows[0]["SignatureLine1"]).Trim() + "</b></td>");
                        sb.Append("</tr>");
                    }
                    if (common.myStr(dt.Rows[0]["SignatureLine2"]).Trim().Length > 0)
                    {
                        sb.Append("<tr>");
                        sb.Append("<td align ='right'><b>" + common.myStr(dt.Rows[0]["SignatureLine2"]).Trim() + "</b></td>");
                        sb.Append("</tr>");
                    }
                    if (common.myStr(dt.Rows[0]["SignatureLine3"]).Trim().Length > 0)
                    {
                        sb.Append("<tr>");
                        sb.Append("<td align ='right'><b>" + common.myStr(dt.Rows[0]["SignatureLine3"]).Trim() + "</b></td>");
                        sb.Append("</tr>");
                    }
                    if (common.myStr(dt.Rows[0]["SignatureLine4"]).Trim().Length > 0)
                    {
                        sb.Append("<tr>");
                        sb.Append("<td align ='right'><b>" + common.myStr(dt.Rows[0]["SignatureLine4"]).Trim() + "</b></td>");
                        sb.Append("</tr>");
                    }
                }
                sb.Append("</table>");
            }
            else
            {
                sb.Append("<br />");
                sb.Append("<br />");
                sb.Append("<br />");
            }
            return sb;
        }

        catch (Exception Ex)
        {
            objException.HandleException(Ex);
            sb = new StringBuilder();
            return sb;
        }
    }


    protected void lnkSendToPoliceStation_Click(object sender, EventArgs e)
    {
        try
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

            RadWindowForNew.NavigateUrl = "~/Editor/PrintPdf.aspx?SendEmail=1&TemplateId=" + common.myStr(ViewState["SelectedCheckedTemplateIdForObjectDetails"]);
            RadWindowForNew.Height = 100;
            RadWindowForNew.Width = 100;
            RadWindowForNew.Top = 10;
            RadWindowForNew.Left = 10;
            RadWindowForNew.Behaviors = WindowBehaviors.Maximize | WindowBehaviors.Minimize | WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Pin;
            RadWindowForNew.Modal = true;
            RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
            RadWindowForNew.VisibleStatusbar = false;


        }
        catch (Exception ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + ex.Message;
            objException.HandleException(ex);
        }
    }
    #endregion

}
