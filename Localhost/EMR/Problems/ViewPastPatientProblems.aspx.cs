using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Text;
using System.Net;
using System.Web.Script.Serialization;
using Newtonsoft.Json;


public partial class EMR_Problems_ViewPastPatientProblems_ : System.Web.UI.Page
{
    //private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    private static string WebAPIAddress = System.Configuration.ConfigurationManager.AppSettings["WebAPIAddress"].ToString();
    //clsExceptionLog objException = new clsExceptionLog();
    //BaseC.EMRProblems objbc2;
    //DataSet ds;
    //ArrayList colService;
    //StringBuilder sXMLOrderServices;
    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";

        if (common.myStr(Request.QueryString["MP"]) == "NO")
        {
            this.MasterPageFile = "~/Include/Master/BlankMaster.master";
        }
        else if (common.myStr(Request.QueryString["IsEMRPopUp"]) == "1")
        {
            Page.MasterPageFile = "/Include/Master/EMRBlankMaster.master";
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["UserID"] == null)

                if (common.myStr(Request.QueryString["MP"]) == "NO")
                {
                    btnBack.Visible = false;
                    btnSave.Visible = false;
                    btnClose.Visible = true;
                }
                else
                {
                    btnBack.Visible = true;
                    btnSave.Visible = true;
                    btnClose.Visible = false;
                }

