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


public partial class EMR_Masters_GroupMaster : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    BaseC.clsEMR objEmr;
    BaseC.EMRMasters objEMRMasters;
    clsExceptionLog objException = new clsExceptionLog();
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
            if (commonLabelSetting.cFont != "")
            {
                lblMessage.Font.Name = commonLabelSetting.cFont;
            }

            objEmr = new BaseC.clsEMR(sConString);
            ViewState["SpecialisationId"] = null;

            bindDetails();

            txtSpecialisationName.Attributes.Add("onkeypress", "return clickEnterInGrid('" + btnSaveData.ClientID + "', event)");

            txtSpecialisationName.Focus();
        }

    }
    private enum GridColgvDetails : byte
    {

        COL0 = 0,
        COL1 = 1,
        COL2 = 2,
        COL3 = 3,
        COL4 = 4,
        COL5 = 5




    }


    private bool isSaved()
    {
        bool isSave = true;
        if (common.myLen(txtSpecialisationName.Text) == 0)
        {
            lblMessage.Text = "Group Name can't be blank !";
            isSave = false;
            return isSave;
        }

        return isSave;
    }
    private void clearControl()
    {
        lblMessage.Text = "&nbsp;";
        txtSpecialisationName.Text = "";
        ddlStatus.SelectedIndex = 0;
        ViewState["SpecialisationId"] = null;
        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
    }
    private void bindDetails()
    {
        DataSet ds = new DataSet();
        try
        {
            //objEmr = new BaseC.clsEMR(sConString);
            //ds = objEmr.getSpecialisationMaster(common.myInt(ViewState["SpecialisationId"]), common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), 0);

            objEMRMasters = new BaseC.EMRMasters(sConString);
            ds = objEMRMasters.GetTemplateGroup(common.myInt(Session["FacilityId"]));

            gvDetails.DataSource = ds.Tables[0];
            gvDetails.DataBind();
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
    protected void btnNew_OnClick(Object sender, EventArgs e)
    {
        Response.Redirect(common.myStr(Request.Url.AbsoluteUri), false);
    }
    protected void btnSaveData_OnClick(Object sender, EventArgs e)
    {
        try
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);

            if (!isSaved())
            {
                return;
            }

            //objEmr = new BaseC.clsEMR(sConString);

            //string strMsg = objEmr.SaveSpecialisationMaster(common.myInt(ViewState["SpecialisationId"]), common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityID"]),
            //                           common.myStr(txtSpecialisationName.Text, true), common.myInt(ddlStatus.SelectedValue), common.myInt(Session["UserID"]));


            BaseC.EMRMasters objEMRMasters_1 = new BaseC.EMRMasters(sConString); ;
            bool bitStatus = false;
            if (ddlStatus.SelectedValue == "1")
            {
                bitStatus = true;
            }
            else if (ddlStatus.SelectedValue == "0")
            {
                bitStatus = false;
            }


            string strMsg = objEMRMasters_1.SaveTemplateGroup(common.myInt(Session["HospitalLocationID"]), common.myInt(hdnGroupid.Text), common.myInt(Session["FacilityID"]), common.myStr(txtSpecialisationName.Text)
                , common.myBool(chkDisplayInMenuEMR.Checked), common.myBool(chkDisplayInMenuNB.Checked), common.myBool(bitStatus), common.myInt(Session["UserID"]), string.Empty, common.myBool(chkPackageGroup.Checked), common.myBool(chkDisplayInWard.Checked));

            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
            if ((strMsg.ToUpper().Contains(" Updated") || strMsg.ToUpper().Contains(" SAVE")) && !strMsg.ToUpper().Contains("USP"))
            {
                clearControl();

                bindDetails();

                txtSpecialisationName.Focus();
            }
            lblMessage.Text = strMsg;
            btnSaveData.Text = "Save";
            bindDetails();

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }
    protected void gvDetails_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        DataSet ds = new DataSet();

        try
        {
            if (e.CommandName == "EDT")
            {
                if (common.myInt(e.CommandArgument) > 0)
                {
                    clearControl();

                    btnSaveData.Text = "Update";

                    int index = common.myInt(e.CommandArgument);

                    GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    Label lblDisplayInEMR = (Label)row.FindControl("lblDisplayInEMR");
                    Label lblDisplayInNurseWorkbench = (Label)row.FindControl("lblDisplayInNurseWorkbench");


                    //rowindex.BackColor = System.Drawing.Color.Yellow;

                    //txtDisplayInEMR.Text = common.myStr(lblDisplayInEMR.Text);
                    //txtDisplayInNurseWorkbench.Text = common.myStr(lblDisplayInNurseWorkbench.Text);

                    //ViewState["SpecialisationId"] = common.myInt(e.CommandArgument);
                    //objEmr = new BaseC.clsEMR(sConString);

                    //ds = objEmr.getSpecialisationMaster(common.myInt(ViewState["SpecialisationId"]), common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), 0);

                    objEMRMasters = new BaseC.EMRMasters(sConString);
                    ds = objEMRMasters.GetTemplateGroup(common.myInt(Session["FacilityId"]));
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DataView dv = ds.Tables[0].DefaultView;
                        dv.RowFilter = "Groupid=" + common.myInt(e.CommandArgument);
                        if (dv.ToTable().Rows.Count > 0)
                        {
                            DataRow DR = dv.ToTable().Rows[0];
                            hdnGroupid.Text = common.myStr(e.CommandArgument);

                            txtSpecialisationName.Text = common.myStr(DR["GroupName"]);

                            if (common.myInt(DR["DisplayInEMR"]) > 0)
                            {
                                chkDisplayInMenuEMR.Checked = true;

                            }
                            else
                            {
                                chkDisplayInMenuEMR.Checked = false;

                            }
                            if (common.myInt(DR["DisplayInNurseWorkbench"]) > 0)
                            {
                                chkDisplayInMenuNB.Checked = true;
                            }
                            else
                            {
                                chkDisplayInMenuNB.Checked = false;
                            }

                            if (common.myBool(DR["IsPackageTemplate"]))
                            {
                                chkPackageGroup.Checked = true;

                            }
                            else
                            {
                                chkPackageGroup.Checked = false;
                            }


                            chkDisplayInWard.Checked = (common.myBool(DR["DisplayInWard"]) == true ? true : false);
                            ddlStatus.SelectedValue = common.myStr(DR["ActiveStatus"]) == "Active" ? "1" : "0";


                        }
                    }
                }

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

    protected void gvDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        ViewState["SpecialisationId"] = "0";
        clearControl();

        gvDetails.PageIndex = e.NewPageIndex;
        bindDetails();
    }
    protected void gvDetails_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
        {
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    e.Row.Cells[(byte)GridColgvDetails.COL1].Visible = false;
                    e.Row.Cells[(byte)GridColgvDetails.COL3].Visible = false;
                    e.Row.Cells[(byte)GridColgvDetails.COL4].Visible = false;
                }
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Cells[(byte)GridColgvDetails.COL1].Visible = false;
                    e.Row.Cells[(byte)GridColgvDetails.COL3].Visible = false;
                    e.Row.Cells[(byte)GridColgvDetails.COL4].Visible = false;
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
    protected void gvDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }
}
