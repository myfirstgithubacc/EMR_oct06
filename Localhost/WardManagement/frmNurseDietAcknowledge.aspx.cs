using BaseC;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WardManagement_frmNurseDietAcknowledge : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        lblMessages.Text = "&nbsp;";
        if (!Page.IsPostBack)
        {
            dtpFNBDate.DateInput.DateFormat = common.myStr(Application["OutputDateFormat"]);
            dtpFNBDate.DateInput.DisplayDateFormat = common.myStr(Application["OutputDateFormat"]);
            dtpFNBDate.SelectedDate = common.myDate(DateTime.Now.ToString(common.myStr(Application["OutputDateFormat"])));
            dtpFNBDate.MaxDate = common.myDate(DateTime.Now.ToString(common.myStr(Application["OutputDateFormat"])));
            //dtpFNBDate.MaxDate = common.myDate( DateTime.Now.AddDays(1).ToString(common.myStr(Application["OutputDateFormat"])));
            BindDietSlots();
            ddlFor_SelectedIndexChanged(sender, e);
        }
    }
    protected void btnFilter_Click(object sender, EventArgs e)
    {
        GetUnacknowledgeDiet();
    }
    protected void chkAll_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox ChkBoxHeader = (CheckBox)gvSoftDiet.HeaderRow.FindControl("chkAll");
        foreach (GridViewRow row in gvSoftDiet.Rows)
        {
            ((CheckBox)row.FindControl("chkAck")).Checked = ChkBoxHeader.Checked;
        }
    }
    protected void gvSoftDiet_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvSoftDiet.PageIndex = e.NewPageIndex;
        GetUnacknowledgeDiet();
    }
    protected void gvSoftDietA_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvSoftDietA.PageIndex = e.NewPageIndex;
        GetUnacknowledgeDiet();
    }
    protected void btnDietAck_Click(object sender, EventArgs e)
    {
        if (common.myLen(txtAckRemark.Text).Equals(0))
        {
            txtAckRemark.Focus();
            lblMessages.Text = "Please fill Acknowledge Remarks!!!";
            lblMessages.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            return;
        }

        StringBuilder strXML = new StringBuilder();
        ArrayList col = new ArrayList();

        foreach (GridViewRow row in gvSoftDiet.Rows)
        {
            if (((CheckBox)row.FindControl("chkAck")).Checked)
            {
                HiddenField hdnSendToPatientCutDateTime = (HiddenField)row.FindControl("hdnSendToPatientCutDateTime");
                HiddenField hdnPatientDietCardDetailsID = (HiddenField)row.FindControl("hdnPatientDietCardDetailsID");
                HiddenField hdnPatientDietCardMainId = (HiddenField)row.FindControl("hdnPatientDietCardMainId");
                TextBox txtRemark = (TextBox)row.FindControl("txtremark");
                col.Add(common.myInt(((HiddenField)row.FindControl("hdnDietEmrrequestID")).Value)); //DietEmrrequestID
                col.Add(common.myInt(((HiddenField)row.FindControl("hdnDietSlotId")).Value));//DietSlotId
                col.Add(common.myInt(((HiddenField)row.FindControl("hdnEncounterId")).Value));//EncounterId                
                col.Add(common.myInt(((HiddenField)row.FindControl("hdnDietAckCutOffTimeInMinutes")).Value));//DietAckCutOffTimeInMinutes
                col.Add(hdnSendToPatientCutDateTime.Value);//SendToPatientCutDateTime
                //col.Add(common.myInt(((HiddenField)row.FindControl("hdnRegistrationId")).Value));//RegistrationId
                col.Add(txtRemark.Text);//Remark  
                col.Add(hdnPatientDietCardDetailsID.Value);//Remark  
                col.Add(hdnPatientDietCardMainId.Value);//Remark  
                strXML.Append(common.setXmlTable(ref col));
            }
        }

        if (common.myLen(strXML.ToString()).Equals(0))
        {
            lblMessages.Text = "Please select atleast one diet for acknowledge!!!!";
            lblMessages.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            return;
        }

        WardManagement oWardManagement = new WardManagement();
        try
        {
            lblMessages.Text = oWardManagement.SaveEmrSoftDietAcknowlegment(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["UserId"]), strXML.ToString(),
                common.myStr(txtAckRemark.Text));
            if (lblMessages.Text.Equals("Saved successfully"))
            {
                lblMessages.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                txtAckRemark.Text = "";
                GetUnacknowledgeDiet();
            }
            else
                lblMessages.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
        }
        catch (Exception Ex)
        {
            clsExceptionLog oclsExceptionLog = new clsExceptionLog();
            oclsExceptionLog.HandleException(Ex);
            oclsExceptionLog = null;
        }
        finally
        {
            oWardManagement = null;
        }

    }
    protected void ddlFor_SelectedIndexChanged(object sender, EventArgs e)
    {
        gvSoftDiet.Visible = false;
        gvSoftDiet.DataSource = null;
        gvSoftDiet.DataBind();
        gvSoftDietA.Visible = false;
        gvSoftDietA.DataSource = null;
        gvSoftDietA.DataBind();
        btnDietAck.Visible = true;
        pnlRemarks.Visible = true;
        txtAckRemark.Text = "";
        if (ddlFor.SelectedValue.Equals("A"))
        {
            btnDietAck.Visible = false;
            pnlRemarks.Visible = false;
        }
        DataTable dt = new DataTable();
        try
        {
            dt = CreateBlankGrid();
            if (common.myStr(ddlFor.SelectedValue).Equals("UA"))
            {
                gvSoftDiet.Visible = true;
                gvSoftDiet.DataSource = dt;
                gvSoftDiet.DataBind();
            }
            else if (common.myStr(ddlFor.SelectedValue).Equals("A"))
            {
                gvSoftDietA.Visible = true;
                gvSoftDietA.DataSource = dt;
                gvSoftDietA.DataBind();
            }
        }
        catch (Exception Ex)
        {
            clsExceptionLog oclsExceptionLog = new clsExceptionLog();
            oclsExceptionLog.HandleException(Ex);
            oclsExceptionLog = null;
        }
        finally
        {
            dt.Dispose();
        }

    }
    protected void ddlSearchBy_SelectedIndexChanged(object sender, EventArgs e)
    {
        #region ---|| Default State ||---     
        txtEncounterNo.Text = "";
        txtPatientName.Text = "";
        txtRegistrationNo.Text = "";
        txtEncounterNo.Visible = false;
        txtPatientName.Visible = false;
        txtRegistrationNo.Visible = false;
        #endregion
        if (common.myStr(ddlSearchBy.SelectedValue).Equals("E"))
            txtEncounterNo.Visible = true;
        else if (common.myStr(ddlSearchBy.SelectedValue).Equals("P"))
            txtPatientName.Visible = true;
        else if (common.myStr(ddlSearchBy.SelectedValue).Equals("R"))
            txtRegistrationNo.Visible = true;
    }

    #region ---|| Methods ||---   
    private DataTable CreateBlankGrid()
    {
        DataTable dt = CreateBlankTable();
        DataRow dr = dt.NewRow();
        dr["DietEmrrequestID"] = DBNull.Value;
        dr["DietSlotId"] = DBNull.Value;
        dr["EncounterId"] = DBNull.Value;
        dr["RegistrationId"] = DBNull.Value;
        dr["DietAckCutOffTimeInMinutes"] = DBNull.Value;
        dr["SendToPatientCutDateTime"] = DBNull.Value;


        dr["EncounterNo"] = DBNull.Value;
        dr["RegistrationNo"] = DBNull.Value;
        dr["PName"] = DBNull.Value;
        dr["Encounter Status"] = DBNull.Value;
        dr["DietSlotName"] = DBNull.Value;
        dr["DietName"] = DBNull.Value;
        dr["Diagnosis"] = DBNull.Value;
        dr["Remarks"] = DBNull.Value;
        dr["EncodedBy"] = DBNull.Value;
        dr["EncodedDate"] = DBNull.Value;
        dr["SendToPatientDate"] = DBNull.Value;
        dr["DietAckBy"] = DBNull.Value;
        dr["AcknowledgeRemarks"] = DBNull.Value;
        dr["DietAckEncodedDate"] = DBNull.Value;

        dt.Rows.Add(dr);
        return dt;
    }
    private DataTable CreateBlankTable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("DietEmrrequestID", typeof(int));
        dt.Columns.Add("DietSlotId", typeof(int));
        dt.Columns.Add("EncounterId", typeof(int));
        dt.Columns.Add("RegistrationId", typeof(int));
        dt.Columns.Add("DietAckCutOffTimeInMinutes", typeof(int));
        dt.Columns.Add("SendToPatientCutDateTime", typeof(int));
        dt.Columns.Add("EncounterNo", typeof(string));
        dt.Columns.Add("RegistrationNo", typeof(string));
        dt.Columns.Add("PName", typeof(string));
        dt.Columns.Add("DietSlotName", typeof(string));
        dt.Columns.Add("DietName", typeof(string));
        dt.Columns.Add("Diagnosis", typeof(string));
        dt.Columns.Add("Remarks", typeof(string));
        dt.Columns.Add("EncodedBy", typeof(string));
        dt.Columns.Add("EncodedDate", typeof(string));
        dt.Columns.Add("SendToPatientDate", typeof(string));
        dt.Columns.Add("DietAckBy", typeof(string));
        dt.Columns.Add("AcknowledgeRemarks", typeof(string));
        dt.Columns.Add("DietAckEncodedDate", typeof(string));
        dt.Columns.Add("PatientDietCardDetailsID", typeof(int));
        dt.Columns.Add("PatientDietCardMainId", typeof(int));
        dt.Columns.Add("StatusId", typeof(int));
        dt.Columns.Add("EncStatus", typeof(string));
        return dt;
    }
    public void BindDietSlots()
    {
        Diet oDiet = new Diet(sConString);
        DataSet ds = new DataSet();
        try
        {
            ds = oDiet.GetDietSlots();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlSlot.DataSource = ds;
                ddlSlot.DataValueField = "DietSlotsId";
                ddlSlot.DataTextField = "DietSlotName";
                ddlSlot.DataBind();
            }
            ddlSlot.Items.Insert(0, new ListItem("-- Select Slot --", "0"));
        }
        catch (Exception Ex)
        {
            clsExceptionLog oclsExceptionLog = new clsExceptionLog();
            oclsExceptionLog.HandleException(Ex);
            oclsExceptionLog = null;
        }
        finally
        {
            ds.Dispose();
            oDiet = null;
        }
    }
    public void GetUnacknowledgeDiet()
    {
        string RegistrationNo = "", PatientName = "", EncounterNo = "", DietType = "DS";
        int intDietSlotsId = 0;
        if (common.myLen(txtEncounterNo.Text) > 0)
            EncounterNo = txtEncounterNo.Text;
        if (common.myLen(txtPatientName.Text) > 0)
            PatientName = txtPatientName.Text;
        if (common.myLen(txtRegistrationNo.Text) > 0)
            RegistrationNo = txtRegistrationNo.Text;
        if (DietType.Equals("DS") && common.myInt(ddlSlot.SelectedValue) > 0)
            intDietSlotsId = common.myInt(ddlSlot.SelectedValue);

        if (intDietSlotsId.Equals(0))
        {
            lblMessages.Text = "Please select slot!!!!";
            lblMessages.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            return;
        }

        if (dtpFNBDate.SelectedDate == null)
        {
            lblMessages.Text = "Please select FNB Date!!!!";
            lblMessages.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            dtpFNBDate.Focus();
            return;
        }

        WardManagement wm = new WardManagement();
        DataSet ds = new DataSet();
        try
        {
            ds = wm.GetPreparedMealForApproval(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), DietType, common.myDate(dtpFNBDate.SelectedDate).ToString("yyyy/MM/dd HH:mm"),
                                                RegistrationNo, EncounterNo, PatientName, intDietSlotsId, common.myStr(ddlFor.SelectedValue));

            if (common.myStr(ddlFor.SelectedValue).Equals("UA"))
            {
                gvSoftDiet.Visible = true;
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    gvSoftDiet.DataSource = ds.Tables[0];
                else
                {
                    DataRow DR = ds.Tables[0].NewRow();
                    ds.Tables[0].Rows.Add(DR);
                    gvSoftDiet.DataSource = ds.Tables[0];
                }
                gvSoftDiet.DataBind();
            }
            else if (common.myStr(ddlFor.SelectedValue).Equals("A"))
            {
                gvSoftDietA.Visible = true;
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    gvSoftDietA.DataSource = ds.Tables[0];
                else
                {
                    DataRow DR = ds.Tables[0].NewRow();
                    ds.Tables[0].Rows.Add(DR);
                    gvSoftDietA.DataSource = ds.Tables[0];
                }
                gvSoftDietA.DataBind();
            }
        }
        catch (Exception Ex)
        {
            clsExceptionLog oclsExceptionLog = new clsExceptionLog();
            oclsExceptionLog.HandleException(Ex);
            oclsExceptionLog = null;
        }
        finally
        {
            ds.Dispose();
            wm = null;
        }
    }
    #endregion
}