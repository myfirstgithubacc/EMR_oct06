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

public partial class WardManagement_HousekeepingRequest : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
        if (common.myStr(Request.QueryString["From"]).Equals("WardMenu"))
        {
            Page.MasterPageFile = "/Include/Master/EMRMaster.master";
        }
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            bool WardMenu = false;
            if ((Request.QueryString["From"] != null) && !(common.myStr(Request.QueryString["From"])).Equals(string.Empty))
            {
                if (Request.QueryString["From"].Equals("WardMenu"))
                {
                    WardMenu = true;
                }

            }
            ViewState["WardMenu"] = WardMenu;
            DataSet dsPatientDetail = new DataSet();
            bindControl();
            bindData();
            Bindward();
            BindBedName();
           
            if(WardMenu)
            {
               
                //divWardMenu.Attributes.Add("style", "display:inline");
                // ddlTemplateMain.SelectedIndex = ddlTemplateMain.Items.IndexOf(ddlTemplateMain.Items.FindItemByValue(common.myInt(dv.ToTable().Rows[0]["TemplateId"]).ToString()));
                ddlward.Enabled = true;
                ddlBedNo.Enabled = true;
                asplUD.Visible = false;

            }
            else
            {
                dsPatientDetail = (DataSet)Session["TopPanelPatientDetail" + common.myInt(Session["UserId"])];
                ddlward.SelectedIndex = ddlward.Items.IndexOf(ddlward.Items.FindItemByText(common.myStr(dsPatientDetail.Tables[0].Rows[0]["WardName"]).ToString()));
                ddlBedNo.SelectedIndex = ddlBedNo.Items.IndexOf(ddlBedNo.Items.FindItemByText(common.myStr(dsPatientDetail.Tables[0].Rows[0]["BedNo"].ToString())));
                ddlward.Enabled = false;
                ddlBedNo.Enabled = false;
            }
           
        }
    }


    protected void bindControl()
    {
        common objCommon = new common();
        DataSet ds = new DataSet();
        try
        {
            ds = objCommon.GetReasonMaster(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), "Housekeeping", sConString);

            ddlReasonId.DataSource = ds.Tables[0];
            ddlReasonId.DataTextField = "Reason";
            ddlReasonId.DataValueField = "Id";
            ddlReasonId.DataBind();

            ddlReasonId.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("", "0"));

            ddlReasonId.SelectedIndex = 0;
        }
        catch (Exception Ex)
        {
            lblMsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void btnSave_OnClick(object sender, EventArgs e)
    {
        BaseC.WardManagement objWard = new BaseC.WardManagement();
        string strMsg = string.Empty;
    

        try
        {
            lblMsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);

            if (common.myInt(ddlReasonId.SelectedValue).Equals(0))
            {
                lblMsg.Text = "Please define reason!";
                return;
            }
            //if(common.myInt( ddlward.SelectedValue).Equals(0) )
            //{
            //    lblMsg.Text = "Please select ward!";
            //    return;
            //}
            if (common.myInt(ddlBedNo.SelectedValue).Equals(0))
            {
                lblMsg.Text = "Please select Bed No.!";
                return;
            }
            if (common.myBool(ViewState["WardMenu"]))
            {
                strMsg = objWard.SaveUpdateHousekeepingRequest(0, common.myInt(Session["FacilityId"]), common.myInt(ddlBedNo.SelectedValue),
                                                  common.myInt(Session["EncounterId"]), common.myInt(ddlReasonId.SelectedValue),
                                                   common.myStr(txtOtherRemarks.Text).Trim(), "G", false, common.myInt(Session["UserId"]), common.myInt(null));// Akshay
            }
            else
            {
                strMsg = objWard.SaveUpdateHousekeepingRequest(0, common.myInt(Session["FacilityId"]), common.myInt(Request.QueryString["BedId"]),
                                                    common.myInt(Session["EncounterId"]), common.myInt(ddlReasonId.SelectedValue),
                                                    common.myStr(txtOtherRemarks.Text).Trim(), "G", false, common.myInt(Session["UserId"]), common.myInt(null)); // Akshay
            }
            lblMsg.Text = strMsg;
            if (strMsg.ToUpper().Contains(" SUCCESSFULLY") && !strMsg.ToUpper().Contains("USP"))
            {
                ddlReasonId.SelectedIndex = 0;
                txtOtherRemarks.Text = string.Empty;

                lblMsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);

                //string strPopup = "<script language='javascript' ID='script1'>" + "window.close();" + "</script>";

                //ScriptManager.RegisterStartupScript((Page)HttpContext.Current.Handler, typeof(Page), "Script1", strPopup, false);
                //return;

                bindData();
            }


        }
        catch (Exception Ex)
        {
            lblMsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void gvEncounter_OnRowCommand(object Sender, GridViewCommandEventArgs e)
    {
        lblMsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
        BaseC.WardManagement objP = new BaseC.WardManagement();
        string strMsg = string.Empty;
        try
        {
            if (common.myStr(e.CommandName).ToUpper().Equals("REQUESTDELETE"))
            {
                int RequestId = common.myInt(e.CommandArgument);
                if (RequestId > 0)
                {
                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    HiddenField hdnEncounterId = (HiddenField)row.FindControl("hdnEncounterId");

                    strMsg = objP.SaveUpdateHousekeepingRequest(RequestId, common.myInt(Session["FacilityId"]), 0,
                                            common.myInt(hdnEncounterId.Value), 0, string.Empty, "L", false, common.myInt(Session["UserId"]), common.myInt(null));// Akshay

                    lblMsg.Text = strMsg;
                    if (strMsg.ToUpper().Contains(" SUCCESSFULLY") && !strMsg.ToUpper().Contains("USP"))
                    {
                        lblMsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                        bindData();
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            lblMsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMsg.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    protected void gvEncounter_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HiddenField hdnRequestId = (HiddenField)e.Row.FindControl("hdnRequestId");
                ImageButton ibtnDelete = (ImageButton)e.Row.FindControl("ibtnDelete");

                ibtnDelete.Visible = true;

                if (common.myInt(hdnRequestId.Value).Equals(0))
                {
                    ibtnDelete.Visible = false;
                }
            }
        }
        catch (Exception Ex)
        {
            lblMsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMsg.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    protected void gvEncounter_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvEncounter.PageIndex = e.NewPageIndex;
        bindData();
    }

    private void bindData()
    {
        DataSet ds = new DataSet();
        BaseC.WardManagement objP = new BaseC.WardManagement();
        try
        {
            if (common.myBool(ViewState["WardMenu"]))
            {
                ds = objP.getWardHousekeepingRequest(common.myInt(Session["FacilityId"]), 0, 0, string.Empty,
                                                    string.Empty, string.Empty, string.Empty, "R", 0);
            }
            else
            {
                ds = objP.getWardHousekeepingRequest(common.myInt(Session["FacilityId"]), common.myInt(Session["EncounterId"]), 0, string.Empty,
                                       string.Empty, string.Empty, string.Empty, "R", 0);
            }

            if (ds.Tables[0].Rows.Count > 0)
            {
                gvEncounter.DataSource = ds.Tables[0];
            }
            else
            {
                DataRow DR = ds.Tables[0].NewRow();
                ds.Tables[0].Rows.Add(DR);

                gvEncounter.DataSource = ds.Tables[0];
            }

            gvEncounter.DataBind();
        }
        catch (Exception Ex)
        {
            lblMsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            ds.Dispose();
        }
    }

    protected void Bindward()
    {
        try
        {
            BaseC.ATD objbase = new BaseC.ATD(sConString);
            DataSet ds = new DataSet();
            ds = objbase.GetWard(common.myInt(Session["FacilityId"]));
            ddlward.DataSource = ds;
            ddlward.DataTextField = "wardName";
            ddlward.DataValueField = "WardId";
            ddlward.DataBind();

            ddlward.Items.Insert(0, new RadComboBoxItem("All", string.Empty));
            ddlward.Items[0].Value = "0";

        }
        catch (Exception Ex)
        {
            lblMsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }

    }
    protected void ddlward_SelectedIndexChanged(object sender, EventArgs e)
    {
        Hashtable hsinput = new Hashtable();
        DataSet objds = new DataSet();
        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

        try
        {
            BindBedName();
        }
        catch (Exception Ex)
        {
            lblMsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }

    }

    private void BindBedName()
    {
        Hashtable hsinput = new Hashtable();
        DataSet objds = new DataSet();
        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

        try
        {
            hsinput.Add("@IntBedCat", 0);
            hsinput.Add("@IntWardno", ddlward.SelectedValue);
            hsinput.Add("@intFacilityId", common.myInt(Session["FacilityId"]));
            hsinput.Add("@intHouseKeepingFlag", 1);
            objds = dl.FillDataSet(CommandType.StoredProcedure, "USPGetVacantERbedStatus", hsinput);
            ddlBedNo.DataSource = objds;
            ddlBedNo.DataTextField = "BedNo";
            ddlBedNo.DataValueField = "BedId";
            ddlBedNo.DataBind();

            ddlBedNo.Items.Insert(0, new RadComboBoxItem("[ Select ]", string.Empty));
            ddlBedNo.Items[0].Value = "0";
        }
        catch (Exception Ex)
        {
            lblMsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            if (objds != null)
            {
                objds.Dispose();
            }
            hsinput = null;
            dl = null;
        }
    }


}