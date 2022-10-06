using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Configuration;
using Telerik.Web.UI;

public partial class EMR_Masters_EMRTemplatesSetup : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();

    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";

        if (common.myStr(Request.QueryString["IsEMRPopUp"]) == "1")
        {
            // Page.MasterPageFile = "/Include/Master/EMRBlankMaster.master";
            Page.MasterPageFile = "/Include/Master/BlankMaster.master";

        }
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

            bindControl();
            ddlSpecialisation_OnSelectedIndexChanged(null, null);
            BindFreeTextUserTemplate();
            BindDDLFreeTextUserTemplate();
        }
    }

    private void bindControl()
    {
        BaseC.clsLISMaster objlis = new BaseC.clsLISMaster(sConString);
        BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
        clsIVF objI = new clsIVF(sConString);

        DataSet ds = new DataSet();
        try
        {
            /**********Facility************/
            ds = objlis.getFacilityList(common.myInt(Session["HospitalLocationID"]), 0, 0, common.myInt(Session["UserID"]));

            ddlFacility.DataValueField = "FacilityId";
            ddlFacility.DataTextField = "FacilityName";
            ddlFacility.DataSource = ds;
            ddlFacility.DataBind();

            if (ddlFacility.Items.Count > 0)
            {
                ddlFacility.SelectedIndex = ddlFacility.Items.IndexOf(ddlFacility.Items.FindByValue(common.myInt(Session["FacilityId"]).ToString()));
            }
            else
            {
                ddlFacility.SelectedIndex = 0;
            }

            /**********Specialisation************/

            ds = objEMR.getSpecialisationMaster(0, common.myInt(Session["HospitalLocationID"]), common.myInt(ddlFacility.SelectedValue), 1);
            ddlSpecialisation.DataValueField = "SpecialisationId";
            ddlSpecialisation.DataTextField = "SpecialisationName";
            ddlSpecialisation.DataSource = ds;
            ddlSpecialisation.DataBind();
            ddlSpecialisation.SelectedIndex = 0;

            ddlSpecialisation.Items.Insert(0, new RadComboBoxItem("All", "0"));

            if (Request.QueryString["SpecialisationId"] != null || common.myInt(Request.QueryString["SpecialisationId"]) > 0)
            {

                ddlSpecialisation.SelectedIndex = ddlSpecialisation.Items.IndexOf(ddlSpecialisation.Items.FindItemByValue(common.myStr(Request.QueryString["SpecialisationId"])));
            }
            else

            {
                ddlSpecialisation.SelectedIndex = 0;
            }




            /**********DPSetting************/
            ddlDPSetting.Items.Insert(0, new RadComboBoxItem("All", "M"));
            ddlDPSetting.Items.Insert(1, new RadComboBoxItem("Right To Left", "R"));
            ddlDPSetting.Items.Insert(2, new RadComboBoxItem("Left To Right", "L"));
            ddlDPSetting.SelectedIndex = 0;

            /**********Template************/

            bindTemplate();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
        finally
        {
            objlis = null;
            objEMR = null;
            objI = null;

            ds.Dispose();
        }
    }

    private void bindTemplate()
    {
        clsIVF objI = new clsIVF(sConString);

        DataSet ds = new DataSet();
        try
        {
            ds = objI.getSingleScreenTemplates(common.myInt(Session["HospitalLocationID"]), common.myInt(ddlFacility.SelectedValue),
                                common.myInt(ddlSpecialisation.SelectedValue), common.myInt(ddlProvider.SelectedValue));

            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    gvTemplates.DataSource = ds.Tables[0];
                    gvTemplates.DataBind();
                }
            }

            if (ds.Tables.Count > 1)
            {
                if (ds.Tables[1].Rows.Count > 0)
                {
                    ViewState["FreeTextUserTemplateData"] = ds.Tables[1];
                    //ddlFreeTextUserTemplate.DataSource = ds.Tables[1];
                    //ddlFreeTextUserTemplate.DataTextField = "TemplateName";
                    //ddlFreeTextUserTemplate.DataValueField = "TemplateId";
                    //ddlFreeTextUserTemplate.DataBind();
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
            objI = null;

            ds.Dispose();
        }
    }

    private bool isSaved()
    {
        bool isSave = true;

        if (common.myInt(ddlFacility.SelectedValue).Equals(0))
        {
            lblMessage.Text = "Please select facility !";
            isSave = false;
            return isSave;
        }
        if (common.myInt(ddlSpecialisation.SelectedValue).Equals(0))
        {
            lblMessage.Text = "Please select specialisation !";
            isSave = false;
            return isSave;
        }
        if (common.myInt(ddlDPSetting.SelectedIndex) > 0 && common.myInt(ddlProvider.SelectedValue).Equals(0))
        {
            lblMessage.Text = "Please select Provider !";
            isSave = false;
            return isSave;
        }
        return isSave;
    }


    protected void ddlSpecialisation_OnSelectedIndexChanged(Object sender, EventArgs e)
    {
        lblMessage.Text = string.Empty;

        bindTemplate();

        BindProvider();
        ddlFreeTextUserTemplate.SelectedIndex = -1;
        ddlFreeTextUserTemplate.Text = string.Empty;
        BindBlankFreeTextTemplates();
    }


    private void BindProvider()
    {
        try
        {
            //Binding Doctor  dropdownlist inside grid
            DataTable dt = new DataTable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            StringBuilder str = new StringBuilder();
            DataSet objDs = new DataSet();


            Hashtable hsIn = new Hashtable();
            hsIn.Add("@iHospitalLocationId", common.myInt(Session["HospitalLocationId"]));
            hsIn.Add("@iUserId", common.myInt(Session["UserId"]));
            hsIn.Add("@intSpecializationId", common.myInt(ddlSpecialisation.SelectedValue));
            hsIn.Add("@intFacilityId", common.myInt(Session["FacilityId"]));
            objDs = objDl.FillDataSet(CommandType.StoredProcedure, "uspGetEmployeeWithResource", hsIn);

            //Cache.Insert("Doctor", objDs, null, DateTime.Now.AddMinutes(60), System.Web.Caching.Cache.NoSlidingExpiration);
            ddlProvider.DataSource = objDs;
            ddlProvider.DataTextField = "DoctorName";
            ddlProvider.DataValueField = "DoctorID";

            ddlProvider.DataBind();
            ddlProvider.Items.Insert(0, new RadComboBoxItem("All", "0"));
            ddlProvider.SelectedIndex = 0;

            //if (objDs.Tables[0].Rows.Count > 1)
            //{
            //    ddlProvider.Enabled = true;
            //}
            //else
            //{
            //    if (common.myBool(Session["isEMRSuperUser"]))
            //    {
            //        ddlProvider.Enabled = true;
            //    }
            //    else
            //    {
            //        ddlProvider.Enabled = false;
            //    }
            //}

        }
        catch (Exception Ex)
        {
            //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private void clearControl()
    {
        lblMessage.Text = "&nbsp;";

        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
    }

    protected void btnNew_OnClick(Object sender, EventArgs e)
    {
        Response.Redirect(common.myStr(Request.Url.AbsoluteUri), false);
    }

    protected void gvTemplates_OnRowDataBound(object o, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            ((CheckBox)e.Row.FindControl("chkAll")).Attributes.Add("onclick", "javascript:SelectAll('" +
                ((CheckBox)e.Row.FindControl("chkAll")).ClientID + "')");

            ((CheckBox)e.Row.FindControl("chkAllManditory")).Attributes.Add("onclick", "javascript:SelectAllManditory('" +
              ((CheckBox)e.Row.FindControl("chkAllManditory")).ClientID + "')");

            ((CheckBox)e.Row.FindControl("chkAllCollapse")).Attributes.Add("onclick", "javascript:SelectAllCollapse('" +
              ((CheckBox)e.Row.FindControl("chkAllCollapse")).ClientID + "')");
        }
        if ((e.Row.RowType == DataControlRowType.DataRow) && (e.Row.RowState == DataControlRowState.Normal || e.Row.RowState == DataControlRowState.Edit) || (e.Row.RowType == DataControlRowType.DataRow) || (e.Row.RowState == DataControlRowState.Alternate))
        {
            CheckBox chkRowManditory = (CheckBox)e.Row.FindControl("chkRowManditory");
            CheckBox chkRowCollapse = (CheckBox)e.Row.FindControl("chkRowCollapse");
            HiddenField hdnId = (HiddenField)e.Row.FindControl("hdnId");
            HiddenField hdnIsMandatory = (HiddenField)e.Row.FindControl("hdnIsMandatory");
            HiddenField hdnIsCollapse = (HiddenField)e.Row.FindControl("hdnIsCollapse");
            HiddenField hdnTemplateCode = (HiddenField)e.Row.FindControl("hdnTemplateCode");

            if (common.myStr(hdnTemplateCode.Value).Equals("OTN") || common.myStr(hdnTemplateCode.Value).Equals("DGN")
                || common.myStr(hdnTemplateCode.Value).Equals("ORD") || common.myStr(hdnTemplateCode.Value).Equals("PRS")
                || common.myStr(hdnTemplateCode.Value).Equals("LAB") || common.myStr(hdnTemplateCode.Value).Equals("FUA"))
            {
                chkRowManditory.Visible = false;
            }

            chkRowManditory.Checked = common.myBool(hdnIsMandatory.Value);
            chkRowCollapse.Checked = common.myBool(hdnIsCollapse.Value);
        }
    }

    protected void btnSaveData_OnClick(Object sender, EventArgs e)
    {
        clsIVF objI = new clsIVF(sConString);
        StringBuilder strXML = new StringBuilder();
        ArrayList coll = new ArrayList();

        StringBuilder strxmlFreeTextTemplateIds = new StringBuilder();
        ArrayList collFreeTextTemplateIds = new ArrayList();
        try
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);

            if (!isSaved())
            {
                return;
            }

            foreach (GridViewRow item in gvTemplates.Rows)
            {
                CheckBox chkRow = (CheckBox)item.FindControl("chkRow");
                HiddenField hdnId = (HiddenField)item.FindControl("hdnId");

                if (chkRow.Checked && common.myInt(hdnId.Value) > 0)
                {
                    CheckBox chkRowManditory = (CheckBox)item.FindControl("chkRowManditory");
                    CheckBox chkRowCollapse = (CheckBox)item.FindControl("chkRowCollapse");

                    coll.Add(common.myInt(common.myInt(hdnId.Value)));
                    coll.Add(common.myBool(chkRowManditory.Checked));
                    coll.Add(common.myBool(chkRowCollapse.Checked));

                    strXML.Append(common.setXmlTable(ref coll));
                }
            }

            foreach (GridViewRow item in gvFreeTextTemplates.Rows)
            {
                CheckBox chkRowFreeTextTemplates = (CheckBox)item.FindControl("chkRowFreeTextTemplates");
                HiddenField hdnTemplateId = (HiddenField)item.FindControl("hdnTemplateId");

                if (chkRowFreeTextTemplates.Checked && common.myInt(hdnTemplateId.Value) > 0)
                {
                    collFreeTextTemplateIds.Add(common.myInt(common.myInt(hdnTemplateId.Value)));
                    collFreeTextTemplateIds.Add(common.myInt(common.myInt(ddlFreeTextUserTemplate.SelectedValue)));
                    strxmlFreeTextTemplateIds.Append(common.setXmlTable(ref collFreeTextTemplateIds));
                }
            }

            string strMsg = objI.SaveEMRSingleScreenUserTemplates(common.myInt(Session["HospitalLocationID"]), common.myInt(ddlFacility.SelectedValue),
                            common.myInt(ddlSpecialisation.SelectedValue), strXML.ToString(), strxmlFreeTextTemplateIds.ToString(), common.myInt(Session["UserID"]), common.myInt(ddlProvider.SelectedValue), common.myStr(ddlDPSetting.SelectedValue));

            if ((strMsg.ToUpper().Contains(" UPDATE") || strMsg.ToUpper().Contains(" SAVE")) && !strMsg.ToUpper().Contains("USP"))
            {
                clearControl();
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                bindTemplate();
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

    protected void ddlProvider_OnSelectedIndexChanged(Object sender, EventArgs e)
    {
        lblMessage.Text = string.Empty;
        bindTemplate();
        ddlFreeTextUserTemplate.SelectedIndex = -1;
        ddlFreeTextUserTemplate.Text = string.Empty;
        BindBlankFreeTextTemplates();
    }

    protected void gvFreeTextTemplates_OnRowDataBound(object o, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                ((CheckBox)e.Row.FindControl("chkAllFreeTextTemplates")).Attributes.Add("onclick", "javascript:SelectAllFreeTextTemplates('" +
                    ((CheckBox)e.Row.FindControl("chkAllFreeTextTemplates")).ClientID + "')");
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        //if ((e.Row.RowType == DataControlRowType.DataRow) && (e.Row.RowState == DataControlRowState.Normal || e.Row.RowState == DataControlRowState.Edit) || (e.Row.RowType == DataControlRowType.DataRow) || (e.Row.RowState == DataControlRowState.Alternate))
        //{

        //}
    }
    protected void ddlFreeTextUserTemplate_OnSelectedIndexChanged(Object sender, EventArgs e)
    {
        BindFreeTextUserTemplate();
    }

    private void BindFreeTextUserTemplate()
    {
        BaseC.clsEMR objemr = new BaseC.clsEMR(sConString);
        DataTable dt = new DataTable();
        try
        {
            if (!common.myInt(ddlFreeTextUserTemplate.SelectedValue).Equals(0))
            {
                dt = objemr.GetFreeTextTemplateSetup(common.myInt(Session["HospitalLocationId"]), common.myInt(ddlFacility.SelectedValue), common.myInt(ddlSpecialisation.SelectedValue), common.myInt(ddlProvider.SelectedValue), common.myInt(ddlFreeTextUserTemplate.SelectedValue));
                if (dt.Rows.Count > 0)
                {
                    gvFreeTextTemplates.DataSource = dt;
                    gvFreeTextTemplates.DataBind();
                }
                else
                {
                    BindBlankFreeTextTemplates();
                }
            }
            else
            {
                BindBlankFreeTextTemplates();
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
            objemr = null;
            if (dt != null)
            {
                dt.Dispose();
            }
        }
    }

    private void BindBlankFreeTextTemplates()
    {
        DataTable dt = CreateItemTable();
        DataRow dr = dt.NewRow();
        try
        {
            dr["IsChk"] = 0;
            dr["TemplateName"] = string.Empty;
            dr["TemplateId"] = 0;

            dt.Rows.Add(dr);
            dt.AcceptChanges();

            gvFreeTextTemplates.DataSource = dt;
            gvFreeTextTemplates.DataBind();
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
            dt.Columns.Add("IsChk", typeof(bool));
            dt.Columns.Add("TemplateName", typeof(string));
            dt.Columns.Add("TemplateId", typeof(int));
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

    private void BindDDLFreeTextUserTemplate()
    {
        DataTable dt = new DataTable();
        try
        {
            if (ViewState["FreeTextUserTemplateData"] != null)
            {
                dt = (DataTable)ViewState["FreeTextUserTemplateData"];
                ddlFreeTextUserTemplate.DataSource = dt;
                ddlFreeTextUserTemplate.DataTextField = "TemplateName";
                ddlFreeTextUserTemplate.DataValueField = "TemplateId";
                ddlFreeTextUserTemplate.DataBind();
            }
        }
        catch (Exception ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + ex.Message;
            objException.HandleException(ex);
        }
    }

    //  ViewState["FreeTextUserTemplateData"] = ds.Tables[1];

    //ddlFreeTextUserTemplate.DataSource = ds.Tables[1];
    //ddlFreeTextUserTemplate.DataTextField = "TemplateName";
    //ddlFreeTextUserTemplate.DataValueField = "TemplateId";
    //ddlFreeTextUserTemplate.DataBind();



}
