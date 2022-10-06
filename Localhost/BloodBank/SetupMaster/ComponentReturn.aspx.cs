using System;
using System.Data;
using System.Web.UI;
using System.Configuration;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Text;
using System.Collections;

public partial class BloodBank_SetupMaster_ComponentReturn : System.Web.UI.Page
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
                dtpReturnDate.SelectedDate = DateTime.Now;
                gvData.DataSource = string.Empty;
                gvData.DataBind();

                GetEmployeeReturnBy();

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
            hdnIsPasswordRequired.Value = common.myStr(Request.QueryString["IsPasswordRequired"]);
        }
    }

    public void GetEmployeeReturnBy()
    {
        DataSet dsEmployee = objBb.GetEmployeeReturnBy(common.myInt(Session["HospitalLocationID"]));
        if (dsEmployee.Tables[0].Rows.Count > 0)
        {
            ddlReturnBy.DataSource = dsEmployee;
            ddlReturnBy.DataTextField = "EmployeeName";
            ddlReturnBy.DataValueField = "EmployeeId";
            ddlReturnBy.DataBind();
        }
        ddlReturnBy.Items.Insert(0, new RadComboBoxItem("[Select]", "0"));


        ddlReturnBy.SelectedIndex = ddlReturnBy.Items.IndexOf(ddlReturnBy.Items.FindItemByValue(Session["EmployeeId"].ToString()));

    }

    protected void btnNew_OnClick(object sender, EventArgs e)
    {
        Response.Redirect(common.myStr(Request.Url.AbsoluteUri), false);
    }

    protected void btnSaveData_OnClick(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {

            if (ddlReturnBy.SelectedIndex == -1)
            {
                Alert.ShowAjaxMsg("please select ReturnBy", Page);
                ddlReturnBy.Focus();
                return;
            }
            if (common.myStr(txtReason.Text.Trim()) == "")
            {
                Alert.ShowAjaxMsg("please enter commponent Return reason !", Page);
                txtReason.Focus();
                return;
            }
            if (common.myBool(hdnIsPasswordRequired.Value))
            {
                IsValidPassword();
                return;
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
            int CountCompompentChkd = 0;
            if (gvData != null)
            {

                foreach (GridDataItem item in gvData.Items)
                {
                    if ((item.FindControl("chbReturn") as CheckBox).Enabled)
                    {

                        if ((item.FindControl("chbReturn") as CheckBox).Checked)
                        {
                            coll.Add(((item.FindControl("ddlUnitNumber") as RadComboBox).SelectedItem.Text.Trim()));
                            coll.Add((item.FindControl("lblQuantity") as Label).Text.Trim());
                            RadDatePicker crossMatchedDate = (item.FindControl("dtpCrossMatchDate") as RadDatePicker);
                            coll.Add(Convert.ToString(crossMatchedDate.SelectedDate.Value));
                            coll.Add((item.FindControl("chbIssued") as CheckBox).Checked);
                            coll.Add((item.FindControl("chbReturn") as CheckBox).Checked);
                            strXML.Append(common.setXmlTable(ref coll));
                            CountCompompentChkd++;
                        }
                        else
                        {
                            if (CountCompompentChkd == 0)
                            {
                                Alert.ShowAjaxMsg("please select component...", Page);
                                return;
                            }
                        }

                    }
                }
            }

            int CrossMatchedBy = 1;

            if (strXML.ToString() != "")
            {
                strMsg = objBb.SaveComponetReturnDetails(common.myInt(hdnReturnId.Value), common.myInt(Session["HospitalLocationID"]),
                    common.myInt(Session["FacilityId"]), dtpReturnDate.SelectedDate.Value.ToString(),
                    Convert.ToString(hdnComponentIssueNo.Value.ToString()), 1, common.myInt(ddlReturnBy.SelectedValue),
                    txtReason.Text.Trim(), txtRemarks.Text.Trim(),
                    common.myBool(hdnIsPasswordRequired.Value) ? common.myInt(PasswordRequiredHelper.GetTransactionUserId()) : common.myInt(Session["UserId"]), // To swapping between current userid & transaction userid
                    strXML.ToString(),
                    common.myInt(hdnCrossMatchId.Value), common.myStr(txtDiscardQuantity.Text));
                //if(rbtnreturn.SelectedValue=="1")
                //    strMsg = objBb.SaveComponetReturnDetails(common.myInt(hdnReturnId.Value), common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), dtpReturnDate.SelectedDate.Value.ToString(), Convert.ToString(hdnComponentIssueNo.Value.ToString()), 1, common.myInt(ddlReturnBy.SelectedValue), txtReason.Text.Trim(), txtRemarks.Text.Trim(), common.myInt(Session["UserID"]), strXML.ToString(), common.myInt(hdnCrossMatchId.Value));
                //else if(rbtnreturn.SelectedValue=="2")
                //    strMsg = objBb.SaveComponetReturnDetails(common.myInt(hdnReturnId.Value), common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), dtpReturnDate.SelectedDate.Value.ToString(), "", 1, common.myInt(ddlReturnBy.SelectedValue), txtReason.Text.Trim(), txtRemarks.Text.Trim(), common.myInt(Session["UserID"]), strXML.ToString(), common.myInt(hdnCrossMatchId.Value));
                if ((strMsg.ToUpper().Contains(" UPDATE") || strMsg.ToUpper().Contains(" SAVE")))
                {
                    //txtIssueNo.Text = strMsg.Split(new char[] { '-' })[1].ToString();
                    //strMsg = strMsg.Split(new char[] { '-' })[0].ToString();
                    //strMsg += " Issue no. is " + txtIssueNo.Text.Trim();
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                    clearControl();
                    ViewState["_ID"] = "0";
                    SetPermission();

                }
                lblMessage.Text = strMsg;
            }

            else
            {
                lblMessage.Text = "Please validate the page!";
            }




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
        txtUHID.Text = string.Empty;
        txtIPNo.Text = string.Empty;
        txtPatientName.Text = string.Empty;
        txtWardNo.Text = string.Empty;
        txtBedNo.Text = string.Empty;
        txtPatientBloodGroup.Text = string.Empty;
        txtMotherBloodGroup.Text = string.Empty;
        txtRemarks.Text = string.Empty;
        gvData.DataSource = string.Empty;
        txtIssueNo.Text = string.Empty;
        txtRemarks.Text = string.Empty;
        txtReason.Text = string.Empty;
        txtDiscardQuantity.Text = string.Empty;
        txtManualNo.Text = string.Empty;
        txtIssueDate.Text = string.Empty;
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


        RadWindowForNew.NavigateUrl = "ComponentReturnList.aspx?Status=P"
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
        if (hdnComponentIssueId.Value == "")
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



            // ds = objBb.GetComponentReturnDetail(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), common.myStr(hdnComponentIssueNo.Value), 1, common.myStr(Request.QueryString["RegNo"]), common.myStr(Request.QueryString["Regid"]), common.myStr(Request.QueryString["EncounterNo"]), common.myStr(Request.QueryString["EncId"]), 1, common.myInt(hdnReturnId.Value));//objBb.GetComponentRequisition(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), common.myInt(hdnRequisition.Value.Trim()) == 0 ? -1 : common.myInt(hdnRequisition.Value.Trim()), "", 1);

            ds = objBb.GetComponentReturnDetail(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), common.myStr(hdnComponentIssueId.Value), 1, common.myStr(Request.QueryString["RegNo"]), common.myStr(Request.QueryString["Regid"]), common.myStr(Request.QueryString["EncounterNo"]), common.myStr(Request.QueryString["EncId"]), 1, common.myInt(hdnReturnId.Value));//objBb.GetComponentRequisition(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), common.myInt(hdnRequisition.Value.Trim()) == 0 ? -1 : common.myInt(hdnRequisition.Value.Trim()), "", 1);

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

                txtPatientName.Text = common.myStr(tab.Rows[0]["PatientName"]);
                txtIPNo.Text = common.myStr(tab.Rows[0]["EncounterNo"].ToString().Trim());
                txtUHID.Text = common.myStr(tab.Rows[0]["RegistrationNo"].ToString().Trim());
                txtPatientBloodGroup.Text = Convert.ToString(tab.Rows[0]["PatientBloodGroup"]);
                txtMotherBloodGroup.Text = Convert.ToString(tab.Rows[0]["MotherBloodGroup"]);
                txtRemarks.Text = Convert.ToString(tab.Rows[0]["Remarks"]);
                txtReason.Text = common.myStr(tab.Rows[0]["Reason"]);
                txtDiscardQuantity.Text = common.myStr(tab.Rows[0]["DiscartQuantity"]);
                txtWardNo.Text = Convert.ToString(tab.Rows[0]["Ward"]);
                txtBedNo.Text = Convert.ToString(tab.Rows[0]["BedNo"]);
                txtUHID.Text = common.myStr(tab.Rows[0]["RegistrationNo"]);
                txtManualNo.Text = common.myStr(tab.Rows[0]["BloodCenterReceivingNo"]);
                dtpReturnDate.SelectedDate = common.myDate(tab.Rows[0]["RreturnDate"]);

                DataView dvDyTable1 = new DataView(tab);
                dvDyTable1 = tab.DefaultView;
                dvDyTable1.RowFilter = "CrossMatchId ='" + hdnCrossMatchId.Value + "'";

                //string Value = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), "GetSingleRecordForComponentReturn", sConString);
                //if (Value == "Y")
                //{
                // FillGridWithCrossMatchedDetails((DataTable)dvDyTable1.ToTable());
                //}
                //else
                //{
                FillGridWithCrossMatchedDetails(tab);
                // }

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
            hdnRequisition.Value = RequestId.Trim();
            btnGetRequisitionInfo_Click(sender, e);
        }
        catch (Exception ex)
        {

        }

    }

    private void FillGridWithCrossMatchedDetails(DataTable dt)
    {
        gvData.DataSource = dt;
        gvData.DataBind();
        foreach (GridDataItem item in gvData.Items)
        {
            RadComboBox box = (RadComboBox)item.FindControl("ddlUnitNumber");
            box.DataSource = dt;
            box.DataValueField = "TestStockUnitNo";
            box.DataTextField = "TestStockUnitNo";
            box.DataBind();
        }


        for (int i = 0; i < dt.Rows.Count; i++)
        {
            RadComboBox box = gvData.Items[i].FindControl("ddlUnitNumber") as RadComboBox;
            box.SelectedIndex = box.Items.FindItemByText(dt.Rows[i]["TestStockUnitNo"].ToString()).Index;

            RadDatePicker datePicker = gvData.Items[i].FindControl("dtpCrossMatchDate") as RadDatePicker;
            datePicker.SelectedDate = Convert.ToDateTime(dt.Rows[i]["CrossMatchDate"]);

            Label lblQty = gvData.Items[i].FindControl("lblQuantity") as Label;
            lblQty.Text = Convert.ToString(dt.Rows[i]["Qty"]);


            if (common.myBool(dt.Rows[i]["Returned"]))
            {
                (gvData.Items[i].FindControl("chbReturn") as CheckBox).Checked = true;
                (gvData.Items[i].FindControl("chbReturn") as CheckBox).Enabled = false;

                if (common.myBool(dt.Rows[i]["Acknowledge"]))
                {
                    (gvData.Items[i].FindControl("ibtnDelete") as ImageButton).ToolTip = "already Acknowledged";
                    (gvData.Items[i].FindControl("ibtnDelete") as ImageButton).Enabled = false;
                }
                else
                {
                    (gvData.Items[i].FindControl("ibtnDelete") as ImageButton).ToolTip = "Cancel Request";
                    (gvData.Items[i].FindControl("ibtnDelete") as ImageButton).Enabled = true;
                }



            }
            else
            {
                (gvData.Items[i].FindControl("ibtnDelete") as ImageButton).Enabled = false;
                (gvData.Items[i].FindControl("ibtnDelete") as ImageButton).ToolTip = "Can't cancel";

            }


            //(gvData.Items[i].FindControl("chbIssued") as CheckBox).Checked = true;
            //(gvData.Items[i].FindControl("chbIssued") as CheckBox).Enabled = false;





        }

        //if (ViewState["ComponentIssueDetails"] != null)
        //{
        //    DataTable table = (DataTable)ViewState["ComponentIssueDetails"];
        //    for (int i = 0; i < gvData.Items.Count; i++)
        //    {
        //        bool result = Convert.ToBoolean(table.Rows[i]["Issued"]);
        //        if(!result)
        //        {
        //            gvData.Items[i].Visible = false;
        //        }
        //        if (result)
        //        {
        //            (gvData.Items[i].FindControl("chbIssued") as CheckBox).Checked = result;
        //            (gvData.Items[i].FindControl("chbIssued") as CheckBox).Enabled = false;
        //        } 
        //    }

        //}

    }

    protected void ibtnDelete_OnClick(object sender, EventArgs e)
    {
        ImageButton ibtn = (ImageButton)sender;
        if (common.myInt(ibtn.CommandArgument) > 0)
        {
            lblMessage.Text = DeleteRetunComponent(common.myInt(ibtn.CommandArgument));
            btnGetRequisitionInfo_Click(sender, e);
        }



    }

    public string DeleteRetunComponent(int ReturnId)
    {
        DataSet ds = new DataSet();
        Hashtable HshOut = new Hashtable();
        try
        {
            Hashtable HshIn = new Hashtable();

            HshIn.Add("@intReturnId", ReturnId);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            //HshIn.Add("@intFacilityId", FacilityId);
            //HshIn.Add("@chvDonorRegistrationNo", DonorRegistrationNo);
            //HshIn.Add("@dtStartDate", StartDate);
            //HshIn.Add("@dtEndDate", EndDate);
            //HshIn.Add("@bitActive", Active);


            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspBBSaveComponetReturnCanceled", HshIn, HshOut);


        }
        catch (Exception Ex)
        {
            throw Ex;
        }
        return Convert.ToString(HshOut["@chvErrorStatus"]);
    }

    //protected void ddlComponent_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    //{
    //    try
    //    {
    //        string[] hold = e.Value.Split(new char[] { '-' });
    //        if (hold.Length >= 3)
    //        {
    //            hdnComponentID.Value = hold[0];
    //            // txtAcknowledgeUnits.Text = hold[2];
    //        }
    //    }
    //    catch (Exception Ex)
    //    {
    //        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
    //        lblMessage.Text = "Error: " + Ex.Message;
    //        objException.HandleException(Ex);
    //    }
    //    FillCrossCheckedInformationIfDone(common.myInt(hdnRequisition.Value.Trim()));
    //}


    //private void FillCrossCheckedInformationIfDone(int p)
    //{

    //    DataSet ds = new DataSet();
    //    objBb = new BaseC.clsBb(sConString);
    //    try
    //    {
    //        string ComponentId = hdnComponentID.Value.ToString();
    //        int FinalComponentId = 0;
    //        if (!ComponentId.Equals(""))
    //        {
    //            FinalComponentId = Convert.ToInt32(ComponentId);
    //        }
    //    }
    //    catch (Exception Ex)
    //    {
    //        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
    //        lblMessage.Text = "Error: " + Ex.Message;
    //        objException.HandleException(Ex);
    //    }


    //}



    //protected void rbtnreturn_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    if (rbtnreturn.SelectedValue == "1")
    //    {
    //        Label33.Text = "Issue No.";
    //        Label2.Text = "Issue Date";
    //    }
    //    else
    //    {
    //        Label33.Text = "CrossMatch No";
    //        Label2.Text = "CrossMatch Date";

    //    }
    //}

    #region Transaction password validation
    private void IsValidPassword()
    {
        hdnIsValidPassword.Value = "0";
        RadWindowForNew.NavigateUrl = "/Pharmacy/Components/PasswordCheckerAllUser.aspx?UseFor=OPIP";
        RadWindowForNew.Height = 120;
        RadWindowForNew.Width = 340;
        RadWindowForNew.Top = 10;
        RadWindowForNew.Left = 10;
        RadWindowForNew.OnClientClose = "OnClientIsValidPasswordClose";
        RadWindowForNew.VisibleOnPageLoad = true;
        RadWindowForNew.Modal = true;
        RadWindowForNew.VisibleStatusbar = false;
    }

    protected void btnIsValidPasswordClose_OnClick(object Sender, EventArgs e)
    {
        try
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            if (common.myInt(hdnIsValidPassword.Value).Equals(0))
            {
                lblMessage.Text = "Invalid Username/Password!";
                return;
            }

            SaveData();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = Ex.Message;
            objException.HandleException(Ex);
        }
    }
    #endregion
}

