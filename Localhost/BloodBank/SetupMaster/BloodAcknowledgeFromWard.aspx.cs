using System;
using System.Data;
using System.Web.UI;
using System.Configuration;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Text;
using System.Collections;

public partial class BloodBank_SetupMaster_BloodAcknowledgeFromWard : System.Web.UI.Page
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

                if (Request.QueryString["EncounterNo"] != null)
                {
                    hdnRegistrationID.Value = Convert.ToString(Request.QueryString["Regid"]);
                    hdnRegistrationNo.Value = Convert.ToString(Request.QueryString["RegNo"]);
                    hdnEncounterID.Value = Convert.ToString(Request.QueryString["EncId"]);
                    hdnEncounterNo.Value = Convert.ToString(Request.QueryString["EncounterNo"]);
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
                    if ((item.FindControl("chkAcknowledged") as CheckBox).Enabled)
                    {
                        coll.Add(((item.FindControl("lblBagNo") as Label).Text.Trim()));
                        coll.Add((item.FindControl("lblQuantity") as Label).Text.Trim());
                        coll.Add((item.FindControl("chkAcknowledged") as CheckBox).Checked ? 1 : 0);
                        coll.Add(Convert.ToString(hdnCrossMatchNo.Value));
                        strXML.Append(common.setXmlTable(ref coll));
                    }

                }
            }


            if (strXML.ToString() != string.Empty)
                strMsg = objBb.UpdateComponentIssueDetails(txtIssueNo.Text.Trim(), common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), common.myDate(DateTime.Now).ToString(), common.myInt(Session["UserID"]), strXML.ToString());
            else
                strMsg = "Recors is already saved......";

            if ((strMsg.ToUpper().Contains(" UPDATE") || strMsg.ToUpper().Contains(" SAVE")))
            {

                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                clearControl();
                ViewState["_ID"] = "0";
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
        txtIssueDate.Text = string.Empty;
        gvData.DataSource = string.Empty;
        txtIssueNo.Text = string.Empty;
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

        hdnCrossMatchId.Value = "";

        RadWindowForNew.NavigateUrl = "BloodAcknowledgeList.aspx?Regid=" + hdnRegistrationID.Value
                       + "&RegNo=" + common.myStr(hdnRegistrationNo.Value)
                        + "&EncId=" + common.myInt(hdnEncounterID.Value)
                        + "&EncNo=" + common.myStr(hdnEncounterNo.Value)
                        + "&Ack=" + 0.ToString();

        RadWindowForNew.Height = 650;
        RadWindowForNew.Width = 700;
        RadWindowForNew.Top = 200;
        RadWindowForNew.Left = 200;
        RadWindowForNew.OnClientClose = "FillDetailsForComponentRequisitionOnClientClose";
        RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindowForNew.Modal = true;
        RadWindowForNew.VisibleStatusbar = false;
        RadWindowForNew.InitialBehavior = WindowBehaviors.Maximize;
    }
    protected void btnGetRequisitionInfo_Click(object sender, EventArgs e)
    {
        if (hdnCrossMatchId.Value != "")
        {
            GetDetails(0);
        }
        else
            clearControl();

    }
    private void GetDetails(int type)
    {
        #region  to get blood acknowledge info

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

            ds = objBb.GetComponentIssueAcknowledgeDetails(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), common.myStr(hdnComponentIssueNo.Value), common.myInt(hdnCrossMatchId.Value), type, 1); //objBb.GetComponentRequisition(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), common.myInt(hdnRequisition.Value.Trim()) == 0 ? -1 : common.myInt(hdnRequisition.Value.Trim()), "", 1);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataTable tab = ds.Tables[0];
                BaseC.Patient objPatient = new BaseC.Patient(sConString);
                txtPatientName.Text = common.myStr(tab.Rows[0]["PatientName"]);
                txtIssueDate.Text = (common.myDate(tab.Rows[0]["issueDate"])).ToString();
                txtIssueNo.Text = common.myStr(tab.Rows[0]["CrossIssueNo"]);
                txtReferredBy.Text = common.myStr(tab.Rows[0]["ReferredBy"]);
                gvData.DataSource = tab;
                gvData.DataBind();
            }
            else
            {
                gvData.DataSource = string.Empty;
                gvData.DataBind();
            }



        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }

        #endregion
    }
    //public DataSet GetComponentIssueAcknowledgeDetails(int HospitalLocationId, int FacilityId, string CrossIssueNo, int CrossMatchId, int bloodAcknowledged, int Active)
    //{
    //    DataSet ds = new DataSet();
    //    try
    //    {
    //        Hashtable HshIn = new Hashtable();
    //        HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
    //        HshIn.Add("@intFacilityId", FacilityId);
    //        HshIn.Add("@CrossIssueNo", CrossIssueNo);
    //        HshIn.Add("@intCrossMatchId", CrossMatchId);
    //        HshIn.Add("@bitbloodAcknowledged", bloodAcknowledged);
    //        HshIn.Add("@bitActive", Active);
    //        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
    //        ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetComponentIssueAcknowledgeDetails", HshIn);
    //    }
    //    catch (Exception Ex)
    //    {
    //        throw Ex;
    //    }
    //    return ds;
    //}

    private void GetAndSetIssueDetailsIfSaved(string p)
    {

        DataSet ds = new DataSet();
        objBb = new BaseC.clsBb(sConString);
        try
        {
            DataSet dset = objBb.GetComponentIssueDetails(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), p, common.myInt(hdnCrossMatchId.Value), 1, "P");
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
    private void FillUnitNumberDropDownListInGrid()
    {
        DataTable dt = new DataTable();
        objBb = new BaseC.clsBb(sConString);
        dt = objBb.GetComponentBagNo(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), Convert.ToInt32(hdnComponentID.Value), 1).Tables[0];
        foreach (GridDataItem item in gvData.Items)
        {
            RadComboBox box = (RadComboBox)item.FindControl("ddlUnitNumber");
            box.DataSource = dt;
            box.DataValueField = "SegmentNo";
            box.DataTextField = "Bagnumber";
            box.DataBind();
        }

    }
    protected void lbtnSearchCrossMatch_Click(object sender, EventArgs e)
    {
        RadWindowForNew.NavigateUrl = "SearchComponentIssueDetails.aspx";
        RadWindowForNew.Height = 650;
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
            if (hdnCrossMatchId.Value != "")
            {
                GetDetails(1);
            }
            else
                clearControl();


        }
        catch (Exception ex)
        {

        }

    }
    private void FillGridWithCrossMatchedDetails(DataTable dt)
    {
        gvData.DataSource = dt;
        gvData.DataBind();
        FillUnitNumberDropDownListInGrid();


        for (int i = 0; i < dt.Rows.Count; i++)
        {
            RadComboBox box = gvData.Items[i].FindControl("ddlUnitNumber") as RadComboBox;
            box.SelectedIndex = box.Items.FindItemByText(dt.Rows[i]["TestStockUnitNo"].ToString()).Index;

            RadDatePicker datePicker = gvData.Items[i].FindControl("dtpCrossMatchDate") as RadDatePicker;
            datePicker.SelectedDate = Convert.ToDateTime(dt.Rows[i]["CrossMatchDate"]);

            Label lblQty = gvData.Items[i].FindControl("lblQuantity") as Label;
            lblQty.Text = Convert.ToString(dt.Rows[i]["Qty"]);

        }

        if (ViewState["ComponentIssueDetails"] != null)
        {
            DataTable table = (DataTable)ViewState["ComponentIssueDetails"];
            for (int i = 0; i < gvData.Items.Count; i++)
            {
                bool result = Convert.ToBoolean(table.Rows[i]["Issued"]);
                if (!result)
                {
                    gvData.Items[i].Visible = false;
                }
                (gvData.Items[i].FindControl("chbIssued") as CheckBox).Checked = result;
            }

        }

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
    protected void gvData_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem Griditem = (GridDataItem)e.Item;
            HiddenField hdnIssue = (HiddenField)e.Item.FindControl("hdnIssue");

            CheckBox chkbox = (CheckBox)e.Item.FindControl("chbIssued");
            CheckBox chkAcknowledge = (CheckBox)e.Item.FindControl("chkAcknowledged");
            HiddenField hdnAcknowledge = (HiddenField)e.Item.FindControl("hdnAcknowledge");
            if (common.myBool(hdnAcknowledge.Value))
            {
                chkAcknowledge.Checked = true;
                chkAcknowledge.Enabled = false;
            }
            if (common.myBool(hdnIssue.Value))
            {
                chkbox.Checked = true;

            }
        }
    }
    protected void lbtnSearchrBloodAcknowledge_Click(object sender, EventArgs e)
    {
        hdnCrossMatchId.Value = "";
        RadWindowForNew.NavigateUrl = "BloodAcknowledgeList.aspx?Regid=" + hdnRegistrationID.Value
                      + "&RegNo=" + common.myStr(hdnRegistrationNo.Value)
                       + "&EncId=" + common.myInt(hdnEncounterID.Value)
                       + "&EncNo=" + common.myStr(hdnEncounterNo.Value)
                       + "&Ack=" + 1.ToString();

        //RadWindowForNew.NavigateUrl = "SearchComponentIssueDetails.aspx";
        RadWindowForNew.Height = 650;
        RadWindowForNew.Width = 750;
        RadWindowForNew.Top = 200;
        RadWindowForNew.Left = 200;
        RadWindowForNew.OnClientClose = "SearchComponentIssueDetailsOnClientClose";
        RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindowForNew.Modal = true;
        RadWindowForNew.VisibleStatusbar = false;
        RadWindowForNew.InitialBehavior = WindowBehaviors.Maximize;
    }
}

