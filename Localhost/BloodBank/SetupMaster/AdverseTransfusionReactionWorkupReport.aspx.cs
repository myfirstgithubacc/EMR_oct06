using System;
using System.Data;
using System.Web.UI;
using System.Configuration;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Text;
using System.Collections;

public partial class BloodBank_SetupMaster_AdverseTransfusionReactionWorkupReport : System.Web.UI.Page
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
    public DataSet GetAdverseTransfusionParameterDetails()
    {
        DataSet ds = new DataSet();
        try
        {
            Hashtable HshIn = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            ds = objDl.FillDataSet(CommandType.StoredProcedure, "USPGetAdverseTransfusionParameterDetails", HshIn);

        }
        catch (Exception Ex)
        {
            throw Ex;
        }
        return ds;

    }
    private void BindGrid()
    {
        try
        {
            objBb = new BaseC.clsBb(sConString);
            //DataSet dset = objBb.GetAdverseTransfusionParameterDetails();
            DataSet dset = GetAdverseTransfusionParameterDetails();
            DataTable tab = dset.Tables[0];
            gvDetails.DataSource = tab;
            gvDetails.DataBind();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
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
                //dtpReturnDate.SelectedDate = DateTime.Now;
                BindGrid();
                bindBloodGroup();
                //string strRegId = common.myStr(Request.QueryString["Regid"]);
                //if (strRegId != "")
                //{
                //    rbtnreturn.Enabled = false;
                //    rbtnreturn.SelectedValue = "1";

                //}
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

            //if (ddlReturnBy.SelectedIndex == -1)
            //{
            //    Alert.ShowAjaxMsg("please select ReturnBy", Page);
            //    ddlReturnBy.Focus();
            //    return;
            //}
            //if (common.myStr(txtReason.Text.Trim()) == "")
            //{
            //    Alert.ShowAjaxMsg("please enter commponent Return reason !", Page);
            //    txtReason.Focus();
            //    return;
            //}

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


            #region Physical Examination List Field

            StringBuilder strXML = new StringBuilder();
            ArrayList coll = new ArrayList();

            if (gvDetails != null)
            {

                foreach (GridDataItem dataItem in gvDetails.Items)
                {
                    HiddenField chkRow = (HiddenField)dataItem.FindControl("hdnRenderControl");
                    RadTextBox parameterValueTextBox = null;
                    RadComboBox parameterValueCombo = null;

                    switch (chkRow.Value.Trim())
                    {
                        case "T":
                            parameterValueTextBox = dataItem.FindControl("radTxt") as RadTextBox;
                            break;
                        case "D":
                            parameterValueCombo = dataItem.FindControl("radCmb") as RadComboBox;
                            break;
                    }
                    // if( (parameterValueTextBox!=null && parameterValueTextBox.Text.Trim().Length>0) || (parameterValueCombo!=null && parameterValueCombo.SelectedIndex>=0) )
                    // {
                    HiddenField parameterCode = (HiddenField)dataItem.FindControl("hdnParameter_Code");
                    coll.Add(parameterCode.Value.ToString().Trim());
                    coll.Add(parameterValueTextBox != null ? parameterValueTextBox.Text.Trim() : parameterValueCombo.SelectedValue.ToString());                
                    coll.Add("A");
                    strXML.Append(common.setXmlTable(ref coll));
                    //  }


                }

            }

            #endregion
            //common.myInt(hdnDonorRegistrationId.Value)
            // string strMsg = objBb.SavePhysicalExaminationPatientDetails(common.myInt(ViewState["_ID"]), common.myInt(Session["HospitalLocationID"]),
            //    common.myInt(Session["FacilityId"]), common.myStr(txtDonorRegNo.Text.Trim()), common.myStr(RadioButtonList1.SelectedValue.ToString().Trim()), 1,
            //  txtRemarks.Text.Trim(), common.myInt(ddlStatus.SelectedValue), common.myInt(Session["UserID"]),strXML.ToString());
            /*
            string acceptedRejected = string.Empty;
            string deferredType = string.Empty;

            if (RadioButtonList1.SelectedIndex == 0)
            {
                acceptedRejected = common.myStr(RadioButtonList1.SelectedValue.ToString().Trim());
            }
            else
            {
                acceptedRejected = common.myStr(RadioButtonList1.SelectedValue.ToString().Trim());
                deferredType = common.myStr(RadioButtonList2.SelectedValue.ToString().Trim());
            }

            */
            //string strMsg = objBb.SavePhysicalExaminationPatientDetails(common.myInt(ViewState["_ID"]), common.myInt(Session["HospitalLocationID"]),
            //             common.myInt(Session["FacilityId"]), hdnDonorRegistrationId.Value.ToString(), acceptedRejected, 1,
            //             txtRemarks.Text.Trim(), common.myInt(ddlStatus.SelectedValue), common.myInt(Session["UserID"]), strXML.ToString(), common.myInt(ddlBloodGroup.SelectedValue), deferredType, common.myInt(ddlDeferReason.SelectedValue));


            string strMsg = SaveAdverseTransfusionReactionWorkupReport(common.myInt(hdnIssue.Value), common.myInt(Session["HospitalLocationID"]),
                         common.myInt(Session["FacilityId"]), common.myInt(ddlBloodGroup1.SelectedValue), common.myInt(ddlBloodGroup2.SelectedValue)
                         , ddlMinor.SelectedValue, txtConclusion.Text.Trim(), 1, common.myInt(Session["UserID"]), strXML.ToString()
                         , txtPR4Saline.Text.Trim(), txtPR22Saline.Text.Trim(), txtPR37Enzyme.Text.Trim(), txtPR37Albumin.Text.Trim()
                         , txtPO4Saline.Text.Trim(), txtPO22Saline.Text.Trim(), txtPO37Enzyme.Text.Trim(), txtPO37Albumin.Text.Trim());

                         

        if ((strMsg.ToUpper().Contains(" UPDATE") || strMsg.ToUpper().Contains(" SAVE")))
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
             
                clearControl();
                ViewState["_ID"] = "0";
                lblMessage.Text = strMsg;


            }
            else
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Error: " + strMsg;
            }
           
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }


    public String SaveAdverseTransfusionReactionWorkupReport(int IssueId, int HospitalLocationId, int FacilityId, int chvPrestTranBloodGroup
        , int chvPostTranBloodGroup, string chvCompatibilitymonor, string chvConclusion, int Active, int UserId, string ParameterDetails,string  PR4Saline, string PR22Saline, string PR37Enzyme, string PR37Albumin
        , string PO4Saline, string PO22Saline, string PO37Enzyme, string PO37Albumin)        
    {
        Hashtable HshIn = new Hashtable();
        Hashtable HshOut = new Hashtable();

        HshIn.Add("@intIssueId", IssueId);
        HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
        HshIn.Add("@intFacilityId", FacilityId);
        HshIn.Add("@chvPrestTranBloodGroup", chvPrestTranBloodGroup);
        HshIn.Add("@chvPostTranBloodGroup", chvPostTranBloodGroup);
        HshIn.Add("@chvCompatibilitymonor", chvCompatibilitymonor);
        HshIn.Add("@chvConclusion", chvConclusion);
        HshIn.Add("@bitActive", Active);
        HshIn.Add("@intEncodedBy", UserId);
        HshIn.Add("@xmlParameterDetails", ParameterDetails);

        HshIn.Add("@txtPR4Saline", PR4Saline);
        HshIn.Add("@txtPR22Saline", PR22Saline);
        HshIn.Add("@txtPR37Enzyme", PR37Enzyme);
        HshIn.Add("@txtPR37Albumin", PR37Albumin);
        HshIn.Add("@txtPO4Saline", PO4Saline);
        HshIn.Add("@txtPO22Saline", PO22Saline);
        HshIn.Add("@txtPO37Enzyme", PO37Enzyme);
        HshIn.Add("@txtPO37Albumin", PO37Albumin);

        
        HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspBBSaveAdverseTransfusionReactionWorkupReport", HshIn, HshOut);

        return HshOut["@chvErrorStatus"].ToString();
    }
    //private void SaveData()
    //{

    //try
    //{
    //    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
    //    lblMessage.Text = string.Empty;
    //    objBb = new BaseC.clsBb(sConString);

    //    StringBuilder build = new StringBuilder();
    //    string strMsg = "";


    //    strXML = new StringBuilder();
    //    coll = new ArrayList();

    //    if (gvData != null)
    //    {

    //        foreach (GridDataItem item in gvData.Items)
    //        {
    //            if ((item.FindControl("chbReturn") as CheckBox).Enabled)
    //            {
    //                if ((item.FindControl("chbReturn") as CheckBox).Checked)
    //                {
    //                    coll.Add(((item.FindControl("ddlUnitNumber") as RadComboBox).SelectedItem.Text.Trim()));
    //                    coll.Add((item.FindControl("lblQuantity") as Label).Text.Trim());
    //                    RadDatePicker crossMatchedDate = (item.FindControl("dtpCrossMatchDate") as RadDatePicker);
    //                    coll.Add(Convert.ToString(crossMatchedDate.SelectedDate.Value));
    //                    coll.Add((item.FindControl("chbIssued") as CheckBox).Checked);
    //                    coll.Add((item.FindControl("chbReturn") as CheckBox).Checked);
    //                    strXML.Append(common.setXmlTable(ref coll));
    //                }
    //                else
    //                {
    //                    Alert.ShowAjaxMsg("please select component...", Page);
    //                    return;
    //                }

    //            }
    //        }
    //    }


    //    int CrossMatchedBy = 1;

    //    if (strXML.ToString() != "")
    //    {
    //        strMsg = objBb.SaveComponetReturnDetails(common.myInt(hdnReturnId.Value), common.myInt(Session["HospitalLocationID"]),
    //            common.myInt(Session["FacilityId"]), dtpReturnDate.SelectedDate.Value.ToString(),
    //            Convert.ToString(hdnComponentIssueNo.Value.ToString()), 1, common.myInt(ddlReturnBy.SelectedValue),
    //            txtReason.Text.Trim(), txtRemarks.Text.Trim(), common.myInt(Session["UserID"]), strXML.ToString(),
    //            common.myInt(hdnCrossMatchId.Value), common.myStr(txtDiscardQuantity.Text));
    //        //if(rbtnreturn.SelectedValue=="1")
    //        //    strMsg = objBb.SaveComponetReturnDetails(common.myInt(hdnReturnId.Value), common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), dtpReturnDate.SelectedDate.Value.ToString(), Convert.ToString(hdnComponentIssueNo.Value.ToString()), 1, common.myInt(ddlReturnBy.SelectedValue), txtReason.Text.Trim(), txtRemarks.Text.Trim(), common.myInt(Session["UserID"]), strXML.ToString(), common.myInt(hdnCrossMatchId.Value));
    //        //else if(rbtnreturn.SelectedValue=="2")
    //        //    strMsg = objBb.SaveComponetReturnDetails(common.myInt(hdnReturnId.Value), common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), dtpReturnDate.SelectedDate.Value.ToString(), "", 1, common.myInt(ddlReturnBy.SelectedValue), txtReason.Text.Trim(), txtRemarks.Text.Trim(), common.myInt(Session["UserID"]), strXML.ToString(), common.myInt(hdnCrossMatchId.Value));
    //    }

    //    else
    //        lblMessage.Text = "Can't save";


    //    if ((strMsg.ToUpper().Contains(" UPDATE") || strMsg.ToUpper().Contains(" SAVE")))
    //    {
    //        //txtIssueNo.Text = strMsg.Split(new char[] { '-' })[1].ToString();
    //        //strMsg = strMsg.Split(new char[] { '-' })[0].ToString();
    //        //strMsg += " Issue no. is " + txtIssueNo.Text.Trim();
    //        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
    //        clearControl();
    //        ViewState["_ID"] = "0";
    //        SetPermission();

    //    }
    //    lblMessage.Text = strMsg;
    //}
    //catch (Exception Ex)
    //{
    //    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
    //    lblMessage.Text = "Error: " + Ex.Message;
    //    objException.HandleException(Ex);
    //}
    //}


    private void clearControl()
    {
        txtUHID.Text = string.Empty;
        txtIPNo.Text = string.Empty;
        txtPatientName.Text = string.Empty;
        txtWardNo.Text = string.Empty;
        txtBedNo.Text = string.Empty;
        //txtPatientBloodGroup.Text = string.Empty;
        //txtMotherBloodGroup.Text = string.Empty;
        //txtRemarks.Text = string.Empty;

        txtIssueNo.Text = string.Empty;
        //txtRemarks.Text = string.Empty;
        //txtReason.Text = string.Empty;
        //txtDiscardQuantity.Text = string.Empty;
        //txtManualNo.Text = string.Empty;
        txtIssueDate.Text = string.Empty;

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


        RadWindowForNew.NavigateUrl = "ComponentReturnList.aspx?Status=T"
                       + "&Regid=" + Request.QueryString["Regid"]
                       + "&RegNo=" + common.myStr(Request.QueryString["RegNo"])
                       + "&EncId=" + Request.QueryString["EncId"]
                       + "&EncNo=" + common.myStr(Request.QueryString["EncNo"]);

        RadWindowForNew.Height = 450;
        RadWindowForNew.Width = 900;
        RadWindowForNew.Top = 200;
        RadWindowForNew.Left = 200;
        RadWindowForNew.OnClientClose = "SearchComponentIssueDetailsOnClientClose";
        RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindowForNew.Modal = true;
        RadWindowForNew.VisibleStatusbar = false;


    }


    protected void btnGetRequisitionInfo_Click(object sender, EventArgs e)
    {
        if (hdnCrossMatchId.Value == "")
        {
            clearControl();
            return;
        }

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



            ds = objBb.GetComponentReturnDetail(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), common.myStr(hdnComponentIssueNo.Value), 1, common.myStr(Request.QueryString["RegNo"]), common.myStr(Request.QueryString["Regid"]), common.myStr(Request.QueryString["EncounterNo"]), common.myStr(Request.QueryString["EncId"]), 1, common.myInt(hdnReturnId.Value));//objBb.GetComponentRequisition(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), common.myInt(hdnRequisition.Value.Trim()) == 0 ? -1 : common.myInt(hdnRequisition.Value.Trim()), "", 1);

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataTable tab = ds.Tables[0];
                BaseC.Patient objPatient = new BaseC.Patient(sConString);
                //if (rbtnreturn.SelectedValue == "1")
                //{
                //    txtIssueDate.Text = common.myDate(tab.Rows[0]["IssueDate"]).ToString();
                //    txtIssueNo.Text = common.myStr(tab.Rows[0]["CrossIssueNo"]);
                //}
                //else
                //{
                //    txtIssueNo.Text = common.myStr(tab.Rows[0]["CrossMatchNo"]);
                //    txtIssueDate.Text = common.myDate(tab.Rows[0]["CrossDate"]).ToString();

                //}

                txtIssueDate.Text = common.myDate(tab.Rows[0]["IssueDate"]).ToString();
                txtIssueNo.Text = common.myStr(tab.Rows[0]["CrossIssueNo"]);
                hdnIssue.Value = common.myStr(tab.Rows[0]["ComponentIssueId"]);
                txtPatientName.Text = common.myStr(tab.Rows[0]["PatientName"]);
                txtIPNo.Text = common.myStr(tab.Rows[0]["EncounterNo"].ToString().Trim());
                txtUHID.Text = common.myStr(tab.Rows[0]["RegistrationNo"].ToString().Trim());
                //txtPatientBloodGroup.Text = Convert.ToString(tab.Rows[0]["PatientBloodGroup"]);
                //txtMotherBloodGroup.Text = Convert.ToString(tab.Rows[0]["MotherBloodGroup"]);
                //txtRemarks.Text = Convert.ToString(tab.Rows[0]["Remarks"]);
                //txtReason.Text = common.myStr(tab.Rows[0]["Reason"]);
                //txtDiscardQuantity.Text = common.myStr(tab.Rows[0]["DiscartQuantity"]);
                txtWardNo.Text = Convert.ToString(tab.Rows[0]["Ward"]);
                txtBedNo.Text = Convert.ToString(tab.Rows[0]["BedNo"]);
                txtUHID.Text = common.myStr(tab.Rows[0]["RegistrationNo"]);
                txtUnitstowhichTrasfusionReactionOccured.Text = common.myStr(tab.Rows[0]["BagNo"]);
                txtVolumeTransfused.Text = common.myStr(tab.Rows[0]["Qty"]);
                ddlBloodGroup1.SelectedValue= common.myStr(tab.Rows[0]["ActualCrossMatchComponent_Group"]);
                txtGender.Text = common.myStr(tab.Rows[0]["Gender"]);
                txtYear.Text = Convert.ToString(tab.Rows[0]["AgeYear"]);
                txtMonth.Text = Convert.ToString((tab.Rows[0]["AgeMonth"]));
                txtDays.Text = Convert.ToString(tab.Rows[0]["AgeDay"]);
                //txtManualNo.Text = common.myStr(tab.Rows[0]["BloodCenterReceivingNo"]);
                //dtpReturnDate.SelectedDate = common.myDate(tab.Rows[0]["RreturnDate"]);
                FillGridWithCrossMatchedDetails(tab);


            }


        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    //public DataSet GetComponentReturnDetail(int HospitalLocationId, int FacilityId, string ComponentIssueNo, int Active, string RegistrationNo, string RegistrationID, string EncounterNo, string EncounterID, int Acknowledged, int ReturnId)
    //{
    //    DataSet ds = new DataSet();
    //    try
    //    {
    //        Hashtable HshIn = new Hashtable();
    //        HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
    //        HshIn.Add("@intFacilityId", FacilityId);
    //        HshIn.Add("@chvComponentIssueNo", ComponentIssueNo);
    //        HshIn.Add("@bitActive", Active);
    //        HshIn.Add("@intRegistrationNo", RegistrationNo);
    //        HshIn.Add("@intRegistrationID", RegistrationID);
    //        HshIn.Add("@intEncounterNo", EncounterNo);
    //        HshIn.Add("@intEncounterID", EncounterID);
    //        HshIn.Add("@intAcknowledged", Acknowledged);
    //        HshIn.Add("@intReturnId", ReturnId);
    //        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
    //        ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetComponentReturnDetail", HshIn);
    //    }
    //    catch (Exception Ex)
    //    {
    //        throw Ex;
    //    }
    //    return ds;
    //}

    public DataSet GetComponentCrossMatchReturnDetail(int HospitalLocationId, int FacilityId, int Active, string RegistrationNo, string RegistrationID, string EncounterNo, string EncounterID, int ReturnId, int CrossMatchId)
    {
        DataSet ds = new DataSet();
        try
        {
            Hashtable HshIn = new Hashtable();
            HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intFacilityId", FacilityId);
            //HshIn.Add("@chvComponentIssueNo", ComponentIssueNo);
            HshIn.Add("@bitActive", Active);
            HshIn.Add("@intRegistrationNo", RegistrationNo);
            HshIn.Add("@intRegistrationID", RegistrationID);
            HshIn.Add("@intEncounterNo", EncounterNo);
            HshIn.Add("@intEncounterID", EncounterID);
            HshIn.Add("@intCrossMatchId", CrossMatchId);
            HshIn.Add("@intReturnId", ReturnId);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetComponentCrossMatchReturnDetail", HshIn);
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
        return ds;
    }

    //private void GetAndSetIssueDetailsIfSaved(string p)
    //{

    //    DataSet ds = new DataSet();
    //    objBb = new BaseC.clsBb(sConString);
    //    try
    //    {            
    //        DataSet dset = objBb.GetComponentIssueDetails(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), p, 1, "P");
    //        if (dset.Tables[0].Rows.Count > 0)
    //        {

    //            ViewState["ComponentIssueDetails"] = dset.Tables[0];
    //            DataRow row = dset.Tables[0].Rows[0];
    //            txtIssueNo.Text = Convert.ToString(row["CrossIssueNo"]);
    //            txtIssueDate.Text = Convert.ToString(row["IssueDate"]);
    //            txtManualNo.Text = Convert.ToString(row["BloodCenterReceivingNo"]);
    //            txtIssueNo.Enabled = false;                

    //        }


    //    }
    //    catch (Exception Ex)
    //    {
    //        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
    //        lblMessage.Text = "Error: " + Ex.Message;
    //        objException.HandleException(Ex);
    //    }

    //}


    //private void FillChildComponents(DataTable table)
    //{

    //    try
    //    {
    //        ddlComponent.DataSource = table;
    //        ddlComponent.DataValueField = "ComponentDefaultMLQty";
    //        ddlComponent.DataTextField = "ComponentName";
    //        ddlComponent.DataBind();
    //    }
    //    catch (Exception Ex)
    //    {
    //        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
    //        lblMessage.Text = "Error: " + Ex.Message;
    //        objException.HandleException(Ex);
    //    }

    //}



    //private DataTable GetComponentDetailsUsingRequestNo(int p)
    //{
    //    DataTable dt = new DataTable();
    //    objBb = new BaseC.clsBb(sConString);
    //    return dt = objBb.GetComponentDetailsUsingRequestNo(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), p, 1).Tables[0];
    //}





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
        hdnIssue.Value = "";
        //RadWindowForNew.NavigateUrl = "SearchComponentIssueDetails.aspx";
        RadWindowForNew.NavigateUrl = "ComponentReturnList.aspx?Status=P"
                       + "&Regid=" + common.myStr(Request.QueryString["Regid"])
                       + "&RegNo=" + common.myStr(Request.QueryString["RegNo"])
                       + "&EncId=" + common.myInt(Request.QueryString["EncId"])
                       + "&EncNo=" + common.myStr(Request.QueryString["EncounterNo"]);
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
        try
        {
            string CrossMatchid = hdnCrossMatchId.Value.ToString();
            string RequestId = hdnRequisition.Value.ToString();
            string Issue = hdnIssue.Value.ToString();


            hdnRequisition.Value = RequestId.Trim();
            btnGetRequisitionInfo_Click(sender, e);
        }
        catch (Exception ex)
        {

        }

    }

    private void FillGridWithCrossMatchedDetails(DataTable dt)
    {
    }



    protected void gvDetails_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
        {
            //hdnIdentification
            //    hdnParameter_Code

            switch ((e.Item.FindControl("hdnRenderControl") as HiddenField).Value.Trim())
            {
                case "T":
                    ((e.Item.FindControl("radTxt")) as RadTextBox).Visible = true;
                    ((e.Item.FindControl("radCmb")) as RadComboBox).Visible = false;
                    break;
                case "D":
                    ((e.Item.FindControl("radTxt")) as RadTextBox).Visible = false;
                    ((e.Item.FindControl("radCmb")) as RadComboBox).Visible = true;

                    string Parameter_Code = common.myStr((e.Item.FindControl("hdnParameter_Code") as HiddenField).Value);


                    ParameterCodeValue(((e.Item.FindControl("radCmb")) as RadComboBox), Parameter_Code);

                    break;
            }


        }
    }
    
    public DataSet ParameterCodeValue(RadComboBox ddl, string option)
    {
        DataSet ds = new DataSet();
        objBb = new BaseC.clsBb(sConString);
        try
        {
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            //HshIn.Add("@Parameter_Code", option);
            ds = objBb.ParameterCodeValue(option, 0, common.myInt(Session["HospitalLocationId"])
                , common.myInt(Session["FacilityId"])
                , common.myInt(Session["UserId"]));
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl.DataSource = ds;
                ddl.DataValueField = "Parameter_Values_Id";
                ddl.DataTextField = "Parameter_Values";
                ddl.DataBind();
            }

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        return ds;
    }
    private void bindBloodGroup()
    {
        DataSet ds = new DataSet();
        try
        {
            objBb = new BaseC.clsBb(sConString);
            ds = objBb.GetBloodGroupMaster(0, common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), 1);

            ddlBloodGroup1.DataSource = ds.Tables[0];
            ddlBloodGroup1.DataValueField = "BloodGroupId";
            ddlBloodGroup1.DataTextField = "BloodGroupDescription";
            ddlBloodGroup1.DataBind();

            ddlBloodGroup1.Items.Insert(0, new RadComboBoxItem("", "0"));


            ddlBloodGroup2.DataSource = ds.Tables[0];
            ddlBloodGroup2.DataValueField = "BloodGroupId";
            ddlBloodGroup2.DataTextField = "BloodGroupDescription";
            ddlBloodGroup2.DataBind();

            ddlBloodGroup2.Items.Insert(0, new RadComboBoxItem("", "0"));

            //ddlBloodGroupWhom.DataSource = ds.Tables[0];
            //ddlBloodGroupWhom.DataValueField = "BloodGroupId";
            //ddlBloodGroupWhom.DataTextField = "BloodGroupDescription";
            //ddlBloodGroupWhom.DataBind();

            //ddlBloodGroupWhom.Items.Insert(0, new RadComboBoxItem("", "0"));



        }
        catch (Exception)
        {

            throw;
        }
        finally
        {
            ds.Dispose();
        }
    }
}

