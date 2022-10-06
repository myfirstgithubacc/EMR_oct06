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

public partial class BloodAcknowledgeList : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    BaseC.clsBb objBb;
    clsExceptionLog objException = new clsExceptionLog();
    DataSet dsSearch;


    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            if (common.myStr(Request.QueryString["EncNo"]) != string.Empty)
            {
                hdnRegistrationID.Value = Convert.ToString(Request.QueryString["Regid"]);
                hdnRegistrationNo.Value = common.myStr(Request.QueryString["RegNo"]);
                hdnEncounterID.Value = Convert.ToString(Request.QueryString["EncId"]);
                hdnEncounterNo.Value = Convert.ToString(Request.QueryString["EncNo"]);
                txtUHID.Text = hdnRegistrationNo.Value;
                txtEncounter.Text = hdnEncounterNo.Value;
                txtEncounter.Enabled = false;
                txtUHID.Enabled = false;

            }
            hdnAck.Value = Request.QueryString["Ack"].ToString();

            bindData(common.myInt(hdnAck.Value));
            bindControl();
        }
    }

    protected void gvBagEntryDetailsList_OnPageIndexChanged(object sender, GridPageChangedEventArgs e)
    {
        gvBagEntryDetailsList.CurrentPageIndex = e.NewPageIndex;
        bindData(common.myInt(hdnAck.Value));
    }

    protected void gvBagEntryDetailsList_OnItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Select")
            {
                if (!common.myStr(((HiddenField)e.Item.FindControl("hdnCrossMatchNo")).Value).Equals(""))
                {
                    hdnCrossMatchNo.Value = common.myStr(((HiddenField)e.Item.FindControl("hdnCrossMatchNo")).Value);
                    hdnRequisition.Value = common.myStr(((HiddenField)e.Item.FindControl("hdnRequisition")).Value);
                    hdnComponentID.Value = common.myStr(((HiddenField)e.Item.FindControl("hdnComponentID")).Value);
                    hdnComponentIssueNo.Value = common.myStr(((HiddenField)e.Item.FindControl("hdnComponentIssueNo")).Value);
                    hdnCrossMatchId.Value = common.myStr(((HiddenField)e.Item.FindControl("hdnCrossMatchId")).Value);
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "returnToParent();", true);
                    return;
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



    private void bindData(int Acknowledged)
    {
        DataTable table = new DataTable();
        try
        {
            objBb = new BaseC.clsBb(sConString);

            table = objBb.GetBloodAcknowledgeDetails(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), 1, common.myInt(hdnRegistrationID.Value), common.myStr(txtUHID.Text.Trim()), common.myInt(hdnEncounterID.Value), common.myInt(Acknowledged), common.myStr(txtEncounter.Text.Trim()), common.myStr(txtPatientName.Text.Trim()), common.myStr(ddlSearchOn.SelectedValue)).Tables[0]; 

            if (table.Rows.Count > 0)
            {
                gvBagEntryDetailsList.DataSource = table;
            }
            else
            {
                DataRow DR = table.NewRow();
                table.Rows.Add(DR);
                gvBagEntryDetailsList.DataSource = table;
            }
            
            gvBagEntryDetailsList.DataBind();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            table.Dispose();
        }

    }



    protected void ddlSearchOn_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindData(common.myInt(hdnAck.Value));
    }
    
    protected void btnCloseW_Click(object sender, EventArgs e)
    {
            }

    protected void btnSearch_OnClick(Object sender, EventArgs e)
    {
        bindData(common.myInt(hdnAck.Value));
    }
    protected void btnClearSearch_OnClick(Object sender, EventArgs e)
    {
        txtSearch.Text = "";
        lblMessage.Text = "";
        ddlSearchOn.SelectedIndex = 0;
        txtUHID.Text = "";
        txtEncounter.Text = "";
        txtPatientName.Text = "";
    }
    protected void ddlLocation_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        if (ddlLocation.SelectedValue != null)
        {
            Session["FacilityId"] = ddlLocation.SelectedValue.ToString();
            // bindControl();
            bindData(common.myInt(hdnAck.Value));
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






}
