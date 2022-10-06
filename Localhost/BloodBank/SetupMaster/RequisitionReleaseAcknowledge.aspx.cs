using System;
using System.Data;
using System.Web.UI;
using System.Configuration;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Text;
using System.Collections;

public partial class BloodBank_SetupMaster_RequisitionReleaseAcknowledge : System.Web.UI.Page
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
                dvConfirmCancel.Visible = false;
                btnCancel.Visible = false;

                objBb = new BaseC.clsBb(sConString);
                ViewState["_ID"] = "0";

                if (common.myStr(Request.QueryString["MP"]) != "NO")
                {
                    btnclose.Visible = false;
                }

                //bindDetailsData();
                if (Request.QueryString["RegNo"] != null)
                {
                    //FillUserNames();
                    //btnAddItem.Enabled = false;

                }
                SetPermission();


                string Value = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityID"]),
                    "ShowNotificationOnBloodReleaseRequisition", sConString);
                if(Value=="Y")
                {
                    lbl_notification.Visible = true;
                    lbl_notification.Text = common.GetFlagValueHospitalSetupDesc(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityID"]),
                    "ShowNotificationOnBloodReleaseRequisition", sConString);
                }
                else
                {
                    lbl_notification.Visible = false;

                }
                 
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
    private void FillUserNames()
    {
        hdnEncounterId.Value = Convert.ToString(Request.QueryString["EncId"]).Trim();
        hdnEncounterNo.Value = Convert.ToString(Request.QueryString["EncNo"]).Trim();
        hdnRegistrationId.Value = Convert.ToString(Request.QueryString["Regid"]).Trim();
        hdnRegistrationNo.Value = Convert.ToString(Request.QueryString["RegNo"]).Trim();
        //hdnRequisition.Value = Convert.ToString(Request.QueryString["RegNo"]).Trim();

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


            ds = objBb.GetComponentRequisitionAcknowledge(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), common.myInt(hdnEncounterNo.Value.Trim()), 1, common.myInt(hdnRegistrationId.Value), 1);
            if (ds.Tables[0].Rows.Count > 0)
            {

                DataTable tab = ds.Tables[0];
                hdnRequisition.Value = common.myStr(tab.Rows[0]["RequisitionId"]);
                BaseC.Patient objPatient = new BaseC.Patient(sConString);
                txtRequestNo.Text = hdnRequisition.Value.ToString();
                txtRequestDate.Text = common.myStr(tab.Rows[0]["RequestDate"]);
                txtPatientName.Text = common.myStr(tab.Rows[0]["PatientName"]);
                txtSex.Text = common.myStr(tab.Rows[0]["Gender"]).Equals("2") ? "Male" : "Female";
                txtYear.Text = Convert.ToString(tab.Rows[0]["AgeYear"]);
                txtMonth.Text = Convert.ToString((tab.Rows[0]["AgeMonth"]));
                txtDays.Text = Convert.ToString(tab.Rows[0]["AgeDay"]);
                txtBedNo.Text =
                txtWardNo.Text = tab.Rows[0]["wardName"].ToString();
                txtIPNo.Text = common.myStr(tab.Rows[0]["EncounterNo"].ToString().Trim());
                txtUHID.Text = common.myStr(tab.Rows[0]["RegistrationNo"].ToString().Trim());
                string IsRequestAcknowledged = Convert.ToString(tab.Rows[0]["RequestAcknowledged"]);
                string IsSampleAcknowledged = Convert.ToString(tab.Rows[0]["SampleAcknowledged"]);
                //FillWardAndBedNo();
                DataTable table = GetRequisitionAcknowledgedComponent(common.myInt(hdnRequisition.Value.Trim()) == 0 ? -1 : common.myInt(hdnRequisition.Value.Trim()));
                GetReleaseRequisitionDetailsIfSaved(common.myInt(hdnRequisition.Value));
                FillChildComponents(table);
                ViewState["_ID"] = common.myStr(tab.Rows[0]["ReleaseID"]);
            }
        }



        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void OnTextChanged(object sender, EventArgs e)
    {
        ClientScript.RegisterClientScriptBlock(GetType(), "alert", "alert('" + (sender as TextBox).Text + "');", true);
    }

    private void FillWardAndBedNo()
    {
        if (Request.QueryString["RegNo"] != null)
        {
            objBb = new BaseC.clsBb(sConString);
            DataSet dset = objBb.GetPatientDetailsUsingRegistrationId(Convert.ToInt32(Request.QueryString["Regid"]));
            if (dset.Tables.Count > 0)
            {
                DataRow row = dset.Tables[0].Rows.Count > 0 ? dset.Tables[0].Rows[0] : null;
                if (row != null)
                {
                    txtWardNo.Text = Convert.ToString(row["WardName"]);
                    txtBedNo.Text = Convert.ToString(row["BedNo"]);
                }

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
            if(common.myBool(hdnIsPasswordRequired.Value))
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

            strXML = new StringBuilder();
            coll = new ArrayList();

            if (gvData != null)
            {
                foreach (GridDataItem dataItem in gvData.Items)
                {
                    HiddenField hdnComponentId = (HiddenField)dataItem.FindControl("hdnComponentID");
                    Label lblComponentName = (Label)dataItem.FindControl("lblComponentName");

                    TextBox qtyToBeReleased = (TextBox)dataItem.FindControl("txtQtytobeReleased");
                    if (qtyToBeReleased.Enabled)
                    {
                        //if (qtyToBeReleased.Text == string.Empty)
                        //{

                        //    lblMessage.Text = lblComponentName.Text + "Release quantity can not be 0";
                        //    return;
                        //}
                        if (common.myInt(qtyToBeReleased.Text) > 0)
                        {
                            Label lblOrderQty = (Label)dataItem.FindControl("lblOrderQty");
                            Label lblRequestingQty = (Label)dataItem.FindControl("lblRequestingQty");
                            Label lblQtyIssued = (Label)dataItem.FindControl("lblQtyIssued");
                            int Sum_orQty_ReqQty_Qtyissued = common.myInt(lblQtyIssued.Text) - common.myInt(qtyToBeReleased.Text);
                            if (Sum_orQty_ReqQty_Qtyissued >= 0)
                            {
                                coll.Add(common.myInt(hdnComponentId.Value));
                                coll.Add(common.myInt(qtyToBeReleased.Text.Trim()));
                                strXML.Append(common.setXmlTable(ref coll));
                            }
                            else
                            {
                                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);

                                lblMessage.Text = lblComponentName.Text + "  Qty To beReleased is not Ok";
                                clearControl();
                                return;
                            }
                        }
                        //else if (common.myInt(qtyToBeReleased.Text) == 0 && qtyToBeReleased.Enabled)
                        //{
                        //    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);

                        //    lblMessage.Text = lblComponentName.Text + "  Qty To beReleased is not Ok";
                        //    clearControl();
                        //    return;
                        //}

                    }

                    //Code has been Modiedfied -2-feb-2014 End
                }

            }

            if (strXML.ToString() == "")
            {
                lblMessage.Text = "Release quantity required...";
                return;
            }

            int ReleaseId = 0;
            string strMsg = objBb.SaveComponentRequestReleaseMain(common.myInt(ViewState["_ID"]), common.myInt(Session["HospitalLocationID"]),
                                                    common.myInt(Session["FacilityId"]), common.myInt(hdnRequisition.Value),
                                                    common.myBool(hdnIsPasswordRequired.Value) ? common.myInt(PasswordRequiredHelper.GetTransactionUserId()) : common.myInt(Session["UserId"]), // To swapping between current userid & transaction userid
                                                    strXML.ToString(),ref ReleaseId);


            if ((strMsg.ToUpper().Contains(" UPDATE") || strMsg.ToUpper().Contains(" SAVE")))
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                //clearControl();
                ViewState["_ID"] = ReleaseId;
                SetPermission();
                if (ReleaseId > 0)
                {
                    btnPrint.Visible = true;
                }                
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

    private void clearGrid()
    {
        gvData.DataSource = null;
        gvData.DataBind();
    }

    private void clearControl()
    {

        txtBedNo.Text = string.Empty;
        txtDays.Text = string.Empty;
        txtIPNo.Text = string.Empty;
        txtMonth.Text = string.Empty;
        txtPatientName.Text = string.Empty;
        txtRequestDate.Text = string.Empty;
        txtRequestNo.Text = string.Empty;
        txtSex.Text = string.Empty;
        txtUHID.Text = string.Empty;
        txtWardNo.Text = string.Empty;
        txtYear.Text = string.Empty;

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
        //RadWindowForNew.NavigateUrl = "ComponentRequisitionList.aspx
        if (Request.QueryString["Regid"] != null)
        {
            RadWindowForNew.NavigateUrl = "RequisitionReleaseAcknowledgeList.aspx?Regid="
                                            + Request.QueryString["Regid"].ToString();
        }
        else
            RadWindowForNew.NavigateUrl = "RequisitionReleaseAcknowledgeList.aspx";


        RadWindowForNew.Height = 400;
        RadWindowForNew.Width = 800;
        RadWindowForNew.Top = 40;
        RadWindowForNew.Left = 100;
        RadWindowForNew.OnClientClose = "FillDetailsForComponentRequisitionOnClientClose";
        RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindowForNew.Modal = true;
        RadWindowForNew.VisibleStatusbar = false;
        RadWindowForNew.InitialBehavior = WindowBehaviors.Maximize;
    }



    protected void btnGetRequisitionInfo_Click(object sender, EventArgs e)
    {
        lblMessage.Text = "";

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
                txtRequestNo.Text = hdnRequisition.Value.ToString();
                txtRequestDate.Text = common.myStr(tab.Rows[0]["RequestDate"]);
                txtPatientName.Text = common.myStr(tab.Rows[0]["PatientName"]);
                txtSex.Text = common.myStr(tab.Rows[0]["GenderToShow"]);//.Equals("1") ? "Male" : "Female";
                txtYear.Text = Convert.ToString(tab.Rows[0]["AgeYear"]);
                txtMonth.Text = Convert.ToString((tab.Rows[0]["AgeMonth"]));
                txtDays.Text = Convert.ToString(tab.Rows[0]["AgeDay"]);
                txtBedNo.Text = common.myStr(tab.Rows[0]["BedNo"]);
                txtWardNo.Text = common.myStr(tab.Rows[0]["Ward"]);
                txtIPNo.Text = common.myStr(tab.Rows[0]["EncounterNo"].ToString().Trim());
                txtUHID.Text = common.myStr(tab.Rows[0]["RegistrationNo"].ToString().Trim());
                string IsRequestAcknowledged = Convert.ToString(tab.Rows[0]["RequestAcknowledged"]);
                string IsSampleAcknowledged = Convert.ToString(tab.Rows[0]["SampleAcknowledged"]);
                //FillWardAndBedNo();
                DataTable table = GetRequisitionAcknowledgedComponent(common.myInt(hdnRequisition.Value.Trim()) == 0 ? -1 : common.myInt(hdnRequisition.Value.Trim()));
                GetReleaseRequisitionDetailsIfSaved(common.myInt(hdnRequisition.Value));
                FillChildComponents(table);

                DataTable dtReleaseList = ds.Tables[3];

                FillReleaseRequests(dtReleaseList);

                gvData.Enabled = true;
                btnSaveData.Visible = true;
                btnCancel.Visible = false;
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }

        //FillUserNames();
    }

    private void GetReleaseRequisitionDetailsIfSaved(int p)
    {
        DataSet ds = new DataSet();
        objBb = new BaseC.clsBb(sConString);
        //p = 25;
        ds = objBb.GetReturnRequisitionDetailsForWard(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), p);
        if (ds.Tables.Count > 0)
        {
            DataTable dtable = ds.Tables[0];

            if (dtable.Rows.Count > 0)
            {
                foreach (GridDataItem item in gvData.Items)
                {
                    int ComponentID = Convert.ToInt32((item.FindControl("hdnComponentID") as HiddenField).Value);
                    var dview = (from x in dtable.AsEnumerable()
                                 where Convert.ToInt32(x.Field<int>("ComponentCode")) == ComponentID
                                 select x).AsDataView();
                    //Old Code-before -12-2-2014 Start
                    //DataTable tab = dview.ToTable();
                    ////DataTable tab= dview.Table;
                    //string Qty = tab.Rows[0][1].ToString();
                    //string IssueQty = tab.Rows[0][2].ToString();
                    //(item.FindControl("lblRequestingQty") as Label).Text = Qty;
                    //(item.FindControl("lblQtyIssued") as Label).Text = IssueQty;
                    //Old Code-before -12-2-2014 End


                    DataTable tab = dview.ToTable();
                    //DataTable tab= dview.Table;
                    if (tab.Rows.Count > 0)
                    {
                        string Qty = tab.Rows[0][1].ToString();
                        string IssueQty = tab.Rows[0][2].ToString();
                        (item.FindControl("lblRequestingQty") as Label).Text = Qty;
                        (item.FindControl("lblQtyIssued") as Label).Text = IssueQty;

                    }
                }
            }
        }

    }

    private DataTable GetRequisitionAcknowledgedComponent(int p)
    {
        DataTable dt = new DataTable();
        objBb = new BaseC.clsBb(sConString);
        return dt = objBb.GetRequisitionAcknowledgedComponent(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), p, 1).Tables[0];
    }

    private void FillChildComponents(DataTable table)
    {
        try
        {
            gvData.DataSource = table;
            gvData.DataBind();
            //ddlComponent.DataSource = table;
            //ddlComponent.DataValueField = "ComponentDefaultMLQty";
            //ddlComponent.DataTextField = "ComponentName";
            //ddlComponent.DataBind();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    private void FillReleaseRequests(DataTable table)
    {
        try
        {
            RadGridRequistionReleased.DataSource = table;
            RadGridRequistionReleased.DataBind();
            //ddlComponent.DataSource = table;
            //ddlComponent.DataValueField = "ComponentDefaultMLQty";
            //ddlComponent.DataTextField = "ComponentName";
            //ddlComponent.DataBind();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    public static string GetQty(string Data)
    {
        return Data.Split('-')[2].ToString();
    }
    public static string GetComponentID(string Data)
    {
        return Data.Split('-')[0].ToString();
    }
    protected void gvData_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem Griditem = (GridDataItem)e.Item;
            //Label lblOrderQty = (Label)e.Item.FindControl("lblOrderQty");
            //Label lblRequestingQty = (Label)e.Item.FindControl("lblRequestingQty");
            Label lblBalanceQty = (Label)e.Item.FindControl("lblQtyIssued");

            TextBox txtQtytobeReleased = (TextBox)e.Item.FindControl("txtQtytobeReleased");
            //if ((common.myInt(lblOrderQty.Text) - common.myInt(lblRequestingQty.Text) - common.myInt(lblQtyIssued.Text)) > 0)
            if (common.myInt(lblBalanceQty.Text) > 0)
            {

                txtQtytobeReleased.Enabled = true;
            }
            else
            {
                txtQtytobeReleased.Text = "0";
                txtQtytobeReleased.Enabled = false;
            }
        }
    }

    protected void lnkBtnSearchReleaseRequisition_OnClick(object sender, EventArgs e)
    {
        hdnReleaseID.Value = "";
        hdnRequisition.Value = "";
        RadWindowForNew.NavigateUrl = "/BloodBank/SetupMaster/ReleaseRequisitionList.aspx?ReqAck=1&Regid="
                                            + Request.QueryString["Regid"].ToString()
                                            + "&RegNo=" + common.myStr(Request.QueryString["RegNo"])
                                            + "&EncId=" + common.myInt(Request.QueryString["EncId"])
                                            + "&EncNo=" + common.myStr(Request.QueryString["EncNo"]); 
        RadWindowForNew.Height = 500;
        RadWindowForNew.Width = 900;
        RadWindowForNew.Top = 10;
        RadWindowForNew.Left = 10;
        RadWindowForNew.OnClientClose = "ReleaseRequisitionList_OnClientClose";
        RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindowForNew.Modal = true;
        RadWindowForNew.VisibleStatusbar = false;
    }


    protected void btnGetReleaseInfo_OnClick(object sender, EventArgs e)
    {
        lblMessage.Text = "";
        DataSet ds = new DataSet();
        objBb = new BaseC.clsBb(sConString);
        try
        {
            int ReleaseID =common.myInt(hdnReleaseID.Value);
            ViewState["_ID"] = ReleaseID;
            ds = objBb.GetComponentRequisition(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), common.myInt(hdnRequisition.Value.Trim()) == 0 ? -1 : common.myInt(hdnRequisition.Value.Trim()), "", 1, ReleaseID);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataTable tab = ds.Tables[0];
                BaseC.Patient objPatient = new BaseC.Patient(sConString);
                txtRequestNo.Text = hdnRequisition.Value.ToString();
                txtRequestDate.Text = common.myStr(tab.Rows[0]["RequestDate"]);
                txtPatientName.Text = common.myStr(tab.Rows[0]["PatientName"]);
                txtSex.Text = common.myStr(tab.Rows[0]["Gender"]).Equals("2") ? "Male" : "Female";
                txtYear.Text = Convert.ToString(tab.Rows[0]["AgeYear"]);
                txtMonth.Text = Convert.ToString((tab.Rows[0]["AgeMonth"]));
                txtDays.Text = Convert.ToString(tab.Rows[0]["AgeDay"]);
                txtBedNo.Text = common.myStr(tab.Rows[0]["BedNo"]);
                txtWardNo.Text = common.myStr(tab.Rows[0]["Ward"]);
                txtIPNo.Text = common.myStr(tab.Rows[0]["EncounterNo"].ToString().Trim());
                txtUHID.Text = common.myStr(tab.Rows[0]["RegistrationNo"].ToString().Trim());
                string IsRequestAcknowledged = Convert.ToString(tab.Rows[0]["RequestAcknowledged"]);
                string IsSampleAcknowledged = Convert.ToString(tab.Rows[0]["SampleAcknowledged"]);
                txtReleaseId.Text =common.myStr(ReleaseID);

                //FillWardAndBedNo();
                DataTable table = objBb.GetReleaseAcknowledge(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), 1, common.myInt(hdnReleaseID.Value), 0).Tables[0];
                GetReleaseRequisitionDetailsIfSaved(common.myInt(hdnRequisition.Value));
                FillChildComponents(table);

                DataTable dtReleaseList = ds.Tables[3];

                FillReleaseRequests(dtReleaseList);

                gvData.Enabled = false;
                btnSaveData.Visible = false;
                //btnCancel.Visible = true;
                btnPrint.Visible = true;
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }

        //FillUserNames();
    }

    protected void Cancel_OnClick(object sender, EventArgs e)
    {
        dvConfirmCancel.Visible = true;
    }


    protected void btnYes_OnClick(object sender, EventArgs e)
    {
        try
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);

            if (common.myInt(hdnReleaseID.Value) == 0)
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Release Requisition not selected";
                return;
            }
            objBb = new BaseC.clsBb(sConString);
            string strMsg = objBb.releaseRequisitionCancel(common.myInt(hdnReleaseID.Value));

            clearControl();

            lblMessage.Text = strMsg;

            btnCancel.Visible = false;
            dvConfirmCancel.Visible = false;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void btnNo_OnClick(object sender, EventArgs e)
    {
        dvConfirmCancel.Visible = false;
    }


    protected void btnPrint_Click(object sender, EventArgs e)
    {
        if (hdnRequisition.Value != "" && hdnRequisition.Value != "0")
        {
            if (ViewState["_ID"] != null)
            {
                if (common.myInt(ViewState["_ID"]) > 0)
                {
                    RadWindowForNew.NavigateUrl = "/EMRReports/BloodBank/BloodBankReport.aspx?ReportName=RequiReleaseAck&ReqId=" + hdnRequisition.Value + "&ReleaseId="+ common.myInt(ViewState["_ID"]);
                    RadWindowForNew.Height = 550;
                    RadWindowForNew.Width = 580;
                    RadWindowForNew.Top = 10;
                    RadWindowForNew.Left = 10;
                    RadWindowForNew.VisibleOnPageLoad = true;
                    RadWindowForNew.Modal = true;
                    RadWindowForNew.VisibleStatusbar = false;
                    RadWindowForNew.InitialBehaviors = WindowBehaviors.Maximize;
                }
            }
        }
        else
        {
            Alert.ShowAjaxMsg("Please select Patient", Page);
            return;
        }
    }
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