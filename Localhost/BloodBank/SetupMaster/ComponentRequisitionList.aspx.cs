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
using System.Text;
using System.Data.SqlClient;
using System.Threading;
using System.Globalization;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Web.SessionState;
using Telerik.Web.UI;

public partial class ComponentRequisitionList : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    BaseC.clsBb objBb;

    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Font.Bold = commonLabelSetting.cBold;
            bindControl();
            if (commonLabelSetting.cFont != "")
            {
                lblMessage.Font.Name = commonLabelSetting.cFont;
            }
            txtFromDate.DateInput.DateFormat = common.myStr(Session["OutputDateFormat"]);
            txtToDate.DateInput.DateFormat = common.myStr(Session["OutputDateFormat"]);

            if (Request.QueryString["AckStatus"] == "Ack")
            {
                ddlStatus.SelectedValue = "RUK";
            }
            else
            {
                ddlStatus.SelectedValue = "SUK";
            }

            if (common.myInt(Request.QueryString["Regid"]) != 0)
            {
                hdnEncounterId.Value = (common.myInt(Request.QueryString["EncId"])).ToString();
                CreateTable(Convert.ToInt32(Request.QueryString["Regid"]));
            }
            else if (Request.QueryString["From"] != null)
            {
                if (Request.QueryString["From"].Equals("ReleaseBloodBank"))
                {

                    CreateTable(-1);
                }
            }
            else
            {
                CreateTable(0);
            }
        }

    }


    private void clearControl()
    {
        txtSearch.Text = "";
        lblMessage.Text = "&nbsp;";

    }

    protected void btnSearch_OnClick(Object sender, EventArgs e)
    {
        //bindData();
        CreateTable(0);
    }

    protected void btnClearSearch_OnClick(Object sender, EventArgs e)
    {
        txtSearch.Text = "";
        lblMessage.Text = "";
        ddlSearchOn.SelectedIndex = 0;
    }


    //Added on 08-07-2014  Start  By Naushad
    protected void btnPrintLabel_Click(Object sender, EventArgs e)
    {
        if (common.myStr(hdn_EncounterID.Value) != "")
        {

            RadWindow1.NavigateUrl = "/EMRReports/ReportBarCode.aspx?EncNo=" + common.myStr(hdn_EncounterID.Value) + "&RequsitionID=" + common.myStr(hdnRequisition.Value) + "&ReportName=BloodBarCodeLabel";
            RadWindow1.Width = 850;
            RadWindow1.Top = 40;
            RadWindow1.Left = 100;
            RadWindow1.VisibleOnPageLoad = true;
            RadWindow1.Modal = true;
            RadWindow1.VisibleStatusbar = false;
        }
    }
    //Added on 08-07-2014  End By Naushad


    protected void gvEncounter_OnItemDataBound(object sender, GridItemEventArgs e)
    {
    }
    protected void gvEncounter_OnPageIndexChanged(object sender, GridPageChangedEventArgs e)
    {
        gvEncounter.CurrentPageIndex = e.NewPageIndex;
        if (Request.QueryString["Regid"] != null)
        {
            CreateTable(Convert.ToInt32(Request.QueryString["Regid"]));
        }
        else
        {
            CreateTable(0);
        }
    }

    protected void gvEncounter_OnItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Select")
            {
                hdnRequisition.Value = common.myStr(((HiddenField)e.Item.FindControl("hdnRequisitionId")).Value);
                hdnRelease.Value = common.myStr(((HiddenField)e.Item.FindControl("hdnReleaseID")).Value);
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "returnToParent();", true);
                return;
            }

            if (e.CommandName == "Print")
            {
                hdn_EncounterID.Value = common.myStr(((HiddenField)e.Item.FindControl("hdnEncounterId")).Value);
                hdnRequisition.Value = common.myStr(((HiddenField)e.Item.FindControl("hdnRequisitionId")).Value);
                btnPrintLabel_Click(sender, e);
            }

            if (e.CommandName == "Ack")
            {
                hdnRequisition.Value = common.myStr(((HiddenField)e.Item.FindControl("hdnRequisitionId")).Value);
                ViewState["RequisitionID"] = hdnRequisition.Value;
                dvConfirm.Visible = true;
                //btnYes_OnClick
            }




        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    //Added on 30-08-2014 Start Naushad

    protected void btnCancel_OnClick(object sender, EventArgs e)
    {
        dvConfirm.Visible = false;
    }



    protected void btnYes_OnClick(object sender, EventArgs e)
    {
        int i = UpdateBBComponentRequsitionMainAck();
        dvConfirm.Visible = false;
        btnSearch_OnClick(null, null);
    }

    //Added on 30-08-2014 End Naushad


    void CreateTable(int RegistatrationID)
    {
        DataSet ds = new DataSet();
        objBb = new BaseC.clsBb(sConString);
        try
        {

            //Added on 30-08-2014 Start Naushad
            string AckStatus = "";
            string Ptype = "";

            if (Request.QueryString["AckStatus"] == "Ack")
            {
                AckStatus = "N";
                if (Request.QueryString["Ptype"] != null)
                {
                    Ptype = common.myStr(Request.QueryString["Ptype"]);
                }
            }
            //gvEncounter.DataSource = ds.Tables[0];
            //Added on 30-08-2014 End Naushad


            if (RegistatrationID == 0)
            {
                ds = objBb.GetComponentRequisition(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), 0, common.myLong(txtUHID.Text), common.myStr(ddlSearchOn.SelectedValue), 1, common.myInt(hdnEncounterId.Value), common.myStr(txtEncounter.Text), common.myStr(txtPatientName.Text), AckStatus, Ptype, common.myStr(ddlStatus.SelectedValue), common.myStr(drpActiveInActive.SelectedValue));
                if (ds.Tables.Count > 0)
                {
                    if (common.myLen(Request.QueryString["WardName"]) > 0)
                    {
                        DataView DV = ds.Tables[0].Copy().DefaultView;
                        DV.RowFilter = "Ward like '%"+ common.myStr(Request.QueryString["WardName"]) + "%'";

                        ds = new DataSet();
                        ds.Tables.Add(DV.ToTable().Copy());
                        DV.Dispose();
                    }
                }
            }
            else if (RegistatrationID == -1)
            {
                ds = objBb.GetComponentRequisitionForReleaseAknowledgeList(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), 0, common.myStr(ddlSearchOn.SelectedValue), 1, 0, 1);
            }
            else
            {
                ds = GetComponentRequisition(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), 0, common.myLong(txtUHID.Text), common.myStr(ddlSearchOn.SelectedValue), 1, common.myInt(hdnEncounterId.Value), common.myStr(txtEncounter.Text), common.myStr(txtPatientName.Text));
            }
            if (ds.Tables.Count > 0)
            {
                int totalRecord = common.myInt(ds.Tables[0].Rows.Count);

                if (ds.Tables[0].Rows.Count == 0)
                {
                    DataRow dr = ds.Tables[0].NewRow();
                    ds.Tables[0].Rows.Add(dr);
                }

                gvEncounter.DataSource = ds.Tables[0];
                gvEncounter.DataBind();
                //Added on 30-08-2014 Start Naushad
                if (Request.QueryString["AckStatus"] != "Ack")
                {
                    gvEncounter.Columns.FindByUniqueName("Acknowledge").Visible = false;
                }

                if (Request.QueryString["Ptype"] == "I")
                {
                    gvEncounter.Columns.FindByUniqueName("Select").Visible = false;
                }
                lblTotRecord.ForeColor = System.Drawing.Color.Red;
                lblTotRecord.Text = "Total Request (" + totalRecord + ")";
                //Added on 30-08-2014 End  Naushad
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

    //add by prashant 04/05/2014
    public DataSet GetComponentRequisition(int HospitalLocationId, int FacilityId, int RequisitionId, long RegistrationId, string RequestType, int Active, int EncounterId, string EncounterNo, string PatientName)
    {
        DataSet ds = new DataSet();
        try
        {
            Hashtable HshIn = new Hashtable();
            HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@intRequisitionId", RequisitionId);
            HshIn.Add("@chvRequestType", RequestType);
            HshIn.Add("@bitActive", Active);
            HshIn.Add("@intRegistrationId", RegistrationId);
            HshIn.Add("@intEncounterId", EncounterId);
            HshIn.Add("@chvEncounterNo", EncounterNo);
            HshIn.Add("@chvPatienName", PatientName);
            HshIn.Add("@chrStatus", common.myStr(ddlStatus.SelectedValue));
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetComponentRequisitionList", HshIn);
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
        return ds;
    }



    //Added on 30-08-2014 Start Naushad Ali
    public int UpdateBBComponentRequsitionMainAck()
    {
        Hashtable hshInput = new Hashtable();
        DataSet ds = new DataSet();
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        int intRequistionID = common.myInt(ViewState["RequisitionID"]);
        string sQuery = "Update BBComponentRequisitionMain SET RequestAcknowledged=1,LastChangedBy=" + common.myInt(Session["UserID"]) + ",LastChangedDate=GETUTCDATE() WHERE RequisitionId=" + intRequistionID;
        int i = objDl.ExecuteNonQuery(CommandType.Text, sQuery, hshInput);
        return i;
    }


    protected void ddlSearchOn_SelectedIndexChanged(object sender, EventArgs e)
    {
        //txtSearch.Text = "";   
        if (Request.QueryString["Regid"] != null)
        {
            CreateTable(Convert.ToInt32(Request.QueryString["Regid"]));
        }
        else
        {
            CreateTable(0);
        }
    }
    protected void drpActiveInActive_SelectedIndexChanged(object sender, EventArgs e)
    {
        //txtSearch.Text = "";   
        if (Request.QueryString["Regid"] != null)
        {
            CreateTable(Convert.ToInt32(Request.QueryString["Regid"]));
        }
        else
        {
            CreateTable(0);
        }
    }


    protected void btnCloseW_Click(object sender, EventArgs e)
    {
        Session["FacilityId"] = ddlLocation.SelectedValue.ToString();
    }

    protected void gvEncounter_SelectedIndexChanged(object sender, EventArgs e)
    {

        GridDataItem item = (GridDataItem)gvEncounter.SelectedItems[0];

        //GridViewRow row=(GridViewRow)sender) 
        hdn_EncounterID.Value = ((HiddenField)item.FindControl("hdnEncounterId")).Value;
        hdnRequisition.Value = ((HiddenField)item.FindControl("hdnRequisitionId")).Value;

        //hdn_EncounterID.Value = common.myStr(((HiddenField)item.FindControl("hdnEncounterId.Value")).Value);         

    }


    protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
    {

        if (Request.QueryString["Regid"] != null)
        {
            CreateTable(Convert.ToInt32(Request.QueryString["Regid"]));
        }
        else
        {
            CreateTable(0);
        }
    }
    private void bindControl()
    {
        try
        {
            BaseC.EMRMasters.EMRFacility objEMRFacility = new BaseC.EMRMasters.EMRFacility(sConString);
            DataSet ds = objEMRFacility.GetFacility(Convert.ToInt16(Session["HospitalLocationId"]));
            DataView dv;
            dv = ds.Tables[0].DefaultView;
            dv.RowFilter = "Active = 1 ";
            ddlLocation.DataSource = dv;
            ddlLocation.DataTextField = "Name";
            ddlLocation.DataValueField = "FacilityID";
            ddlLocation.DataBind();
            ddlLocation.SelectedValue = Session["FacilityId"].ToString();
            ListItem lst = new ListItem();
            bool tf = true;
            bool tEncounter = true;
            if (common.myInt(Request.QueryString["RegEnc"]) == 1)
            {
                tf = false;
            }
            if (common.myInt(Request.QueryString["RegEnc"]) == 0)
            {
                tEncounter = false;
            }

            lst = new ListItem("DonorRegistration", "0", tf);
            //rdoRegEnc.Items.Add(lst);
            lst = new ListItem("Encounter", "1", tEncounter);
            //rdoRegEnc.Items.Add(lst);

            lst = new ListItem("Discharge", "2", true);

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }
    protected void ddlLocation_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        if (ddlLocation.SelectedValue != null)
        {
            if (IsPostBack)
                Session["FacilityId"] = ddlLocation.SelectedValue.ToString();

            // bindControl();
            if (Request.QueryString["Regid"] != null)
            {
                CreateTable(Convert.ToInt32(Request.QueryString["Regid"]));
            }
            else
            {
                CreateTable(0);
            }
        }
    }

}
