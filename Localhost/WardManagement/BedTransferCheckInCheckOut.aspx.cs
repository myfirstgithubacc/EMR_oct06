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
using System.Xml.Linq;
using Telerik.Web.UI;

public partial class WardManagement_AckByWard : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            this.Page.Title = "Bed Transfer Check Out / Check In";

            txtSearchRegNo.Visible = false;
            txtSearch.Visible = false;

            switch (common.myStr(Request.QueryString["SearchCriteria"]))
            {
                case "R":
                    txtSearchRegNo.Visible = true;
                    ddlSearchCriteria.SelectedValue = common.myStr(Request.QueryString["SearchCriteria"]);
                    txtSearchRegNo.Text = common.myStr(Request.QueryString["SearchValue"]);
                    break;
                case "ENC":
                    txtSearch.Visible = true;
                    ddlSearchCriteria.SelectedValue = common.myStr(Request.QueryString["SearchCriteria"]);
                    txtSearch.Text = common.myStr(Request.QueryString["SearchValue"]);
                    break;
                case "N":
                    txtSearch.Visible = true;
                    ddlSearchCriteria.SelectedValue = common.myStr(Request.QueryString["SearchCriteria"]);
                    txtSearch.Text = common.myStr(Request.QueryString["SearchValue"]);
                    break;
            }
            fetchData();
        }

    }

    protected void gvAckByWardDetails_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvAckByWardDetails.PageIndex = e.NewPageIndex;
        BindgvAckByWardDetails();
    }

    protected void ddlSearchCriteria_SelectedIndexChanged(object sender, EventArgs e)
    {

        txtSearchRegNo.Visible = false;
        txtSearch.Visible = false;

        switch (common.myStr(ddlSearchCriteria.SelectedValue))
        {
            case "R":
                txtSearchRegNo.Visible = true;
                break;
            case "ENC":
                txtSearch.Visible = true;
                break;
            case "N":
                txtSearch.Visible = true;
                break;
        }
    }

    protected void btnFilter_Click(object sender, EventArgs e)
    {
        try
        {
            fetchData();

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void fetchData()
    {
        try
        {
            BindgvAckByWardDetails();

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

    protected void btnClearFilter_Click(object sender, EventArgs e)
    {
        ddlSearchCriteria.SelectedIndex = 0;
        txtSearchRegNo.Text = string.Empty;
        txtSearch.Text = string.Empty;
        fetchData();
    }

    private void BindgvAckByWardDetails()
    {
        BaseC.WardManagement objWM = new BaseC.WardManagement();
        DataSet ds = new DataSet();
        int WardId = common.myInt(Request.QueryString["wardid"]);
        int RegistrationNo = 0;
        string EncounterNo = string.Empty;
        string PatientName = string.Empty;
        try
        {
            switch (common.myStr(ddlSearchCriteria.SelectedValue))
            {
                case "R":
                    RegistrationNo = common.myInt(txtSearchRegNo.Text.Trim());
                    break;
                case "ENC":
                    EncounterNo = txtSearch.Text.Trim();
                    break;
                case "N":
                    PatientName = txtSearch.Text.Trim();
                    break;
            }

            ds = objWM.BedTransferCheckInCheckOut(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), EncounterNo, WardId, RegistrationNo, PatientName);

            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        lblNoOfPatient.Text = "(" + common.myStr(ds.Tables[0].Rows.Count) + ")";
                        gvAckByWardDetails.DataSource = ds.Tables[0];
                        gvAckByWardDetails.DataBind();
                    }
                    else
                    {
                        BindBlankAckByWardDetails();
                    }
                }
                else
                {
                    BindBlankAckByWardDetails();
                }
            }
            else
            {
                BindBlankAckByWardDetails();
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
            objWM = null;
        }
    }

    private void BindBlankAckByWardDetails()
    {
        DataTable dt = CreateItemTable();
        DataRow dr = dt.NewRow();
        try
        {
            dr["EncodedDate"] = string.Empty;
            dr["RegistrationNo"] = string.Empty;
            dr["EncounterNo"] = string.Empty;
            dr["PatientName"] = string.Empty;
            dr["AgeGender"] = string.Empty;
            dr["WardName"] = string.Empty;
            dr["Fromward"] = string.Empty;
            dr["FromBedNo"] = string.Empty;
            dr["Toward"] = string.Empty;
            dr["ToBedNo"] = string.Empty;
            dr["RequestRemarks"] = string.Empty;
            dr["Id"] = 0;
            dr["EncounterId"] = 0;


            dr["CheckOutBy"] = 0;
            dr["CheckInBy"] = 0;
            dr["frombedcategoryid"] = 0;
            dr["frombillingcategoryid"] = 0;
            dr["frombedid"] = 0;
            dr["fromwardid"] = 0;
            dr["tobedcategoryid"] = 0;
            dr["tobillingcategoryid"] = 0;
            dr["tobedid"] = 0;
            dr["towardid"] = 0;
            dr["registrationid"] = 0;
            dr["CheckOut"] = false;
            dr["CheckIn"] = false;
            dr["ConsultingDoctor"] = string.Empty;
            dr["CheckOutDateTime"] = string.Empty;
            dr["IsApproval"] = 0;

            dt.Rows.Add(dr);
            dt.AcceptChanges();

            gvAckByWardDetails.DataSource = dt;
            gvAckByWardDetails.DataBind();

            //GridViewRow gv1 = gvAckByWardDetails.Rows[0];

            //ImageButton ibtnDelete = (ImageButton)gv1.FindControl("ibtnDelete");
            //ibtnDelete.Enabled = false;
            //ibtnDelete.Visible = false;
        }

        catch (Exception ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + ex.Message;
            objException.HandleException(ex);

        }
        finally
        {
            dt.Dispose();
        }
    }

    protected DataTable CreateItemTable()
    {
        DataTable dt = new DataTable();
        try
        {
            dt.Columns.Add("EncodedDate", typeof(string));
            dt.Columns.Add("RegistrationNo", typeof(string));
            dt.Columns.Add("EncounterNo", typeof(string));
            dt.Columns.Add("PatientName", typeof(string));
            dt.Columns.Add("AgeGender", typeof(string));
            dt.Columns.Add("WardName", typeof(string));
            dt.Columns.Add("Fromward", typeof(string));
            dt.Columns.Add("FromBedNo", typeof(string));
            dt.Columns.Add("Toward", typeof(string));
            dt.Columns.Add("ToBedNo", typeof(string));
            dt.Columns.Add("RequestRemarks", typeof(string));
            dt.Columns.Add("Id", typeof(int));
            dt.Columns.Add("EncounterId", typeof(int));


            dt.Columns.Add("CheckOutBy", typeof(int));
            dt.Columns.Add("CheckInBy", typeof(int));
            dt.Columns.Add("frombedcategoryid", typeof(int));
            dt.Columns.Add("frombillingcategoryid", typeof(int));
            dt.Columns.Add("frombedid", typeof(int));
            dt.Columns.Add("fromwardid", typeof(int));
            dt.Columns.Add("tobedcategoryid", typeof(int));
            dt.Columns.Add("tobillingcategoryid", typeof(int));
            dt.Columns.Add("tobedid", typeof(int));
            dt.Columns.Add("towardid", typeof(int));
            dt.Columns.Add("registrationid", typeof(int));
            dt.Columns.Add("CheckOut", typeof(bool));
            dt.Columns.Add("CheckIn", typeof(bool));
            dt.Columns.Add("ConsultingDoctor", typeof(string));
            dt.Columns.Add("CheckOutDateTime", typeof(string));
            dt.Columns.Add("IsApproval", typeof(int));

            return dt;
        }
        catch (Exception ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + ex.Message;
            objException.HandleException(ex);
            return dt;
        }

    }

    protected void gvAckByWardDetails_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Details")
            {
                GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                //int EncounterId = common.myInt(((HiddenField)row.FindControl("hdnEncounterId")).Value.ToString());

                //ViewState["EncounterId"] = EncounterId;
                int Id = common.myInt(((HiddenField)row.FindControl("hdnId")).Value.ToString());

                int FromBedCategoryId = common.myInt(((HiddenField)row.FindControl("hdnFromBedCategoryId")).Value.ToString());
                int FromBillingCategoryId = common.myInt(((HiddenField)row.FindControl("hdnFromBillingCategoryId")).Value.ToString());
                int FromBedId = common.myInt(((HiddenField)row.FindControl("hdnFromBedId")).Value.ToString());
                int FromWardId = common.myInt(((HiddenField)row.FindControl("hdnFromWardId")).Value.ToString());
                int ToBedCategoryid = common.myInt(((HiddenField)row.FindControl("hdnToBedCategoryid")).Value.ToString());
                int ToBillingCategoryid = common.myInt(((HiddenField)row.FindControl("hdnToBillingCategoryid")).Value.ToString());
                int ToBedId = common.myInt(((HiddenField)row.FindControl("hdnToBedId")).Value.ToString());
                int ToWardId = common.myInt(((HiddenField)row.FindControl("hdnToWardId")).Value.ToString());
                int RegistrationId = common.myInt(((HiddenField)row.FindControl("hdnRegistrationId")).Value.ToString());
                bool CheckOut = common.myBool(((HiddenField)row.FindControl("hdnCheckOut")).Value.ToString());
                bool CheckIn = common.myBool(((HiddenField)row.FindControl("hdnCheckIn")).Value.ToString());


                ViewState["Id"] = Id;

                ViewState["FromBedCategoryId"] = FromBedCategoryId;
                ViewState["FromBillingCategoryId"] = FromBillingCategoryId;
                ViewState["FromBedId"] = FromBedId;
                ViewState["FromWardId"] = FromWardId;
                ViewState["ToBedCategoryid"] = ToBedCategoryid;
                ViewState["ToBillingCategoryid"] = ToBillingCategoryid;
                ViewState["ToBedId"] = ToBedId;
                ViewState["ToWardId"] = ToWardId;
                ViewState["RegistrationId"] = RegistrationId;

                ViewState["CheckOut"] = CheckOut;
                ViewState["CheckIn"] = CheckIn;

                foreach (GridViewRow rownew in gvAckByWardDetails.Rows)
                {
                    rownew.BackColor = System.Drawing.Color.FromName("White");
                }
                row.BackColor = System.Drawing.Color.FromName("LightSkyBlue");

                GetRemarks(Id);
            }

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private void GetRemarks(int id)
    {
        BaseC.ATD oATD = new BaseC.ATD(sConString);
        DataSet oDATASET = new DataSet();
        oDATASET = oATD.getRemarksBedTransferRequest(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), id);
        if (oDATASET.Tables[0].Rows.Count > 0) { txtRemark.Text = common.myStr(oDATASET.Tables[0].Rows[0]["Remarks"]); }
    }

    //protected void btnAck_OnClick(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        BaseC.ATD oATD = new BaseC.ATD(sConString);
    //        int iId = common.myInt(ViewState["Id"]);
    //        string sRemarks = common.myStr(txtRemark.Text);
    //        int iReturn = oATD.UpdateACKByWard(iId, sRemarks);
    //        if (iReturn == 0) { lblMessage.Visible = true; lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor); lblMessage.Text = "Remarks Updated !"; txtRemark.Text = ""; }
    //        oATD = null;
    //        fetchData();
    //    }
    //    catch (Exception Ex)
    //    {
    //        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
    //        lblMessage.Text = "Error: " + Ex.Message;
    //        objException.HandleException(Ex);
    //    }
    //}



    protected void btnCheckOut_Click(object sender, EventArgs e)
    {
        try
        {

            if (common.myBool(ViewState["CheckOut"]))
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Patient is already check out";
                Alert.ShowAjaxMsg(common.myStr(lblMessage.Text), Page);
                return;
            }

            if (common.myInt(ViewState["Id"]).Equals(0))
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Please select patient";
                Alert.ShowAjaxMsg(common.myStr(lblMessage.Text), Page);
                return;
            }

            if (common.myInt(ViewState["ToBedCategoryid"]).Equals(0) || common.myInt(ViewState["ToBillingCategoryid"]).Equals(0) || common.myInt(ViewState["ToBedId"]).Equals(0) || common.myInt(ViewState["ToWardId"]).Equals(0))
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Either  Bed or Ward or BedCategory or BillingCategory Transfer not filled ";
                Alert.ShowAjaxMsg(common.myStr(lblMessage.Text), Page);
                return;
            }

            BaseC.ATD oATD = new BaseC.ATD(sConString);
            int iId = common.myInt(ViewState["Id"]);
            string sRemarks = common.myStr(txtRemark.Text);
            int iReturn = oATD.UpdateBedTransferReqCheckOut(iId, sRemarks, common.myInt(Session["UserId"]));
            if (iReturn == 0) { lblMessage.Visible = true; lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor); lblMessage.Text = "Check Out successful !"; txtRemark.Text = ""; }
            oATD = null;
            fetchData();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void btnCheckIn_Click(object sender, EventArgs e)
    {
        try
        {

            if (common.myInt(ViewState["Id"]).Equals(0))
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Please select patient";
                Alert.ShowAjaxMsg(common.myStr(lblMessage.Text), Page);
                return;
            }

            if (common.myInt(ViewState["ToBedCategoryid"]).Equals(0) || common.myInt(ViewState["ToBillingCategoryid"]).Equals(0) || common.myInt(ViewState["ToBedId"]).Equals(0) || common.myInt(ViewState["ToWardId"]).Equals(0) || !common.myBool(ViewState["CheckOut"]))
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Check out not done";
                Alert.ShowAjaxMsg(common.myStr(lblMessage.Text), Page);
                return;
            }


            BaseC.ATD oATD = new BaseC.ATD(sConString);

            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable hshtableout = new Hashtable();
            Hashtable hshtablein = new Hashtable();



            int iId = common.myInt(ViewState["Id"]);


            #region Bed Transfer
            hshtablein.Add("@TransferRequistionId", iId);
            hshtablein.Add("@HospitalLocationId", common.myInt(Session["HospitalLocationID"]));
            hshtablein.Add("@RegistrationId", common.myInt(ViewState["RegistrationId"]));


            hshtablein.Add("@FromBedCategoryId", common.myInt(ViewState["FromBedCategoryId"]));
            hshtablein.Add("@FromBillCategoryId", common.myInt(ViewState["FromBillingCategoryId"]));


            hshtablein.Add("@FromBedId", common.myInt(ViewState["FromBedId"]));
            hshtablein.Add("@FromWardId ", common.myInt(ViewState["FromWardId"]));
            hshtablein.Add("@ToBedCategoryId", common.myInt(ViewState["ToBedCategoryid"]));
            hshtablein.Add("@ToBillCategoryId", common.myInt(ViewState["ToBillingCategoryid"]));
            hshtablein.Add("@ToBedId", common.myInt(ViewState["ToBedId"]));
            hshtablein.Add("@ToWardId", common.myInt(ViewState["ToWardId"]));
            hshtablein.Add("@UserId", common.myInt(Session["UserId"])); // To swapping between current userid & transaction userid);
            hshtablein.Add("@TransferDate", DateTime.Now);
            hshtableout.Add("@StrStatus", SqlDbType.VarChar);
            hshtablein.Add("@intfacilityId", common.myStr(Session["FacilityId"]));

            //if (common.myStr(Request.QueryString["ER"]) == "ER")
            //{
            //    hshtablein.Add("@encounterid", common.myStr(Request.QueryString["encid"]));
            //    hshtableout = dl.getOutputParametersValues(CommandType.StoredProcedure, "UspSaveERBedTransfer", hshtablein, hshtableout);
            //}
            //else
            //{
            //    hshtablein.Add("@bIsBedRetail", (chkIsBedRetail.Checked == true) ? 1 : 0);
            hshtableout = dl.getOutputParametersValues(CommandType.StoredProcedure, "USPSaveBedTransfer", hshtablein, hshtableout);
            //  }
            lblMessage.ForeColor = System.Drawing.Color.Green;
            lblMessage.Text = common.myStr(hshtableout["@StrStatus"]);

            if (common.myStr(lblMessage.Text).ToUpper().Contains("SUCCESS"))
            {
                string sRemarks = common.myStr(txtRemark.Text);
                int iReturn = oATD.UpdateBedTransferReqCheckIn(iId, sRemarks, common.myInt(Session["UserId"]));
                if (iReturn == 0) { lblMessage.Visible = true; lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor); lblMessage.Text = lblMessage.Text; txtRemark.Text = ""; }
                oATD = null;
            }

            //Alert.ShowAjaxMsg(common.myStr(hshtableout["@StrStatus"]), Page);
            Alert.ShowAjaxMsg(common.myStr(lblMessage.Text), Page);

            #endregion


            fetchData();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void gvAckByWardDetails_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HiddenField hdnIsApproval = (HiddenField)e.Row.FindControl("hdnIsApproval");

                if (common.myInt(hdnIsApproval.Value).Equals(2))
                {
                    LinkButton lnkDetails = (LinkButton)e.Row.FindControl("lnkDetails");

                    lnkDetails.Visible = false;
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

}
