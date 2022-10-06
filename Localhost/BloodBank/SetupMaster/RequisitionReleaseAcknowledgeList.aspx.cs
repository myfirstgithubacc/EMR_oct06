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

public partial class RequisitionReleaseAcknowledgeList : System.Web.UI.Page
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
            if (Request.QueryString["Regid"] != null)
            {
                hdnEncounterId.Value = (common.myInt(Request.QueryString["EncId"])).ToString();
                
                CreateTable(Convert.ToInt32(Request.QueryString["Regid"]));
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
        if (Request.QueryString["Regid"] != null)
        {
            CreateTable(Convert.ToInt32(Request.QueryString["Regid"]));
        }
        else
        {
            CreateTable(0);
        }
    }

    protected void btnClearSearch_OnClick(Object sender, EventArgs e)
    {        
        txtSearch.Text = "";
        lblMessage.Text = "";
        ddlSearchOn.SelectedIndex = 0;
        txtEncounter.Text = "";
        txtPatientName.Text = "";
        txtEncounter.Text = "";
        txtUHID.Text = "";
    }

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
    }
   

  

   

    void CreateTable(int RegistatrationID)
    {       
        DataSet ds = new DataSet();       
        objBb = new BaseC.clsBb(sConString);
        try
        {
            //objBb.  21-07-2014
            ds = objBb.GetBloodReleaseRequestion(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), 0, common.myStr(ddlSearchOn.SelectedValue), 1, common.myInt(RegistatrationID), common.myInt(hdnEncounterId.Value), common.myStr(txtUHID.Text.Trim()), common.myStr(txtEncounter.Text.Trim()), common.myStr(txtPatientName.Text.Trim()));
           // ds = objBb.GetBloodReleaseRequestion(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), 0, common.myStr(ddlSearchOn.SelectedValue), 1, 0, common.myInt(hdnEncounterId.Value), common.myStr(txtUHID.Text.Trim()), common.myStr(txtEncounter.Text.Trim()), common.myStr(txtPatientName.Text.Trim()));
            if (ds.Tables[0].Rows.Count == 0)
            {
                DataRow dr = ds.Tables[0].NewRow();
                ds.Tables[0].Rows.Add(dr);
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
        }

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


    protected void btnCloseW_Click(object sender, EventArgs e)
    {

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
