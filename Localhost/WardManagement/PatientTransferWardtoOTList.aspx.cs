using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Text;
using System.Xml.Linq;
using Telerik.Web.UI;

public partial class WardManagement_PatientTransferWardtoOTList : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();

    private enum enumColumns : byte
    {
        TheatreName = 0,

        RegistrationNo = 1,
        EncounterNo = 2,
        PatientName = 3,
        WardRemarks = 4,

        BedNo = 5,
        WardName = 6,

        WardtoOTByName = 7,
        WardtoOTDate = 8,
        OTAckCancelByName = 9,
        OTAckCancelDate = 10,
        OTAckCancelRemarks = 11,
        TransferToWardByName = 12,
        TransferToWardDate = 13,
        Acknowledge = 14,
        Cancel = 15,
        SendtoWard = 16,
        WardAck = 17
    }

    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            objException = new clsExceptionLog();

            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Font.Bold = commonLabelSetting.cBold;
            if (commonLabelSetting.cFont != "")
            {
                lblMessage.Font.Name = commonLabelSetting.cFont;
            }
            txtSearchN.Visible = false;
            txtSearch.Visible = false;
            if (common.myStr(ddlName.SelectedValue) == "R")
            {
                txtSearchN.Visible = true;
            }
            else
            {
                txtSearch.Visible = true;
            }
            ViewState["PT"] = common.myStr(Request.QueryString["PT"]);


            ViewState["DecimalPlaces"] = "2";

            clearControl();

            if (common.myStr(ViewState["PT"]) == "WARD")
            {
                rdoTransferStatus.SelectedIndex = rdoTransferStatus.Items.IndexOf(rdoTransferStatus.Items.FindByValue("3"));
                rdoTransferStatus.Visible = false;
            }

            bindControl();

            bindData();
        }
    }

    private void bindControl()
    {
        DataSet ds = new DataSet();
        BaseC.clsLISPhlebotomy objval = new BaseC.clsLISPhlebotomy(sConString);
        try
        {

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
            objval = null;
        }
    }

    private void clearControl()
    {
        try
        {
            lblMessage.Text = "&nbsp;";
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);

           
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    protected void btnSearch_OnClick(Object sender, EventArgs e)
    {
        bindData();
    }

    protected void btnClearSearch_OnClick(Object sender, EventArgs e)
    {
        clearControl();
        bindData();
    }


    protected void gvEncounter_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType != DataControlRowType.Pager)
            {
                //RegistrationNo
                //EncounterNo
                //PatientName
                //WardRemarks
                //WardtoOTByName
                //WardtoOTDate
                //OTAckCancelByName
                //OTAckCancelDate
                //OTAckCancelRemarks
                //TransferToWardByName
                //TransferToWardDate
                //Acknowledge
                //Cancel
                //SendtoWard
                //WardAck
                if (common.myStr(ViewState["PT"]) == "OT")
                {
                    switch (common.myInt(rdoTransferStatus.SelectedValue))
                    {
                        case 0:
                            e.Row.Cells[Convert.ToByte(enumColumns.OTAckCancelByName)].Visible = false;
                            e.Row.Cells[Convert.ToByte(enumColumns.OTAckCancelDate)].Visible = false;
                            e.Row.Cells[Convert.ToByte(enumColumns.TransferToWardByName)].Visible = false;
                            e.Row.Cells[Convert.ToByte(enumColumns.TransferToWardDate)].Visible = false;
                            e.Row.Cells[Convert.ToByte(enumColumns.OTAckCancelDate)].Visible = false;
                            e.Row.Cells[Convert.ToByte(enumColumns.SendtoWard)].Visible = false;
                            e.Row.Cells[Convert.ToByte(enumColumns.WardAck)].Visible = false;
                            break;
                        case 1:
                            e.Row.Cells[Convert.ToByte(enumColumns.TransferToWardByName)].Visible = false;
                            e.Row.Cells[Convert.ToByte(enumColumns.TransferToWardDate)].Visible = false;
                            e.Row.Cells[Convert.ToByte(enumColumns.Acknowledge)].Visible = false;
                            e.Row.Cells[Convert.ToByte(enumColumns.Cancel)].Visible = false;
                            e.Row.Cells[Convert.ToByte(enumColumns.SendtoWard)].Visible = false;
                            e.Row.Cells[Convert.ToByte(enumColumns.WardAck)].Visible = false;
                            break;
                        case 2:
                            e.Row.Cells[Convert.ToByte(enumColumns.TransferToWardByName)].Visible = false;
                            e.Row.Cells[Convert.ToByte(enumColumns.TransferToWardDate)].Visible = false;
                            e.Row.Cells[Convert.ToByte(enumColumns.Acknowledge)].Visible = false;
                            e.Row.Cells[Convert.ToByte(enumColumns.Cancel)].Visible = false;
                            e.Row.Cells[Convert.ToByte(enumColumns.SendtoWard)].Visible = false;
                            e.Row.Cells[Convert.ToByte(enumColumns.WardAck)].Visible = false;
                            break;
                        case 3:
                            e.Row.Cells[Convert.ToByte(enumColumns.TransferToWardByName)].Visible = false;
                            e.Row.Cells[Convert.ToByte(enumColumns.TransferToWardDate)].Visible = false;
                            e.Row.Cells[Convert.ToByte(enumColumns.Acknowledge)].Visible = false;
                            e.Row.Cells[Convert.ToByte(enumColumns.Cancel)].Visible = false;
                            e.Row.Cells[Convert.ToByte(enumColumns.WardAck)].Visible = false;
                            break;
                    }
                }
                else if (common.myStr(ViewState["PT"]) == "WARD")
                {
                    e.Row.Cells[Convert.ToByte(enumColumns.OTAckCancelByName)].Visible = false;
                    e.Row.Cells[Convert.ToByte(enumColumns.OTAckCancelDate)].Visible = false;
                    e.Row.Cells[Convert.ToByte(enumColumns.Acknowledge)].Visible = false;
                    e.Row.Cells[Convert.ToByte(enumColumns.Cancel)].Visible = false;
                    e.Row.Cells[Convert.ToByte(enumColumns.SendtoWard)].Visible = false;
                }
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //HiddenField hdnRefundApprovedAckBy = (HiddenField)e.Row.FindControl("hdnRefundApprovedAckBy");
                TextBox txtOTAckCancelRemarks = (TextBox)e.Row.FindControl("txtOTAckCancelRemarks");

                if (common.myStr(ViewState["PT"]) == "OT")
                {
                    if (common.myInt(rdoTransferStatus.SelectedValue) > 0)
                    {
                        txtOTAckCancelRemarks.Enabled = false;
                    }
                }

                //Label lblAmount = (Label)e.Row.FindControl("lblAmount");
                //lblAmount.Text = common.myDbl(lblAmount.Text).ToString("F" + common.myInt(ViewState["DecimalPlaces"]));

                //DropDownList ddlApprovalStatus = (DropDownList)e.Row.FindControl("ddlApprovalStatus");

                //if (common.myStr(ViewState["PT"]) == "ACK")
                //{
                //    ddlApprovalStatus.Enabled = false;
                //    txtApprovalRemarks.Enabled = false;

                //    if (common.myInt(hdnApprovalStatusId.Value) != common.myInt(ViewState["ApprovedStatusId"])
                //        || common.myInt(hdnRefundApprovedAckBy.Value) > 0)
                //    {
                //        lnkAcknowledge.Visible = false;
                //    }
                //}

                //if (common.myStr(ViewState["PT"]) == "APP")
                //{
                //    if (common.myInt(hdnRefundApprovedAckBy.Value) > 0)
                //    {
                //        ddlApprovalStatus.Enabled = false;
                //    }
                //}

                //if (common.myInt(hdnRefundApprovedAckBy.Value) > 0)
                //{
                //    e.Row.BackColor = System.Drawing.Color.LightGreen;
                //    txtApprovalRemarks.Enabled = false;
                //}

                //if (ViewState["ApprovalStatus"] != null)
                //{


                //    ddlApprovalStatus.DataSource = (DataTable)ViewState["ApprovalStatus"];
                //    ddlApprovalStatus.DataTextField = "Status";
                //    ddlApprovalStatus.DataValueField = "StatusId";
                //    ddlApprovalStatus.DataBind();

                //    ddlApprovalStatus.Items.Insert(0, new ListItem("", "0"));

                //    ddlApprovalStatus.SelectedIndex = ddlApprovalStatus.Items.IndexOf(ddlApprovalStatus.Items.FindByValue(common.myStr(hdnApprovalStatusId.Value)));
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

    protected void gvEncounter_OnRowCommand(object Sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Acknowledge")
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);

                GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                HiddenField hdnRegistrationId = (HiddenField)row.FindControl("hdnRegistrationId");
                HiddenField hdnEncounterId = (HiddenField)row.FindControl("hdnEncounterId");
                TextBox txtOTAckCancelRemarks = (TextBox)row.FindControl("txtOTAckCancelRemarks");
                HiddenField hdnOTBookingID = (HiddenField)row.FindControl("hdnOTBookingID");
                if (common.myLen(txtOTAckCancelRemarks.Text) == 0)
                {
                    lblMessage.Text = "OT Remarks can't be blank !";
                    return;
                }

                //C-Ack Transfer in OT
                saveData("C", common.myInt(e.CommandArgument), common.myInt(hdnRegistrationId.Value), common.myInt(hdnEncounterId.Value),
                         common.myStr(txtOTAckCancelRemarks.Text).Trim(), common.myInt(hdnOTBookingID.Value));
            }
            else if (e.CommandName == "Cancel1")
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);

                GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                HiddenField hdnRegistrationId = (HiddenField)row.FindControl("hdnRegistrationId");
                HiddenField hdnEncounterId = (HiddenField)row.FindControl("hdnEncounterId");
                TextBox txtOTAckCancelRemarks = (TextBox)row.FindControl("txtOTAckCancelRemarks");

                if (common.myLen(txtOTAckCancelRemarks.Text) == 0)
                {
                    lblMessage.Text = "OT Remarks can't be blank !";
                    return;
                }

                //D-Cancel Transfer in OT
                saveData("D", common.myInt(e.CommandArgument), common.myInt(hdnRegistrationId.Value), common.myInt(hdnEncounterId.Value),
                         common.myStr(txtOTAckCancelRemarks.Text).Trim());
            }
            else if (e.CommandName == "SendtoWard")
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);

                GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                HiddenField hdnRegistrationId = (HiddenField)row.FindControl("hdnRegistrationId");
                HiddenField hdnEncounterId = (HiddenField)row.FindControl("hdnEncounterId");
                TextBox txtOTAckCancelRemarks = (TextBox)row.FindControl("txtOTAckCancelRemarks");

                if (common.myLen(txtOTAckCancelRemarks.Text) == 0)
                {
                    lblMessage.Text = "OT Remarks can't be blank !";
                    return;
                }

                //E-Transfer to Ward
                saveData("E", common.myInt(e.CommandArgument), common.myInt(hdnRegistrationId.Value), common.myInt(hdnEncounterId.Value),
                         common.myStr(txtOTAckCancelRemarks.Text).Trim());
            }
            else if (e.CommandName == "WardAck")
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);

                GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                HiddenField hdnRegistrationId = (HiddenField)row.FindControl("hdnRegistrationId");
                HiddenField hdnEncounterId = (HiddenField)row.FindControl("hdnEncounterId");
                TextBox txtOTAckCancelRemarks = (TextBox)row.FindControl("txtOTAckCancelRemarks");

                if (common.myLen(txtOTAckCancelRemarks.Text) == 0)
                {
                    lblMessage.Text = "OT Remarks can't be blank !";
                    return;
                }

                //F-Ward Ack
                saveData("F", common.myInt(e.CommandArgument), common.myInt(hdnRegistrationId.Value), common.myInt(hdnEncounterId.Value),
                         common.myStr(txtOTAckCancelRemarks.Text).Trim());
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    protected void rdoTransferStatus_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            bindData();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private void bindData()
    {
        DataSet ds = new DataSet();
        BaseC.clsOTBooking objCls = new BaseC.clsOTBooking(sConString);
        try
        {
            int TransferStatus = common.myStr(ViewState["PT"]).ToUpper().Equals("WARD") ? -1 : common.myInt(rdoTransferStatus.SelectedValue);

            string RegNo = "", EncounterNo="", PatientName="";
            if (common.myStr(ddlName.SelectedValue) == "R")
            {
                RegNo = txtSearchN.Text;
            }
            else if (common.myStr(ddlName.SelectedValue) == "ENC")
            {
                EncounterNo = txtSearch.Text;
            }
            else if (common.myStr(ddlName.SelectedValue) == "N")
            {
                PatientName = txtSearch.Text;
            }


            ds = objCls.getPatientTransferWardtoOT(common.myInt(Session["FacilityId"]), common.myInt(RegNo), common.myStr(EncounterNo).Trim(),
                                                common.myStr(PatientName).Trim(), TransferStatus);

            DataView DV = ds.Tables[0].DefaultView;
            if (common.myStr(ViewState["PT"]).ToUpper().Equals("OT"))
            {
                if (common.myBool(common.myInt(rdoTransferStatus.SelectedValue)))
                {
                    DV.RowFilter = "ISNULL(TransferToWardBy,0)=0";
                }
            }
            else if (common.myStr(ViewState["PT"]).ToUpper().Equals("WARD"))
            {
                DV.RowFilter = "ISNULL(TransferToWardBy,0)= " + common.myInt(Session["UserId"]) + "AND ISNULL(WardAckBy,0)=0";
            }

            ds.Tables.RemoveAt(0);

            ds.Tables.Add(DV.ToTable());

            if (ds.Tables[0].Rows.Count == 0)
            {
                DataRow DR = ds.Tables[0].NewRow();
                ds.Tables[0].Rows.Add(DR);
            }

            gvEncounter.DataSource = ds.Tables[0];
            gvEncounter.DataBind();
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
            objCls = null;
        }
    }

    protected void gvEncounter_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvEncounter.PageIndex = e.NewPageIndex;
        bindData();
    }

    protected void btnSave_OnClick(Object sender, EventArgs e)
    {
        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
        BaseC.EMRBilling.clsRefund objR = new BaseC.EMRBilling.clsRefund(sConString);
        try
        {
            StringBuilder strXMLItems = new StringBuilder();
            ArrayList coll = new ArrayList();

            foreach (GridViewRow item in gvEncounter.Rows)
            {
                DropDownList ddlApprovalStatus = (DropDownList)item.FindControl("ddlApprovalStatus");

                if (common.myInt(ddlApprovalStatus.SelectedValue) > 0 && ddlApprovalStatus.Enabled)
                {
                    TextBox txtApprovalRemarks = (TextBox)item.FindControl("txtApprovalRemarks");
                    if (common.myInt(ddlApprovalStatus.SelectedValue) == common.myInt(ViewState["RejectedStatusId"])
                        && common.myLen(txtApprovalRemarks.Text) == 0)
                    {
                        lblMessage.Text = "Rejection remarks can't be blank!";
                        return;
                    }

                    HiddenField hdnRefundId = (HiddenField)item.FindControl("hdnRefundId");

                    coll.Add(common.myInt(hdnRefundId.Value));//DumpRefundId INT, 
                    coll.Add(common.myInt(ddlApprovalStatus.SelectedValue));//ApprovalStatusId INT,
                    coll.Add(common.myStr(txtApprovalRemarks.Text));//ApprovalRemarks VARCHAR(255)

                    strXMLItems.Append(common.setXmlTable(ref coll));
                }
            }

            string strMsg = objR.SaveRefundApprovalDump(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
                                                strXMLItems.ToString(), common.myInt(Session["UserId"]), "uspSaveRefundApprovalDump");

            if ((strMsg.ToUpper().Contains(" UPDATE") || strMsg.ToUpper().Contains(" SAVE")) && !strMsg.ToUpper().Contains("USP"))
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                bindData();

                lblMessage.Text = strMsg;
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
            objR = null;
        }
    }

    protected void ddlApproved_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            bindData();
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

    protected void saveData(string saveEvent, int TransferId, int RegistrationId, int EncounterId, string Remarks, int OTBookingID = 0)
    {
        BaseC.clsOTBooking objCls = new BaseC.clsOTBooking(sConString);
        try
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);

            string strMsg = objCls.savePatientTransferWardtoOT(saveEvent, TransferId, common.myInt(Session["HospitalLocationID"]),
                                common.myInt(Session["FacilityId"]), RegistrationId, EncounterId,
                                Remarks, common.myInt(Session["UserID"]), 0);

            if ((strMsg.ToUpper().Contains(" UPDATE") || strMsg.ToUpper().Contains(" SAVE")) && !strMsg.ToUpper().Contains("USP"))
            {
                if (OTBookingID > 0)
                {
                    BaseC.RestFulAPI objwcfOt = new BaseC.RestFulAPI(sConString);
                    objwcfOt.ChangeOTStatus(common.myInt(Session["HospitalLocationID"]), OTBookingID, "OT-CHKIN",
                        common.myInt(Session["FacilityId"]), common.myInt(Session["UserID"]));
                }
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);

                bindData();
            }

            lblMessage.Text = strMsg;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            objCls = null;
        }
    }
    protected void ddlName_OnTextChanged(object sender, EventArgs e)
    {
        txtSearch.Text = "";
        txtSearchN.Text = "";

        txtSearchN.Visible = false;
        txtSearch.Visible = false;

        if (common.myStr(ddlName.SelectedValue) == "R")
        {
            txtSearchN.Visible = true;
        }
        else
        {
            txtSearch.Visible = true;
        }
        // bindStatus(rblSearchCriteria.SelectedValue);
    }
}