            SetControls();
            BindFacility();
            BindProvider();
            ddlrange_SelectedIndexChanged(sender, e);
            BindProblem();

        }

        btnBack.Visible = false;
    }
    protected void btnFilter_Click(object sender, EventArgs e)
    {
        BindProblem();
    }
    void BindProvider()
    {
        DataSet ds = new DataSet();
        try
        {
            //objbc2 = new BaseC.EMRProblems(sConString);
            //ds = new DataSet();
            //ds = objbc2.GetDoctorName(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), common.myInt(Session["RegistrationId"]));

            //BaseC.EMR objEmr = new BaseC.EMR(sConString);
            //ds = objEmr.GetEMRDoctorPatientwise(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), common.myInt(Session["RegistrationId"]));

            string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/GetEMRDoctorPatientwise";

            APIRootClass.GetEMRDoctorPatientwise objRoot = new global::APIRootClass.GetEMRDoctorPatientwise();

            objRoot.HospitalLocationId = common.myInt(Session["HospitalLocationId"]);
            objRoot.FacilityId = common.myInt(Session["FacilityId"]);
            objRoot.RegistrationId = common.myInt(Session["RegistrationId"]);

            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;

            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);

            ds = JsonConvert.DeserializeObject<DataSet>(sValue);


            if (ds.Tables[0].Rows.Count > 0)
            {

                ddlProvider.DataSource = ds;
                ddlProvider.DataTextField = "DoctorName";
                ddlProvider.DataValueField = "DoctorId";
                ddlProvider.DataBind();
                ddlProvider.Items.Insert(0, new RadComboBoxItem("All", "0"));
                ddlProvider.SelectedIndex = 0;
                ddlProvider.SelectedIndex = ddlProvider.Items.IndexOf(ddlProvider.Items.FindItemByValue(common.myStr(Session["DoctorID"])));

            }
        }

        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
        finally
        {
            ds.Dispose();
        }
    }
    void BindFacility()
    {
        DataSet ds = new DataSet();
        try
        {
            //objbc2 = new BaseC.EMRProblems(sConString);
            
            //ds = objbc2.GetFacility(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["UserId"]), common.myInt(Session["GroupID"]));

            string ServiceURL = WebAPIAddress.ToString() + "api/Common/getFacilityList";

            APIRootClass.getFacilityList objRoot = new global::APIRootClass.getFacilityList();

            objRoot.HospitalLocationId = common.myInt(Session["HospitalLocationId"]);
            objRoot.UserId = common.myInt(Session["UserId"]);
            objRoot.GroupID = common.myInt(Session["GroupID"]);
            objRoot.EncodedBy = 0;

            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;

            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            ds = JsonConvert.DeserializeObject<DataSet>(sValue);

            if (ds.Tables[0].Rows.Count > 0)
            {

                ddlFacility.DataSource = ds;
                ddlFacility.DataTextField = "FacilityName";
                ddlFacility.DataValueField = "FacilityId";
                ddlFacility.DataBind();
                ddlFacility.Items.Insert(0, new RadComboBoxItem("All", "0"));
                ddlFacility.SelectedIndex = 0;
                ddlFacility.SelectedIndex = ddlFacility.Items.IndexOf(ddlFacility.Items.FindItemByValue(common.myStr(Session["FacilityId"])));

            }
        }

        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
        finally
        {
            ds.Dispose();
        }
    }
    private void SetControls()
    {

        ddlrange.Items.Clear();

        RadComboBoxItem ls = new RadComboBoxItem();
        ls.Text = "Select All";
        ls.Value = "";
        ls.Selected = true;
        ddlrange.Items.Add(ls);

        RadComboBoxItem lst3 = new RadComboBoxItem();
        lst3.Text = "Today";
        lst3.Value = "DD0";
        ddlrange.Items.Add(lst3);

        //RadComboBoxItem lst10 = new RadComboBoxItem();
        //lst10.Text = "This Week";
        //lst10.Value = "WW0";
        //ddlrange.Items.Add(lst10);


        RadComboBoxItem lst4 = new RadComboBoxItem();
        lst4.Text = "Last Week";
        lst4.Value = "WW-1";
        ddlrange.Items.Add(lst4);

        RadComboBoxItem lst5 = new RadComboBoxItem();
        lst5.Text = "Last Two Week";
        lst5.Value = "WW-2";
        ddlrange.Items.Add(lst5);

        //RadComboBoxItem lst8 = new RadComboBoxItem();
        //lst8.Text = "This Month";
        //lst8.Value = "MM0";
        //ddlrange.Items.Add(lst8);

        RadComboBoxItem lst9 = new RadComboBoxItem();
        lst9.Text = "Last One Month";
        lst9.Value = "MM-1";
        ddlrange.Items.Add(lst9);

        RadComboBoxItem lst6 = new RadComboBoxItem();
        lst6.Text = "Last Year";
        lst6.Value = "YY-1";
        ddlrange.Items.Add(lst6);

        RadComboBoxItem lst7 = new RadComboBoxItem();
        lst7.Text = "Date Range";
        lst7.Value = "4";
        ddlrange.Items.Add(lst7);


    }

    protected void ddlrange_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlrange.SelectedValue == "4")
        {
            //   tblDateRange.Visible = true;

            lblfrndate.Visible = true;
            dtpfrmdate.Visible = true;

            lbltodate.Visible = true;
            dtpTodate.Visible = true;

            dtpfrmdate.DateInput.DateFormat = Session["OutputDateFormat"].ToString();
            dtpfrmdate.DateInput.DisplayDateFormat = Session["OutputDateFormat"].ToString();
            dtpfrmdate.SelectedDate = common.myDate(DateTime.Now.ToString(common.myStr(Session["OutputDateFormat"])));

            dtpTodate.DateInput.DateFormat = Session["OutputDateFormat"].ToString();
            dtpTodate.DateInput.DisplayDateFormat = Session["OutputDateFormat"].ToString();
            dtpTodate.SelectedDate = common.myDate(DateTime.Now.ToString(common.myStr(Session["OutputDateFormat"])));

        }
        else
        {
            //    tblDateRange.Visible = false;



            dtpfrmdate.DateInput.DateFormat = Session["OutputDateFormat"].ToString();
            dtpfrmdate.DateInput.DisplayDateFormat = Session["OutputDateFormat"].ToString();
            dtpfrmdate.DateInput.Text = "";
            dtpfrmdate.SelectedDate = null;

            dtpTodate.DateInput.DateFormat = Session["OutputDateFormat"].ToString();
            dtpTodate.DateInput.DisplayDateFormat = Session["OutputDateFormat"].ToString();
            dtpTodate.DateInput.Text = "";
            dtpTodate.SelectedDate = null;

            lblfrndate.Visible = false;
            dtpfrmdate.Visible = false;

            lbltodate.Visible = false;
            dtpTodate.Visible = false;

        }
        //BindColorLegend();
    }
    protected void ddlProvider_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void gvPatientHistory_OnItemCommand(object sender, GridCommandEventArgs e)
    {
        //try
        //{
        //    ViewState["EncounterId"] = common.myStr(((HiddenField)e.Item.FindControl("hdnEncounterId")).Value).Trim();
        //    ViewState["RegistrationId"] = common.myStr(((HiddenField)e.Item.FindControl("hdnRegistrationId")).Value).Trim();
        //    Session["RegistrationID"] = common.myStr(((HiddenField)e.Item.FindControl("hdnRegistrationId")).Value).Trim();

        //    if (e.CommandName == "Add")
        //    {

        //        if (common.myStr(ViewState["EncounterId"]) != "" && common.myStr(ViewState["RegistrationId"]) != "")
        //        {
        //            RadWindowForNew.NavigateUrl = "/EMR/Masters/ViewPatientHistory.aspx?RegId=" + common.myStr(ViewState["RegistrationId"]) + "&EncId=" + common.myStr(ViewState["EncounterId"]) + "";
        //            RadWindowForNew.Height = 600;
        //            RadWindowForNew.Width = 900;
        //            RadWindowForNew.Top = 20;
        //            RadWindowForNew.Left = 20;
        //            // RadWindowForNew.Title = "Time Slot";
        //            RadWindowForNew.OnClientClose = "OnClientClose";
        //            RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        //            RadWindowForNew.Modal = true;
        //            RadWindowForNew.InitialBehavior = WindowBehaviors.Maximize;
        //            RadWindowForNew.VisibleStatusbar = false;
        //        }
        //        else
        //        {
        //            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
        //            lblMessage.Text = "Please Select Patient!";
        //        }
        //    }
        //}
        //catch (Exception Ex)
        //{
        //    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
        //    lblMessage.Text = "Error: " + Ex.Message;
        //    objException.HandleException(Ex);
        //}
    }
    protected void gvPatientHistory_OnItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item.ItemType == GridItemType.Item)
        {

        }
    }

    void BindProblem()
    {
        //objbc2 = new BaseC.EMRProblems(sConString);
        //ds = new DataSet();
        DataSet ds = new DataSet();
        try
        {
            DateTime FrmDate = common.myDate(dtpfrmdate.SelectedDate);
            DateTime ToDate = common.myDate(dtpTodate.SelectedDate);
            //if (common.myStr(dtpfrmdate.SelectedDate) != "" && common.myStr(dtpTodate.SelectedDate) != "")
            //{
            //    ds = objbc2.GetChiefProblem(common.myInt(Session["HospitalLocationId"]), common.myInt(ddlFacility.SelectedValue), common.myInt(Session["RegistrationId"]), common.myInt(ddlProvider.SelectedValue), "", common.myStr(FrmDate.ToString("yyyy/MM/dd")), common.myStr(ToDate.ToString("yyyy/MM/dd")), "%%", false, common.myBool(chkChronics.Checked), 0, common.myStr(ddlSource.SelectedValue));
            //}
            //else
            //{
            //    ds = objbc2.GetChiefProblem(common.myInt(Session["HospitalLocationId"]), common.myInt(ddlFacility.SelectedValue), common.myInt(Session["RegistrationId"]), common.myInt(ddlProvider.SelectedValue), common.myStr(ddlrange.SelectedValue), common.myStr(dtpfrmdate.SelectedDate), common.myStr(dtpTodate.SelectedDate), "%%", false, common.myBool(chkChronics.Checked), 0, common.myStr(ddlSource.SelectedValue));
            //}

            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/GetChiefProblem";
            APIRootClass.GetChiefProblem objRoot = new global::APIRootClass.GetChiefProblem();
            objRoot.HospitalLocationID = common.myInt(Session["HospitalLocationId"]);
            objRoot.FacilityId = common.myInt(ddlFacility.SelectedValue);
            objRoot.RegistrationId = common.myInt(Session["RegistrationId"]);
            objRoot.DoctorId = common.myInt(ddlProvider.SelectedValue);
            
            if (common.myStr(dtpfrmdate.SelectedDate) != "" && common.myStr(dtpTodate.SelectedDate) != "")
            {
                objRoot.Daterange = "";
                objRoot.FromDate = common.myStr(FrmDate.ToString("yyyy/MM/dd"));
                objRoot.ToDate = common.myStr(ToDate.ToString("yyyy/MM/dd"));
            }
            else
            {
                objRoot.Daterange = common.myStr(ddlrange.SelectedValue);
                objRoot.FromDate = common.myStr(dtpfrmdate.SelectedDate);
                objRoot.ToDate = common.myStr(dtpTodate.SelectedDate);
            }
            objRoot.SearchCriteriya = "%%";
            objRoot.IsDistinct = false;
            objRoot.IsChronic = chkChronics.Checked;
            objRoot.ProblemId = 0;
            objRoot.VisitType = ddlSource.SelectedValue;

            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            //sValue = JsonConvert.DeserializeObject<string>(sValue);

            ds = JsonConvert.DeserializeObject<DataSet>(sValue);

            if (ds.Tables[0].Rows.Count == 0)
            {
                divmessage.Visible = true;
                gvPatientHistory.Visible = false;
                //ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());
                //ds.Tables[0].AcceptChanges();
            }
            else
            {
                gvPatientHistory.Visible = true;
                divmessage.Visible = false;
                gvPatientHistory.DataSource = ds;
                gvPatientHistory.DataBind();
            }
        }
        catch(Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
        finally
        { ds.Dispose(); }
    }
    protected void btnBack_Click(object sender, EventArgs e)
    {
        if (common.myStr(Request.QueryString["MP"]).ToUpper().Equals("NO"))
        {
            Response.Redirect("/EMR/Problems/Default.aspx?From=POPUP");
        }
        else
        {
            Response.Redirect("/EMR/Problems/Default.aspx");
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        DataSet ds = new DataSet();
        try
        {

            //if (common.myInt(Session["UserId"]) != common.myInt(ddlProvider.SelectedValue))
            //{
            //    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //    lblMessage.Text = "You are not authorized to edit problems !";
            //    return;
            //}
            {
                //DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                //objbc2 = new BaseC.EMRProblems(sConString);
                string strsave = "";
                ArrayList colService = new ArrayList();
                StringBuilder sXMLOrderServices = new StringBuilder();

                foreach (GridDataItem item in gvPatientHistory.MasterTableView.Items)
                {
                    CheckBox chkRow = (CheckBox)item["View"].FindControl("CheckBox1");
                    if (chkRow.Checked == true)
                    {
                        HiddenField HdnProblemId = (HiddenField)item.FindControl("HdnProblemId");
                        LinkButton lnkbtnProblem = (LinkButton)item.FindControl("lnkbtnProblem");
                        HiddenField lblId = (HiddenField)item.FindControl("lblId");
                        HiddenField hdnEncounterId = (HiddenField)item.FindControl("hdnEncounterId");
                        HiddenField hdnRegistrationId = (HiddenField)item.FindControl("hdnRegistrationId");

                        Label lblLocationId = (Label)item.FindControl("lblLocationId");
                        Label lblOnsetId = (Label)item.FindControl("lblOnsetId");
                        Label lblDurationId = (Label)item.FindControl("lblDurationId");
                        Label lblDuration = (Label)item.FindControl("lblDuration");
                        Label lblQualityId = (Label)item.FindControl("lblQualityId");
                        Label lblContextId = (Label)item.FindControl("lblContextId");
                        Label lblContext = (Label)item.FindControl("lblContext");
                        Label lblSeverityId = (Label)item.FindControl("lblSeverityId");
                        Label lblSeverity = (Label)item.FindControl("lblSeverity");

                        Label lblLocation = (Label)item.FindControl("lblLocation");
                        Label lblOnset = (Label)item.FindControl("lblOnset");
                        Label lblQuality = (Label)item.FindControl("lblQuality");

                        HiddenField hdnDoctorId = (HiddenField)item.FindControl("hdnDoctorId");
                        HiddenField hdnFacilityId = (HiddenField)item.FindControl("hdnFacilityId");
                        Label lblPrimary = (Label)item.FindControl("lblPrimary");
                        HiddenField hdnChronic = (HiddenField)item.FindControl("hdnChronic");
                        Label lblChronic = (Label)item.FindControl("lblChronic");
                        Label lblSCTId = (Label)item.FindControl("lblSCTId");

                        Label lblSide = (Label)item.FindControl("lblSide");
                        Label lblSideDescription = (Label)item.FindControl("lblSideDescription");
                        Label lblConditionId = (Label)item.FindControl("lblConditionId");
                        Label lblCondition = (Label)item.FindControl("lblCondition");

                        Label lblAssociatedProblemID1 = (Label)item.FindControl("lblAssociatedProblemID1");
                        Label lblAssociatedProblem1 = (Label)item.FindControl("lblAssociatedProblem1");
                        Label lblAssociatedProblemID2 = (Label)item.FindControl("lblAssociatedProblemID2");
                        Label lblAssociatedProblem2 = (Label)item.FindControl("lblAssociatedProblem2");
                        Label lblAssociatedProblemID3 = (Label)item.FindControl("lblAssociatedProblemID3");
                        Label lblAssociatedProblem3 = (Label)item.FindControl("lblAssociatedProblem3");
                        Label lblAssociatedProblemID4 = (Label)item.FindControl("lblAssociatedProblemID4");
                        Label lblAssociatedProblem4 = (Label)item.FindControl("lblAssociatedProblem4");
                        Label lblAssociatedProblemID5 = (Label)item.FindControl("lblAssociatedProblemID5");
                        Label lblAssociatedProblem5 = (Label)item.FindControl("lblAssociatedProblem5");
                        Label lblPercentage = (Label)item.FindControl("lblPercentage");
                        HiddenField hdnDurations = (HiddenField)item.FindControl("hdnPDurations");

                        HiddenField hdnDurationType = (HiddenField)item.FindControl("hdnPDurationType");

                        colService.Add("");
                        colService.Add(HdnProblemId.Value);
                        colService.Add(lnkbtnProblem.Text);
                        colService.Add(lblDurationId.Text);
                        colService.Add(lblDuration.Text);
                        colService.Add(lblContextId.Text);
                        colService.Add(lblContext.Text);
                        colService.Add(lblSeverityId.Text);
                        colService.Add(lblSeverity.Text);
                        colService.Add("0"); //objXML.Append(chkPrimarys.Checked);               
                        colService.Add(hdnChronic.Value);   //objXML.Append(chkChronics.Checked);              
                        colService.Add(hdnDoctorId.Value);
                        colService.Add(hdnFacilityId.Value);
                        colService.Add(lblSCTId.Text);
                        colService.Add(lblQualityId.Text);
                        colService.Add(lblLocationId.Text);
                        colService.Add(lblLocation.Text);
                        colService.Add(lblOnsetId.Text);
                        colService.Add(lblAssociatedProblemID1.Text);
                        colService.Add(lblAssociatedProblem1.Text);
                        colService.Add(lblAssociatedProblemID2.Text);
                        colService.Add(lblAssociatedProblem2.Text);
                        colService.Add(lblAssociatedProblemID3.Text);
                        colService.Add(lblAssociatedProblem3.Text);
                        colService.Add(lblAssociatedProblemID4.Text);
                        colService.Add(lblAssociatedProblem4.Text);
                        colService.Add(lblAssociatedProblemID5.Text);
                        colService.Add(lblAssociatedProblem5.Text);

                        if (common.myInt(lblSide.Text) == -1)
                        {
                            colService.Add(-1);
                        }
                        else
                        {
                            colService.Add(lblSide.Text);
                        }

                        if (common.myInt(lblConditionId.Text) == -1)
                        {
                            colService.Add(-1);
                        }
                        else
                        {
                            colService.Add(lblConditionId.Text);
                        }

                        colService.Add(lblPercentage.Text);
                        colService.Add(common.myStr(hdnDurations.Value));
                        colService.Add(common.myStr(hdnDurationType.Value));
                        sXMLOrderServices.Append(common.setXmlTable(ref colService));



                        //ds = objbc2.CheckDuplicateProblem(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["RegistrationId"]), common.myInt(Session["EncounterId"]), common.myInt(HdnProblemId.Value), common.myBool(hdnChronic.Value));

                        WebClient client1 = new WebClient();
                        client1.Headers["Content-type"] = "application/json";
                        client1.Encoding = Encoding.UTF8;
                        string ServiceURL1 = WebAPIAddress.ToString() + "api/EMRAPI/CheckDuplicateProblem";
                        APIRootClass.CheckDuplicateProblem objRoot1 = new global::APIRootClass.CheckDuplicateProblem();
                        objRoot1.HospitalLocationId = common.myInt(Session["HospitalLocationId"]);
                        objRoot1.RegistrationId = common.myInt(Session["RegistrationId"]);
                        objRoot1.EncounterId = common.myInt(Session["EncounterId"]);
                        objRoot1.ProblemId = common.myInt(HdnProblemId.Value);
                        objRoot1.IsChronic = common.myBool(hdnChronic.Value);

                        string inputJson1 = (new JavaScriptSerializer()).Serialize(objRoot1);
                        string sValue1 = client1.UploadString(ServiceURL1, inputJson1);
                        sValue1 = JsonConvert.DeserializeObject<string>(sValue1);
                        ds = JsonConvert.DeserializeObject<DataSet>(sValue1);

                        if (ds.Tables[0].Rows.Count > 0)
                        {

                            //if (btnSave.Text == "Add To Todays Chief Problem")
                            //{
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                if (HdnProblemId.Value.ToString().Trim() == ds.Tables[0].Rows[i]["ProblemId"].ToString().Trim())
                                {
                                    //Alert.ShowAjaxMsg("This problem is already exists", Page);
                                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                                    lblMessage.Text = "This  (" + lnkbtnProblem.Text.Trim() + ")  already exists!";
                                    return;
                                }
                            }
                            // }
                        }


                    }


                }
                if (sXMLOrderServices.ToString() == "")
                {
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                    lblMessage.Text = "Please Select Problem!";
                    return;
                }
                //strsave = objbc2.EMRSavePatientProblems(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), common.myInt(Session["RegistrationId"]), common.myInt(Session["EncounterId"]), 0, sXMLOrderServices.ToString(), common.myInt(Session["UserId"]), "", false, false, false, common.myInt(Session["DoctorId"]));

                WebClient client = new WebClient();
                client.Headers["Content-type"] = "application/json";
                client.Encoding = Encoding.UTF8;
                string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/SavePatientProblems";
                APIRootClass.SavePatientProblems objRoot = new global::APIRootClass.SavePatientProblems();
                objRoot.HospitalLocationID = common.myInt(Session["HospitalLocationId"]);
                objRoot.FacilityId = common.myInt(Session["FacilityId"]);
                objRoot.RegistrationId = common.myInt(Session["RegistrationId"]);
                objRoot.EncounterId = common.myInt(Session["EncounterId"]);
                objRoot.PageId = 0;
                objRoot.xmlProblemDetails = sXMLOrderServices.ToString();
                objRoot.UserId = common.myInt(Session["UserId"]);
                objRoot.Remarks = "";
                objRoot.IsPregment = false;
                objRoot.IsBreastFeed = false;
                objRoot.IsShowNote = false;
                objRoot.DoctorId = common.myInt(Session["DoctorID"]);

                string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
                string sValue = client.UploadString(ServiceURL, inputJson);
                sValue = JsonConvert.DeserializeObject<string>(sValue);
                strsave = JsonConvert.DeserializeObject<string>(sValue);

                if (strsave.Contains("Data Saved!"))
                {
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                    lblMessage.Text = strsave;
                }
                else
                {
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                    lblMessage.Text = strsave;
                }
            }

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
        finally
        {
            ds.Dispose();
        }
    }
    protected void btnrefresh_Click(object sender, EventArgs e)
    {
        lblMessage.Text = "";
        chkChronics.Checked = false;
        SetControls();
        BindFacility();
        BindProvider();
        ddlrange_SelectedIndexChanged(sender, e);
        BindProblem();
    }

    protected void lnkbtnProblem_OnClick(object sender, EventArgs e)
    {
        LinkButton lnkBtn = (LinkButton)sender;
        string lnkbtnProblem = common.myStr(((LinkButton)lnkBtn.FindControl("lnkbtnProblem")).Text);
        HiddenField Id = (HiddenField)lnkBtn.FindControl("hdnId");
        HiddenField hdnEncounterId = (HiddenField)lnkBtn.FindControl("hdnEncounterId");
        HiddenField hdnRegistrationId = (HiddenField)lnkBtn.FindControl("hdnRegistrationId");
        HiddenField HdnProblemId = (HiddenField)lnkBtn.FindControl("HdnProblemId");
        HiddenField hdnDoctorId = (HiddenField)lnkBtn.FindControl("hdnDoctorId");
        HiddenField hdnFacilityId = (HiddenField)lnkBtn.FindControl("hdnFacilityId");
        HiddenField hdnChronic = (HiddenField)lnkBtn.FindControl("hdnChronic");

        if (common.myInt(HdnProblemId.Value) > 0)
        {
            RadWindowForNew.NavigateUrl = "/EMR/Problems/PoPupCheifcomplaints.aspx?ProblemId=" + HdnProblemId.Value + "&RegId=" + hdnRegistrationId.Value + "&EncId=" + hdnEncounterId.Value + "&FacilityId=" + hdnFacilityId.Value + "&DoctorID=" + hdnDoctorId.Value + "&Chronic=" + hdnChronic.Value + "&PName=" + lnkbtnProblem;
            RadWindowForNew.Height = 500;
            RadWindowForNew.Width = 750;
            RadWindowForNew.Top = 10;
            RadWindowForNew.Left = 10;

            RadWindowForNew.VisibleOnPageLoad = true;
            RadWindowForNew.Modal = true;
            //    RadWindowForNew.Behaviors = WindowBehaviors.Maximize | WindowBehaviors.Minimize | WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Pin | WindowBehaviors.Resize;
            RadWindowForNew.VisibleStatusbar = false;
            RadWindowForNew.InitialBehaviors = Telerik.Web.UI.WindowBehaviors.Default;
        }

    }
}

