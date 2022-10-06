using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Collections;
using Telerik.Web.UI;

public partial class AckNonDrugOrder : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    
    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
        if (common.myStr(Request.QueryString["POPUP"]).ToUpper() == "POPUP")
        {
            this.MasterPageFile = "~/Include/Master/BlankMaster.master";
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            txtPrescription.Focus();
            Page.SetFocus(txtPrescription.ClientID);
            BindPatientHiddenDetails();
            FillSessionAndQuesryStirnValue();
            dtpdate.SelectedDate = DateTime.Now;
            dtpAckdate.SelectedDate = DateTime.Now;
            BindDoctor();
            bindFavouriteNonDrugOrder(string.Empty);
            BindGrid();
            ddlStatus.Visible = false;
            lblStatus.Visible = false;
            if (common.myStr(ViewState["EmployeeType"]).Equals("D") || common.myStr(ViewState["EmployeeType"]).Equals("TM"))
            {
                pnlNurse.Visible = false;
                gvData.Columns[5].Visible = false;
                gvData.Columns[6].Visible = false;
                gvData.Columns[7].Visible = false;
                btnSave.Text = "Save (Ctrl+F3)";
                if (common.myStr(Request.QueryString["POPUP"]).ToUpper() == "POPUP")
                {
                    pnlDoctor.Enabled = false;
                    btnSave.Visible = false;
                    //btnNew.Visible = false;
                }
            }
            else if (common.myStr(ViewState["EmployeeType"]).Equals("N"))
            {
                BindNurse();
                pnlDoctor.Enabled = false;
                pnlNurse.Visible = true;
                gvData.Columns[5].Visible = true;
                gvData.Columns[6].Visible = true;
                gvData.Columns[7].Visible = true;
                btnSave.Text = "Acknowledge (F3)";
            }
            else
            {
                pnlNurse.Visible = false;
                gvData.Columns[5].Visible = false;
                gvData.Columns[6].Visible = false;
                gvData.Columns[7].Visible = false;
                btnSave.Text = "Save (Ctrl+F3)";
                btnSave.Visible = false;
                //btnNew.Visible = false;
            }
            if ((common.myStr(Request.QueryString["Ward"]) == "Y" || common.myStr(Request.QueryString["OPIP"]) == "O") && (common.myStr(ViewState["EmployeeType"]).Equals("D") || common.myStr(ViewState["EmployeeType"]).Equals("TM")))
            {
                btnSave.Visible = true;
            }

        }
        if (common.myStr(Session["EncounterStatus"]).ToUpper().Contains("CLOSE") == true)
        {
            btnSave.Visible = false;
            //btnNew.Visible = false;

        }
    }
    void BindPatientHiddenDetails()
    {
        if (Session["PatientDetailString"] != null)
        {
            lblPatientDetail.Text = Session["PatientDetailString"].ToString();
        }
    }

    void FillSessionAndQuesryStirnValue()
    {
        // ViewState["RegistrationNo"] = common.myStr(Session["RegistrationNo"]);
        ViewState["RegistrationId"] = common.myStr(Session["RegistrationId"]);
        ViewState["EncounterId"] = common.myStr(Session["EncounterId"]);
        ViewState["EncounterNo"] = common.myStr(Session["EncounterNo"]);
    }
    private void BindDoctor()
    {
        try
        {
            BaseC.ICM ObjIcm = new BaseC.ICM(sConString);
            DataSet ds = new DataSet();
            ds = ObjIcm.GetICMSignDoctors(Convert.ToInt16(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]));
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlDoctor.DataSource = ds.Tables[0];
                ddlDoctor.DataTextField = "DoctorName";
                ddlDoctor.DataValueField = "ID";
                ddlDoctor.DataBind();
            }
            ds.Dispose();
            ddlDoctor.SelectedValue = common.myStr(Session["EmployeeId"]);
        }
        catch (Exception ex)
        {
            lblMessage.Text = ex.Message;
            lblMessage.ForeColor = System.Drawing.Color.Red;
        }
    }
    private void BindNurse()
    {
        BaseC.ICM objICM = new BaseC.ICM(sConString);

        try
        {
            DataSet ds = new DataSet();
            ds = objICM.GetICMNurse(common.myInt(Session["FacilityId"]));
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlNurse.DataSource = ds.Tables[0];
                ddlNurse.DataTextField = "NurseName";
                ddlNurse.DataValueField = "ID";
                ddlNurse.DataBind();
            }
            ds.Dispose();
            ddlNurse.SelectedValue = common.myStr(Session["EmployeeId"]);
        }
        catch (Exception ex)
        {
            lblMessage.Text = ex.Message;
            lblMessage.ForeColor = System.Drawing.Color.Red;
        }
        finally
        {
            objICM = null;
        }
    }
    void BindGrid()
    {
        BaseC.ICM objICM = new BaseC.ICM(sConString);
        DataSet ds = new DataSet();
        ds = objICM.NonDrugOrder(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]),
            common.myInt(Session["UserId"]), common.myInt(ViewState["RegistrationId"]), common.myInt(Session["EncounterId"]));
        if (ds.Tables.Count == 2)
        {
            ViewState["EmployeeType"] = common.myStr(ds.Tables[1].Rows[0]["EmployeeType"]).Trim();
            gvData.DataSource = ds.Tables[0];
            gvData.DataBind();
        }
        else
        {
            ViewState["EmployeeType"] = "";
            gvData.DataSource = null;
            gvData.DataBind();
        }
    }
    void Clear()
    {
        txtAckRemark.Text = "";
        // txtPrescription.Text = "";
        txtPrescription.Content = "";


        dtpdate.SelectedDate = DateTime.Now;
        dtpAckdate.SelectedDate = DateTime.Now;
        ddlStatus.Visible = false;
        lblStatus.Visible = false;
        hdnNonDrugOrder.Value = "";
        ddlOrderType.SelectedIndex = 0;
        ddlStatus.SelectedValue = "1";
        lblMessage.Text = "";
        lblModifyBy.Visible = false;
        lblModBy.Visible = false;
        lblModDate.Visible = false;
        lblModifyDate.Visible = false;
        lblModifyBy.Text = "";
        lblModifyDate.Text = "";
        if (common.myStr(ViewState["EmployeeType"]).Equals("N"))
        {
            ddlNurse.SelectedValue = common.myStr(Session["EmployeeId"]);
        }
        else if (common.myStr(ViewState["EmployeeType"]).Equals("D") || common.myStr(ViewState["EmployeeType"]).Equals("TM"))
        {
            ddlDoctor.SelectedValue = common.myStr(Session["EmployeeId"]);
            pnlDoctor.Enabled = true;
            pnlNurse.Visible = false;
            btnSave.Visible = true;
            if (common.myStr(Request.QueryString["POPUP"]).ToUpper() == "POPUP")
            {
                pnlDoctor.Enabled = false;
                btnSave.Visible = false;
            }

            lblAck.Visible = false;
            lblAcknowledgeBy.Visible = false;
            lblAckDate.Visible = false;
            lblAcknowledgeDate.Visible = false;
            lblAckRem.Visible = false;
            lblAckRemark.Visible = false;
            lblAcknowledgeBy.Text = "";
            lblAcknowledgeDate.Text = "";
            lblAckRemark.Text = "";


        }
    }
    protected void btnSave_OnClick(object sender, EventArgs e)
    {
        if (common.myStr(ViewState["EmployeeType"]).Equals("N") && common.myInt(hdnNonDrugOrder.Value) == 0)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Please select prescription of patient!!";
            return;
        }

        if (txtPrescription.Content.Trim() == "" && (common.myStr(ViewState["EmployeeType"]).Equals("D") || common.myStr(ViewState["EmployeeType"]).Equals("TM")))
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Please enter prescription of patient!!";
            return;
        }
        if (ddlDoctor.SelectedValue == "0" && (common.myStr(ViewState["EmployeeType"]).Equals("D") || common.myStr(ViewState["EmployeeType"]).Equals("TM")))
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Please select doctor!!";
            return;
        }
        if (txtAckRemark.Text.Trim() == "" && common.myStr(ViewState["EmployeeType"]).Equals("N"))
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Please enter acknowledge remark!!";
            return;
        }
        BaseC.ICM objICM = new BaseC.ICM(sConString);
        Hashtable hshOutput = new Hashtable();
        string OrderDate = "";
        string AckDate = "";
        int savefor = 0;
        if (common.myStr(ViewState["EmployeeType"]).Equals("D") || common.myStr(ViewState["EmployeeType"]).Equals("TM"))
        {
            savefor = 1;//save for Doctor
        }
        else if (common.myStr(ViewState["EmployeeType"]).Equals("N"))
        {
            savefor = 2; // save for Nurse
        }
        OrderDate = common.myStr(dtpdate.SelectedDate);
        AckDate = common.myStr(dtpAckdate.SelectedDate);
        
        string strMsg = objICM.SaveNonDrugOrder(common.myInt(hdnNonDrugOrder.Value), Convert.ToInt16(Session["HospitalLocationID"]), common.myStr(ViewState["RegistrationId"]), common.myStr(ViewState["EncounterId"]),
            OrderDate,
           // txtPrescription.Text
           txtPrescription.Content
            , common.myStr(ddlOrderType.SelectedValue), common.myInt(ddlDoctor.SelectedValue), common.myInt(ddlNurse.SelectedValue), AckDate, txtAckRemark.Text,
            Convert.ToInt32(Session["UserID"]), common.myInt(Session["FacilityId"]), common.myBool(common.myInt(ddlStatus.SelectedValue)), savefor, false);// 1 for doctor
        if ((strMsg.ToUpper().Contains(" UPDATE") || strMsg.ToUpper().Contains(" SAVE") || strMsg.ToUpper().Contains(" ACKNOWLEDGE")) && !strMsg.ToUpper().Contains("USP"))
        {
            BindGrid();
            Clear();
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
            lblMessage.Text = strMsg;
        }


    }
    protected void btnNew_OnClick(object sender, EventArgs e)
    {
        Clear();
    }
    protected void gvData_OnItemCommand(Object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        if (common.myStr(e.CommandName) == "Select")
        {
            lblMessage.Text = "";
            hdnNonDrugOrder.Value = common.myStr(((Label)e.Item.FindControl("lblNonDrugOrderId")).Text);

            dtpdate.SelectedDate = common.myDate(((Label)e.Item.FindControl("lblOrderDate")).Text);
            // txtPrescription.Text = (((Label)e.Item.FindControl("lblPrescription")).Text);
            txtPrescription.Content = (((Label)e.Item.FindControl("lblPrescription")).Text);
            ddlOrderType.SelectedValue = common.myStr(((HiddenField)e.Item.FindControl("hdnOrderType")).Value);
            ddlDoctor.SelectedValue = common.myStr(((HiddenField)e.Item.FindControl("hdnDoctorId")).Value);

            lblModifyBy.Text = (((Label)e.Item.FindControl("lblModifyBy")).Text);
            lblModifyDate.Text = (((Label)e.Item.FindControl("lblModifyDate")).Text);

            lblAcknowledgeBy.Text = (((Label)e.Item.FindControl("lblAcknowledgeBy")).Text);
            lblAcknowledgeDate.Text = (((Label)e.Item.FindControl("lblAcknowledgeDate")).Text);
            lblAckRemark.Text = (((Label)e.Item.FindControl("lblAcknowledgeRemarks")).Text);


            if ((((Label)e.Item.FindControl("lblModifyBy")).Text) != "")
            {
                lblModifyBy.Visible = true;
                lblModBy.Visible = true;
                lblModDate.Visible = true;
                lblModifyDate.Visible = true;
            }
            else
            {
                lblModifyBy.Visible = false;
                lblModBy.Visible = false;
                lblModDate.Visible = false;
                lblModifyDate.Visible = false;
                lblModifyBy.Text = "";
                lblModifyDate.Text = "";

            }
            if (common.myStr(ViewState["EmployeeType"]).Equals("D") || common.myStr(ViewState["EmployeeType"]).Equals("TM"))
            {
                ddlStatus.Visible = true;
                lblStatus.Visible = true;
                pnlNurse.Enabled = false;
                pnlDoctor.Enabled = true;
                btnSave.Visible = true;
                if (common.myStr(((HiddenField)e.Item.FindControl("hdnAcknowledge")).Value) == "True" || common.myStr(Request.QueryString["POPUP"]).ToUpper() == "POPUP")
                {
                    pnlDoctor.Enabled = false;
                    btnSave.Visible = false;
                }
                if (common.myStr(((HiddenField)e.Item.FindControl("hdnAcknowledge")).Value) == "True")
                {
                    lblAck.Visible = true;
                    lblAcknowledgeBy.Visible = true;
                    lblAckDate.Visible = true;
                    lblAcknowledgeDate.Visible = true;
                    lblAckRem.Visible = true;
                    lblAckRemark.Visible = true;
                    lblAcknowledgeBy.Text = lblAcknowledgeBy.Text;
                    lblAcknowledgeDate.Text = lblAcknowledgeDate.Text;
                    lblAckRemark.Text = lblAckRemark.Text;

                }
                else
                {
                    lblAck.Visible = false;
                    lblAcknowledgeBy.Visible = false;
                    lblAckDate.Visible = false;
                    lblAcknowledgeDate.Visible = false;
                    lblAckRem.Visible = false;
                    lblAckRemark.Visible = false;
                    lblAcknowledgeBy.Text = "";
                    lblAcknowledgeDate.Text = "";
                    lblAckRemark.Text = "";

                }
            }
            else if (common.myStr(ViewState["EmployeeType"]).Equals("N"))
            {
                pnlDoctor.Enabled = false;
                pnlNurse.Enabled = true;
                pnlNurse.Visible = true;
                btnSave.Visible = true;
                if (common.myStr(((HiddenField)e.Item.FindControl("hdnAcknowledge")).Value) == "True")
                {
                    pnlNurse.Enabled = false;
                    btnSave.Visible = false;
                }
            }
            if (common.myStr(((HiddenField)e.Item.FindControl("hdnAcknowledge")).Value) == "True")
            {
                dtpAckdate.SelectedDate = common.myDate(((Label)e.Item.FindControl("lblAcknowledgeDate")).Text);
                txtAckRemark.Text = common.myStr(((Label)e.Item.FindControl("lblAcknowledgeRemarks")).Text);
                ddlNurse.SelectedValue = common.myStr(((HiddenField)e.Item.FindControl("hdnNurseId")).Value);
            }
            else
            {
                dtpAckdate.SelectedDate = DateTime.Now;
                txtAckRemark.Text = "";
            }

            if (common.myStr(Session["EncounterStatus"]).ToUpper().Contains("CLOSE") == true)
            {
                //btnNew.Visible = false;
                btnSave.Visible = false;


            }
        }
    }
    protected void gvData_OnItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item.ItemType == GridItemType.Item
              || e.Item.ItemType == GridItemType.AlternatingItem)
        {
            HiddenField hdnAcknowledge = (HiddenField)e.Item.FindControl("hdnAcknowledge");
            LinkButton lbnSelect = (LinkButton)e.Item.FindControl("lbnSelect");
            if (hdnAcknowledge.Value == "True")
            {
                e.Item.BackColor = System.Drawing.Color.LightGreen;
            }
        }
    }

    protected void gvFav_OnRowCommand(object sender, GridViewCommandEventArgs e)
    {
        BaseC.ICM objICM = new BaseC.ICM(sConString);

        if (e.CommandName == "FAVLIST")
        {
            GridViewRow grd = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);

            //GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
            LinkButton lnkbtn = (LinkButton)grd.FindControl("lnkFavName");
            txtPrescription.Content = ((common.myLen(txtPrescription.Content) > 0) ? txtPrescription.Content + "<br />" : txtPrescription.Content) + lnkbtn.Text;
        }
        else if (e.CommandName == "Del")
        {
            GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
            HiddenField hdnFavId = (HiddenField)row.FindControl("hdnFavId");

            if (common.myInt(hdnFavId.Value) > 0)
            {
                objICM.DeleteICMFavouriteNonDrugOrder(common.myInt(Session["DoctorID"]), common.myInt(hdnFavId.Value));
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                lblMessage.Text = "Non drug order deleted from your favorite list";
            }
            else
            {
                Alert.ShowAjaxMsg("Select non drug order to delete from favorite list", Page);
            }

            bindFavouriteNonDrugOrder("");
        }
    }

    protected void bindFavouriteNonDrugOrder(string txt)
    {
        DataSet objDs = new DataSet();
        BaseC.ICM objICM = new BaseC.ICM(sConString);
        try
        {
            string strSearchCriteria = string.Empty;

            strSearchCriteria = "%" + txt + "%";

            objDs = (DataSet)objICM.getICMFavouriteNonDrugOrder(strSearchCriteria, common.myInt(Session["DoctorID"]));

            if (objDs.Tables[0].Rows.Count == 0)
            {
                DataRow DR = objDs.Tables[0].NewRow();
                objDs.Tables[0].Rows.Add(DR);
            }

            //gvFav.DataSource = objDs.Tables[0];
            // gvFav.DataBind();
            ViewState["FavList"] = objDs.Tables[0];
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            objDs.Dispose();
            objICM = null;
        }
    }

    protected void btnAddToFavourite_Click(object sender, EventArgs e)
    {
        BaseC.ICM objICM = new BaseC.ICM(sConString);
        try
        {
            if (ViewState["FavList"] != "")
            {
                DataTable dt = (DataTable)ViewState["FavList"];
                DataView dv = dt.DefaultView;
                if (dt.Rows.Count > 0)
                {
                    if (txtPrescription.Content != "")
                    {
                        //dv.RowFilter = "Description=" + common.myStr(txtProvisionalDiagnosis.Text);
                        //if (dv.Count > 0)
                        //{
                        //    Alert.ShowAjaxMsg("Diagnosis already exists in Favorites", this);
                        //    return;
                        //}
                    }
                }
            }

            string strmsg = objICM.SaveICMFavouriteNonDrugOrder(common.myInt(Session["DoctorID"]), common.myStr(txtPrescription.Content).Trim(), common.myInt(Session["UserID"]));

            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
            lblMessage.Text = strmsg;
            bindFavouriteNonDrugOrder("");

            //}
            //else
            //{
            //    Alert.ShowAjaxMsg("Select a Problem to Add in Favorites", this);
            //}
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            objICM = null;
        }
    }


}
