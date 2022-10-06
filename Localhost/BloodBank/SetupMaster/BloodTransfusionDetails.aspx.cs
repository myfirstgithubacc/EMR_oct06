using System;
using System.Data;
using System.Web.UI;
using System.Configuration;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Text;
using System.Collections;

public partial class BloodBank_SetupMaster_BloodTransfusionDetails : System.Web.UI.Page
{

    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    BaseC.clsBb objBb;
    UserAuthorisations ua1 = new UserAuthorisations();
    StringBuilder strXML;
    ArrayList coll;


    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
        if (common.myStr(Request.QueryString["MP"]) == "NO")
        {
            this.MasterPageFile = "~/Include/Master/BlankMaster.master";
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                objBb = new BaseC.clsBb(sConString);
                ViewState["_ID"] = "0";
                //bindDetailsData();
                SetPermission();
                gvData.DataSource = string.Empty;
                gvData.DataBind();

                if (Request.QueryString["EncNo"] != null)
                {
                    hdnRegistrationID.Value = Convert.ToString(Request.QueryString["Regid"]);
                    hdnRegistrationNo.Value = Convert.ToString(Request.QueryString["RegNo"]);
                    hdnEncounterID.Value = Convert.ToString(Request.QueryString["EncId"]);
                    hdnEncounterNo.Value = Convert.ToString(Request.QueryString["EncNo"]);
                }
                else if (Request.QueryString["EncNo"] == null)
                {
                    btnSaveData.Visible = false;
                    btnNew.Visible = false;
                    btnclose.Visible = false;
                }
            }
            catch (Exception Ex)
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Error: " + Ex.Message;
                objException.HandleException(Ex);
            }
        }

    }
    protected void btnNew_OnClick(object sender, EventArgs e)
    {
        Response.Redirect(common.myStr(Request.Url.AbsoluteUri), false);
    }

    protected void btnSaveData_OnClick(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            //  string check = txtCurrentCrossMatchUnits.Text.Trim();
            foreach (GridDataItem item in gvData.Items)
            {
                RadDatePicker startDate = item.FindControl("dtpStartTime") as RadDatePicker;
                if (startDate.SelectedDate == null)
                {
                    Alert.ShowAjaxMsg("Start Time is compulsary.", Page);
                    return;
                }
            }
            SaveData();
        }
    }

    private void SaveData()
    {
        try
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = string.Empty;
            objBb = new BaseC.clsBb(sConString);

            StringBuilder build = new StringBuilder();
            string strMsg = "";


            strXML = new StringBuilder();
            coll = new ArrayList();

            if (gvData != null)
            {

                foreach (GridDataItem item in gvData.Items)
                {
                    coll.Add(((item.FindControl("lblUnitNumber") as Label).Text.Trim()));
                    coll.Add((item.FindControl("hdnBloodGroupID") as HiddenField).Value.Trim());
                    coll.Add((item.FindControl("hdnComponentID") as HiddenField).Value.Trim());

                    RadComboBox transfusionStart = item.FindControl("ddlTransfusionStart") as RadComboBox;
                    if (transfusionStart.SelectedValue != null)
                    {
                        coll.Add(transfusionStart.SelectedValue);
                    }
                    else
                    {
                        coll.Add(0);
                    }

                    RadDatePicker startDate = item.FindControl("dtpStartTime") as RadDatePicker;
                    RadDatePicker endDate = item.FindControl("dtpEndTime") as RadDatePicker;
                    if (startDate.SelectedDate != null)
                    {
                        coll.Add(startDate.SelectedDate.Value);
                    }
                    else
                    {
                        // 01/01/1980 is default min value for radtimepicker
                        coll.Add(DBNull.Value);
                        //coll.Add("01/01/1980");
                    }
                    if (endDate.SelectedDate != null)
                    {
                        coll.Add(endDate.SelectedDate.Value);
                    }
                    else
                    {
                        // 01/01/1980 is default min value for radtimepicker
                        coll.Add(DBNull.Value);
                        //coll.Add("01/01/1980");

                    }

                    coll.Add(chkEnsureTransfusion.Checked ? 1 : 0);
                    coll.Add(chkUnitsReceived.Checked ? 1 : 0);
                    coll.Add(chkBaseLineVitals.Checked ? 1 : 0);
                    coll.Add(chkUnitsChecked.Checked ? 1 : 0);   

                    coll.Add((item.FindControl("txtReaction") as RadTextBox).Text.Trim());
                    coll.Add((item.FindControl("txtReason") as RadTextBox).Text.Trim());

                    coll.Add(common.myInt(Session["UserID"]));
                    int BloodTransfusionEndBy = 0;
                    coll.Add(BloodTransfusionEndBy);
                         
                    strXML.Append(common.setXmlTable(ref coll));                    
                }
            }
            if(strXML.ToString()!="")
                strMsg = objBb.SavePatientTransfusionDetails(common.myInt(hdnPatientTransfusionID.Value), common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), hdnEncounterNo.Value, common.myLong(hdnRegistrationNo.Value), txtIssueNo.Text.Trim(), txtIssueDate.Text.Trim(), common.myInt(hdnPatientBloodGroupID.Value), txtRemarks.Text.Trim(), common.myInt(Session["UserID"]), strXML.ToString());

            if ((strMsg.ToUpper().Contains(" UPDATE") || strMsg.ToUpper().Contains(" SAVE")))
            {
                /*
                txtIssueNo.Text = strMsg.Split(new char[] { '-' })[1].ToString();
                strMsg = strMsg.Split(new char[] { '-' })[0].ToString();
                strMsg += " Issue no. is " + txtIssueNo.Text.Trim();
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                clearControl();
                ViewState["_ID"] = "0";
                SetPermission();
                 */
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                clearControl();
                //ViewState["_ID"] = "0";
                SetPermission();

            }
            lblMessage.Text = strMsg;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private void clearControl()
    {
       
        txtPatientName.Text = string.Empty;
        txtIssueDate.Text = "";
        txtPatientBloodGroup.Text = "";
        txtRemarks.Text = "";
        chkBaseLineVitals.Checked = false;
        chkEnsureTransfusion.Checked = false;
        chkUnitsChecked.Checked = false;
        chkUnitsReceived.Checked = false;        
        gvData.DataSource = string.Empty;
        txtIssueNo.Text = string.Empty;
        gvData.DataSource = string.Empty;
        gvData.DataBind();
    }


    private void SetPermission()
    {
        ua1.DisableEnableControl(btnSaveData, false);
        ua1.DisableEnableControl(btnNew, false);

        if (ua1.CheckPermissions("N", Request.Url.AbsolutePath))
        {
            ua1.DisableEnableControl(btnSaveData, true);
            ua1.DisableEnableControl(btnNew, true);
        }
        if (ua1.CheckPermissions("E", Request.Url.AbsolutePath))
        {
            // ua1.DisableEnableControl(btnSaveData, true);
            //ua1.DisableEnableControl(btnNew, true);
            // ua1.DisableEnableControl(btnclose, true);
            //ua1.DisableEnableControl(btnPost, true);
        }
        if (ua1.CheckPermissions("C", Request.Url.AbsolutePath))
        {
            //ua1.DisableEnableControl(btnCancel, true);
        }
        if (ua1.CheckPermissions("P", Request.Url.AbsolutePath))
        {
            //ua1.DisableEnableControl(bt, true);
            //ua1.DisableEnableControl(btnPrintPreview, true);
            //ua1.DisableEnableControl(btnPrint, true);
        }
        ua1.Dispose();
    }

    protected void btnAddItem_Click(object sender, ImageClickEventArgs e)
    {
        hdnComponentIssueNo.Value = "";
        hdnEncounterNo.Value = "";
        hdnRequisition.Value = "";
        hdnRequisition.Value = "";
        hdnCrossMatchId.Value = "";

        hdnComponentIssueId.Value = "";
        RadWindowForNew.NavigateUrl = "BloodTransfusionList.aspx?Regid=" + hdnRegistrationID.Value
                       + "&RegNo=" + common.myStr(hdnRegistrationNo.Value)
                        + "&EncId=" + common.myInt(hdnEncounterID.Value)
                        + "&EncNo=" + common.myStr(hdnEncounterNo.Value)
                        + "&Status=0";
        RadWindowForNew.Height = 450;
        RadWindowForNew.Width = 700;
        RadWindowForNew.Top = 200;
        RadWindowForNew.Left = 200;
        RadWindowForNew.OnClientClose = "SearchComponentIssueDetailsOnClientClose";
        RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindowForNew.Modal = true;
        RadWindowForNew.VisibleStatusbar = false;
    }


    protected void btnGetRequisitionInfo_Click(object sender, EventArgs e)
    {
        if (hdnCrossMatchId.Value != "")
        {
            GetIssueComponent();
        }
        else
        {
            clearControl();
            return;
        }
            
    }

    private void GetIssueComponent()
    {
        DataSet ds = new DataSet();
        objBb = new BaseC.clsBb(sConString);
        try
        {   
            if (ua1.CheckPermissions("E", Request.Url.AbsolutePath))
            {
                ua1.DisableEnableControl(btnSaveData, true);
            }
            else
            {
                ua1.DisableEnableControl(btnSaveData, false);
            }

            ds = objBb.GetComponentRequisition(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), common.myInt(hdnRequisition.Value.Trim()) == 0 ? -1 : common.myInt(hdnRequisition.Value.Trim()), "", 1);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataTable tab = ds.Tables[0];
                BaseC.Patient objPatient = new BaseC.Patient(sConString);
                txtPatientName.Text = common.myStr(tab.Rows[0]["PatientName"]);
                txtPatientBloodGroup.Text = common.myStr(tab.Rows[0]["PatientBloodGroup"]);
                DataTable table = GetComponentDetailsUsingRequestNo(common.myInt(hdnRequisition.Value.Trim()) == 0 ? -1 : common.myInt(hdnRequisition.Value.Trim()));
                FillChildComponents(table);
                GetAndSetIssueDetailsIfSaved(hdnCrossMatchNo.Value.ToString());
            }



        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private void GetAndSetIssueDetailsIfSaved(string p)
    {

        DataSet ds = new DataSet();
        objBb = new BaseC.clsBb(sConString);
        try
        {
            DataSet dset = objBb.GetPatientTransfusionDetail(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), hdnCrossMatchNo.Value.ToString(), common.myInt(hdnCrossMatchId.Value), 1, common.myInt(hdnComponentIssueId.Value));//objBb.GetComponentIssueDetails(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), hdnCrossMatchNo.Value.ToString(), common.myInt(hdnCrossMatchId.Value), 1, "P");//objBb.GetComponentIssueDetails(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), p, 1, "P");
            if (dset.Tables[0].Rows.Count > 0)
            {

                ViewState["ComponentIssueDetails"] = dset.Tables[0];
                DataRow row = dset.Tables[0].Rows[0];
                txtIssueNo.Text = Convert.ToString(row["CrossIssueNo"]);
                txtIssueDate.Text = Convert.ToString(row["IssueDate"]);              
                txtIssueNo.Enabled = false;                
               // txtRemarks.Text = Convert.ToString(row["Remarks"]);
            }


        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }

    }

    //public DataSet GetPatientTransfusionDetail(int HospitalLocationId, int FacilityId, string CrossMatchNo, int CrossMatchId, int Active, int ComponentIssueId)
    //{
    //    DataSet ds = new DataSet();
    //    try
    //    {
    //        Hashtable HshIn = new Hashtable();
    //        HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
    //        HshIn.Add("@intFacilityId", FacilityId);
    //        HshIn.Add("@chvCrossMatchNo", CrossMatchNo);
    //        HshIn.Add("@intCrossMatchId", CrossMatchId);
    //        HshIn.Add("@bitActive", Active);
    //        HshIn.Add("@intComponentIssueId", ComponentIssueId);
    //        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
    //        ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetPatientTransfusionDetails", HshIn);
    //    }
    //    catch (Exception Ex)
    //    {
    //        throw Ex;
    //    }
    //    return ds;
    //}


    private void FillChildComponents(DataTable table)
    {
       
        try
        {
            ddlComponent.DataSource = table;
            ddlComponent.DataValueField = "ComponentDefaultMLQty";
            ddlComponent.DataTextField = "ComponentName";
            ddlComponent.DataBind();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        
    }



    private DataTable GetComponentDetailsUsingRequestNo(int p)
    {
        DataTable dt = new DataTable();
        objBb = new BaseC.clsBb(sConString);
        return dt = objBb.GetComponentDetailsUsingRequestNo(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), p, 1).Tables[0];
    }


    private int GetDoneCrossMatchUnits(int RequestId, int ComponentId)
    {
        return 0;
    }

    //private void FillUnitNumberDropDownListInGrid()
    //{
    //    DataTable dt = new DataTable();
    //    objBb = new BaseC.clsBb(sConString);
    //    dt = objBb.GetComponentBagNo(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), Convert.ToInt32(hdnComponentID.Value), 1).Tables[0];
    //    foreach (GridDataItem item in gvData.Items)
    //    {
    //        RadComboBox box = (RadComboBox)item.FindControl("ddlUnitNumber");
    //        box.DataSource = dt;
    //        box.DataValueField = "SegmentNo";
    //        box.DataTextField = "TestStockUnitNo";
    //        box.DataBind();
    //    }

    //}

    protected void lbtnSearchCrossMatch_Click(object sender, EventArgs e)
    {
        hdnCrossMatchId.Value = "";
        RadWindowForNew.NavigateUrl = "SearchComponentIssueDetails.aspx";
        RadWindowForNew.Height = 450;
        RadWindowForNew.Width = 700;
        RadWindowForNew.Top = 200;
        RadWindowForNew.Left = 200;
        RadWindowForNew.OnClientClose = "SearchComponentIssueDetailsOnClientClose";
        RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindowForNew.Modal = true;
        RadWindowForNew.VisibleStatusbar = false;
    }

    protected void btnGetCrossMatchInfo_Click(object sender, EventArgs e)
    {
        if (hdnComponentIssueId.Value == "")
        {
            clearControl();
            return;
        }
        
        try
        {

            string CrossMatchid = hdnCrossMatchId.Value.ToString();
            string RequestId = hdnRequisition.Value.ToString();
            hdnRequisition.Value = RequestId.Trim();
            btnGetRequisitionInfo_Click(sender, e);
            DataTable dt = new DataTable();
            objBb = new BaseC.clsBb(sConString);
            ddlComponent.SelectedIndex = ddlComponent.FindItem(x => x.Value.Split(new char[] { '-' })[0].Equals(hdnComponentID.Value.ToString().Trim())).Index;
            ddlComponent.SelectedValue.Split(new char[] { '-' })[0] = hdnComponentID.Value.ToString().Trim();
            dt = objBb.GetCrossMatchedDetails(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), hdnCrossMatchNo.Value.ToString(), 1, common.myInt(hdnCrossMatchId.Value) , "C").Tables[0];      
            ddlComponent_SelectedIndexChanged(sender, new RadComboBoxSelectedIndexChangedEventArgs("", "", ddlComponent.SelectedValue + "-" + dt.Rows.Count.ToString(), ddlComponent.SelectedValue));       
            FillGridWithCrossMatchedDetails(dt);
            if (!common.myInt(hdnPatientTransfusionID.Value).Equals(0))
            FillTransfusionDetailsIfSaved();

        }
        catch (Exception ex)
        {

        }

    }

    private void FillGridWithCrossMatchedDetails(DataTable dt)
    {
        gvData.DataSource = dt;
        gvData.DataBind();


        for (int i = 0; i < dt.Rows.Count; i++)
        {
            
            Label lblUnitNumber = gvData.Items[i].FindControl("lblUnitNumber") as Label;
            lblUnitNumber.Text = dt.Rows[i]["TestStockUnitNo"].ToString();

            Label lblQty = gvData.Items[i].FindControl("lblQuantity") as Label;
            lblQty.Text = Convert.ToString(dt.Rows[i]["Qty"]);

            Label lblComponentName = gvData.Items[i].FindControl("lblComponentName") as Label;
            lblComponentName.Text = Convert.ToString(dt.Rows[i]["ComponentName"]);

            Label lblActualCrossMatchComponent_Group = gvData.Items[i].FindControl("lblActualCrossMatchComponent_Group") as Label;
            lblActualCrossMatchComponent_Group.Text = Convert.ToString(dt.Rows[i]["ActualCrossMatchComponent_Group"]);

            Label lblBloodGroup = gvData.Items[i].FindControl("lblBloodGroup") as Label;
            lblBloodGroup.Text = Convert.ToString(dt.Rows[i]["BloodGroup"]);

            txtPatientBloodGroup.Text = lblActualCrossMatchComponent_Group.Text;

            HiddenField ComponentID = gvData.Items[i].FindControl("hdnComponentID") as HiddenField;
            ComponentID.Value = Convert.ToString(dt.Rows[i]["CrossMatchComponent"]);

              HiddenField BloodGroupID = gvData.Items[i].FindControl("hdnBloodGroupID") as HiddenField;
              BloodGroupID.Value = Convert.ToString(dt.Rows[i]["PatientBloodGroupId"]);


              hdnPatientBloodGroupID.Value = BloodGroupID.Value;


        }
    }

    private void FillTransfusionDetailsIfSaved()
    {
        DataSet ds = new DataSet();
        try
        {
            objBb = new BaseC.clsBb(sConString);
            ds = GetPatientTransfusionDetails(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), hdnEncounterNo.Value.ToString(), hdnRegistrationNo.Value, common.myInt(hdnPatientTransfusionID.Value)); //objBb.GetPatientTransfusionDetails(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), hdnEncounterNo.Value.ToString(), Convert.ToInt32(hdnRegistrationNo.Value));
            if(ds.Tables.Count==2)
            {
            DataTable PatientTransfusionMain = ds.Tables[0];
            DataTable PatientTransfusionDetails = ds.Tables[1];
            if(PatientTransfusionMain.Rows.Count>0)
            {
               DataRow row= PatientTransfusionMain.Rows[0];
               txtRemarks.Text = row["Remarks"].ToString();
               hdnPatientTransfusionID.Value = common.myStr(row["TransfusionID"].ToString());
            }
            if(PatientTransfusionDetails.Rows.Count>0)
            {
                //gvData.DataSource = PatientTransfusionDetails;
                //gvData.DataBind();
                for (int i = 0; i < PatientTransfusionDetails.Rows.Count; i++)
                {
                    DataRow row = PatientTransfusionDetails.Rows[i];
                    chkEnsureTransfusion.Checked = Convert.ToInt32(row["TransfusionQ1"])==1 ? true : false;
                    chkUnitsReceived.Checked=  Convert.ToInt32(row["TransfusionQ2"])==1 ? true : false;
                    chkBaseLineVitals.Checked= Convert.ToInt32(row["TransfusionQ3"])==1 ? true: false;
                    chkUnitsChecked.Checked= Convert.ToInt32(row["TransfusionQ4"])==1 ? true :false;

                    //RadComboBox ddlTransfusion = gvData.Items[i].FindControl("ddlTransfusionStart") as RadComboBox;
                    RadComboBox ddlTransfusion = gvData.Items[i].FindControl("ddlTransfusionStart") as RadComboBox;
                    RadTimePicker startTime= gvData.Items[i].FindControl("dtpStartTime") as RadTimePicker;
                    RadTimePicker endTime = gvData.Items[i].FindControl("dtpEndTime") as RadTimePicker;
                    RadTextBox reaction = gvData.Items[i].FindControl("txtReaction") as RadTextBox;
                    RadTextBox reason = gvData.Items[i].FindControl("txtReason") as RadTextBox;

                    ddlTransfusion.SelectedValue = Convert.ToInt32(row["BloodTransfuse"]).ToString();

                    ddlTransfusion.Enabled = Convert.ToInt32(row["BloodTransfuse"]) == 1 ? false : true;

                    DateTime? StartTime = row["StartDatetime"].ToString() == "" ? (DateTime?)null : Convert.ToDateTime(row["StartDatetime"]);
                    DateTime? EndTime = row["EndDatetime"].ToString() == "" ? (DateTime?)null : Convert.ToDateTime(row["EndDatetime"]);

                    startTime.SelectedDate = row["StartDatetime"].ToString() == "" ? StartTime : Convert.ToDateTime(row["StartDatetime"]);
                    if (!(row["EndDatetime"].ToString().Equals("1/1/1900 12:00:00 AM")))
                    {
                        endTime.SelectedDate = row["EndDatetime"].ToString() == "" ? EndTime : Convert.ToDateTime(row["EndDatetime"]);
                        endTime.Enabled = false;
                    }
                    else
                    {
                        endTime.SelectedDate = null;
                        endTime.Enabled = true;

                    }

                    string tt =Convert.ToDateTime(row["StartDatetime"]).ToString("yyyy/MM/dd").ToString();

                    //if (StartTime != null)
                    //{
                    //    startTime.Enabled = false;
                    //}

                    //if (EndTime!= null)
                    //{
                    //    endTime.Enabled = false;
                    //}

                    reaction.Text = row["Reaction"].ToString();
                    reason.Text = row["Reason"].ToString();
                    
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
            ds.Dispose();
        }
    }

    public DataSet GetPatientTransfusionDetails(int HospitalLocationId, int FacilityId, string EncounterNo, string RegistrationNo,int TransfusionID)
    {
        DataSet ds = new DataSet();
        try
        {
            Hashtable HshIn = new Hashtable();

            HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@chvEncounterNo", EncounterNo);
            HshIn.Add("@intRegistrationNo", RegistrationNo);
            HshIn.Add("@intTransfusionID", TransfusionID);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetTransfusionDetails", HshIn);
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
        return ds;
    }

    protected void ddlComponent_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        try
        {
            string[] hold = e.Value.Split(new char[] { '-' });
            if (hold.Length >= 3)
            {
              hdnComponentID.Value = hold[0];
              // txtAcknowledgeUnits.Text = hold[2];
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        FillCrossCheckedInformationIfDone(common.myInt(hdnRequisition.Value.Trim()));
    }


    private void FillCrossCheckedInformationIfDone(int p)
    {
     
        DataSet ds = new DataSet();
        objBb = new BaseC.clsBb(sConString);
        try
        {
            string ComponentId = hdnComponentID.Value.ToString();
            int FinalComponentId = 0;
            if (!ComponentId.Equals(""))
            {
                FinalComponentId = Convert.ToInt32(ComponentId);
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
         

    }

    protected void lbtnSearchrBloodTransfusion_Click(object sender, EventArgs e)
    {
        hdnComponentIssueNo.Value = "";
        hdnEncounterNo.Value = "";
        hdnRequisition.Value = "";
        hdnRequisition.Value = "";

        RadWindowForNew.NavigateUrl = "BloodTransfusionList.aspx?Regid=" + hdnRegistrationID.Value
                       + "&RegNo=" + common.myStr(hdnRegistrationNo.Value)
                        + "&EncId=" + common.myInt(hdnEncounterID.Value)
                        + "&EncNo=" + common.myStr(hdnEncounterNo.Value)
                        + "&Status=1";
        RadWindowForNew.Height = 450;
        RadWindowForNew.Width = 700;
        RadWindowForNew.Top = 200;
        RadWindowForNew.Left = 200;
        RadWindowForNew.OnClientClose = "SearchComponentIssueDetailsOnClientClose";
        RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindowForNew.Modal = true;
        RadWindowForNew.VisibleStatusbar = false;
    }

    protected void gvData_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
        {
            if (common.myInt(hdnPatientTransfusionID.Value) == 0)
            {
                if ((e.Item.FindControl("ddlTransfusionStart") as RadComboBox).SelectedValue.Equals("1"))
                {
                    (e.Item.FindControl("dtpStartTime") as RadTimePicker).Enabled = true;
                    (e.Item.FindControl("dtpEndTime") as RadTimePicker).Enabled = false;
                }
                else
                {
                    (e.Item.FindControl("dtpStartTime") as RadTimePicker).Enabled = false;
                    (e.Item.FindControl("dtpEndTime") as RadTimePicker).Enabled = false;
                }
            }
            else
            {
                (e.Item.FindControl("dtpStartTime") as RadTimePicker).Enabled = false;
                (e.Item.FindControl("dtpEndTime") as RadTimePicker).Enabled = true;
            }
        }
    }
}

